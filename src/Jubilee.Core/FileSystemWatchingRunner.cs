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
using Jubilee.Core.Process.Plugins.Extensions;
using Jubilee.Core.Notifications;

namespace Jubilee.Core
{
	public class FileSystemWatchingRunner : IRunner
	{
		private IPlugin[] plugins;
		private IKernel kernel;
		private FileSystemWatcher watcher;
		private string workingPath;
		private INotificationService notificationService;

		public FileSystemWatchingRunner(IKernel kernel, INotificationService notificationService, IPlugin[] plugins)
		{
			this.kernel = kernel;
			this.notificationService = notificationService;
			this.plugins = plugins;
		}

		public void Run(string workingPath, string filePatternToWatch = "*.*")
		{
			this.workingPath = workingPath;
			watcher = new FileSystemWatcher(workingPath, filePatternToWatch);
			watcher.IncludeSubdirectories = true;
			watcher.EnableRaisingEvents = true;
			watcher.NotifyFilter = NotifyFilters.LastWrite;
			watcher.Changed += new FileSystemEventHandler(FileSystemChanged);
			watcher.Created += new FileSystemEventHandler(FileSystemChanged);

			notificationService.Notify(String.Format("Watching {0}", workingPath));
		}

		private void FileSystemChanged(object sender, FileSystemEventArgs e)
		{
			watcher.EnableRaisingEvents = false;
			foreach (var plugin in plugins.NonDependentPlugins())
			{
				Run(plugin, plugins, workingPath);
			};
			watcher.EnableRaisingEvents = true;
		}

		private void Run(IPlugin plugin, IEnumerable<IPlugin> plugins, string workingPath)
		{
			var canProcessDependentPlugins = plugin.Process(workingPath);
			if (!canProcessDependentPlugins)
				return;

			foreach (var dependentPlugin in plugin.DependentPlugins(plugins))
			{
				Run(dependentPlugin, plugins, workingPath);
			}
		}
	}
}
