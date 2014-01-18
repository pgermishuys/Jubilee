﻿using Jubilee.Core.Tasks;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jubilee.Core.Notifications;
using Jubilee.Core.Process;
using Microsoft.CSharp.RuntimeBinder;
using Jubilee.Core.Guards;

namespace Jubilee.Core.Runners
{
	public class FileSystemWatchingRunner : Runner
	{
		private ITaskProvider taskProvider;
		private FileSystemWatcher fileSystemWatcher;
		private string folderToWatch;
		private INotificationService notificationService;
		private ITaskRunner taskRunner;
		static string[] fileExtensionsWhiteList = new string[] { ".cs", ".coffee", ".rb", ".html", ".cshtml", ".js", ".css", ".fs" };
		public FileSystemWatchingRunner(INotificationService notificationService, ITaskProvider taskProvider)
		{
			this.notificationService = notificationService;
			this.taskProvider = taskProvider;
			this.taskRunner = new TaskRunner();
		}

		public override bool Run()
		{
			Guard.AgainstException<RuntimeBinderException>(() =>
				this.folderToWatch = parameters.FolderToWatch,
				"The FolderToWatch parameter was not found in the parameters. Ensure that the configuration file is correct.");

			Guard.Catch<RuntimeBinderException>(() =>
			{
				if (bool.Parse(parameters.VerboseOutput))
				{
					taskRunner = new VerboseOutputPluginRunner(taskRunner, notificationService);
				}
			});

			if (!Directory.Exists(folderToWatch))
			{
				notificationService.Notify(this.GetType().Name, String.Format("The directory {0} does not exist and cannot be watched", folderToWatch), NotificationType.Information);
				return false;
			}

			fileSystemWatcher = new FileSystemWatcher(folderToWatch, "*.*");
			fileSystemWatcher.IncludeSubdirectories = true;
			fileSystemWatcher.EnableRaisingEvents = true;
			fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			fileSystemWatcher.Changed += new FileSystemEventHandler(FileSystemChanged);
			fileSystemWatcher.Created += new FileSystemEventHandler(FileSystemChanged);
			fileSystemWatcher.Renamed += FileSystemChanged;

			notificationService.Notify(String.Format("watching {0}", folderToWatch));
			return true;
		}

		private void FileSystemChanged(object sender, FileSystemEventArgs e)
		{
			if (fileExtensionsWhiteList.Contains(Path.GetExtension(e.FullPath)) && System.IO.File.Exists(e.FullPath))
			{
				fileSystemWatcher.EnableRaisingEvents = false;
				foreach (var task in taskProvider.GetNonDependentTasks())
				{
					Run(task, new Context(Path.GetDirectoryName(e.FullPath), e.FullPath));
				}
				fileSystemWatcher.EnableRaisingEvents = true;
			}
		}

		private void Run(ITask task, Context context)
		{
			AddParametersForTask(task, context.ToDictionary());

			var canContinue = taskRunner.RunTask(task);

			if (!canContinue)
				return;

			var dependentTasks = taskProvider.GetTasksThatDependOn(task);

			foreach (ITask dependentTask in dependentTasks)
			{
                Run(dependentTask, context);
			}
		}

		private void AddParametersForTask(ITask task, Dictionary<string, object> parameters)
		{
			task.AddParameters(parameters);
		}

		internal class Context
		{
			public string WorkingPath { get; set; }
			public string ChangedFilePath { get; set; }
			public Context(string workingPath, string changedFilePath)
			{
				this.WorkingPath = workingPath;
				this.ChangedFilePath = changedFilePath;
			}
			public Dictionary<string, object> ToDictionary()
			{
				var dictionary = new Dictionary<string, object>();
				foreach (var property in this.GetType().GetProperties())
				{
					dictionary.Add(property.Name, property.GetValue(this));
				}
				return dictionary;
			}
		}
	}
}
