using Jubilee.Core.Process.Plugins;
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

namespace Jubilee.Core
{
	public class FileSystemWatchingRunner : IRunner
	{
		private IPluginProvider pluginProvider;
		private FileSystemWatcher watcher;
		private string workingPath;
		private INotificationService notificationService;
		static string[] fileExtensionsWhiteList = new string[] { ".cs", ".coffee", ".rb", ".html", ".cshtml", ".js", ".css", ".fs" };
		public FileSystemWatchingRunner(INotificationService notificationService, IPluginProvider pluginProvider)
		{
			this.notificationService = notificationService;
			this.pluginProvider = pluginProvider;
		}

		public void Run(string workingPath, string filePatternToWatch = "*.*")
		{
			this.workingPath = workingPath;
			watcher = new FileSystemWatcher(workingPath, filePatternToWatch);
			watcher.IncludeSubdirectories = true;
			watcher.EnableRaisingEvents = true;
			watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Changed += new FileSystemEventHandler(FileSystemChanged);
			watcher.Created += new FileSystemEventHandler(FileSystemChanged);
			watcher.Renamed += FileSystemChanged;

			notificationService.Notify(String.Format("Watching {0}", workingPath));
		}

		private void FileSystemChanged(object sender, FileSystemEventArgs e)
		{
			if (fileExtensionsWhiteList.Contains(Path.GetExtension(e.FullPath)) && System.IO.File.Exists(e.FullPath))
			{
				watcher.EnableRaisingEvents = false;
				foreach (var plugin in pluginProvider.GetAll())
				{
					Run(plugin, new Context(Path.GetDirectoryName(e.FullPath), e.FullPath));
				}
				watcher.EnableRaisingEvents = true;
			}
		}

		private void Run(IPlugin plugin, Context context)
		{
			AddParametersForPlugin(plugin, context.ToDictionary());
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
