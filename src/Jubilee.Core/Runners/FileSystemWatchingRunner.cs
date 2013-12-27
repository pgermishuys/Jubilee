using Jubilee.Core.Plugins;
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
		private IPluginProvider pluginProvider;
		private FileSystemWatcher fileSystemWatcher;
		private string folderToWatch;
		private INotificationService notificationService;
		static string[] fileExtensionsWhiteList = new string[] { ".cs", ".coffee", ".rb", ".html", ".cshtml", ".js", ".css", ".fs" };
		public FileSystemWatchingRunner(INotificationService notificationService, IPluginProvider pluginProvider)
		{
			this.notificationService = notificationService;
			this.pluginProvider = pluginProvider;
		}

		public override bool Run()
		{
			Guard.AgainstException<RuntimeBinderException>(() => 
				this.folderToWatch = parameters.FolderToWatch,
				"The FolderToWatch parameter was not found in the parameters. Ensure that the configuration file is correct.", 
				notificationService);

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
				foreach (var plugin in pluginProvider.GetAll())
				{
					Run(plugin, new Context(Path.GetDirectoryName(e.FullPath), e.FullPath));
				}
				fileSystemWatcher.EnableRaisingEvents = true;
			}
		}

		private void Run(IPlugin plugin, Context context)
		{
			AddParametersForPlugin(plugin, context.ToDictionary());
			notificationService.Notify(plugin.GetType().Name, "Running");
			var canProcessDependentPlugins = plugin.Run();
			if (!canProcessDependentPlugins)
				return;
			var dependentPlugins = pluginProvider.GetDependentPluginsOn(plugin);
			foreach (dynamic dependentPlugin in dependentPlugins)
			{
				AddParametersForPlugin(dependentPlugin, context.ToDictionary());
				dependentPlugin.Run();
			}
		}

		private void AddParametersForPlugin(IPlugin plugin, Dictionary<string, object> parameters)
		{
			plugin.AddParameters(parameters);
		}

		private class Context
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
