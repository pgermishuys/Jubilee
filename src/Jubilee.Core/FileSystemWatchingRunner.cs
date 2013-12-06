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
		private IPlugin[] plugins;
		private FileSystemWatcher watcher;
		private string workingPath;
		private INotificationService notificationService;
		private IKernel kernel;
		static string[] fileExtensionsWhiteList = new string[] { ".cs", ".coffee", ".rb", ".html", ".cshtml", ".js", ".css", ".fs" };
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
				foreach (var plugin in kernel.GetAll<IPlugin>())
				{
					Run(plugin, plugins, Path.GetDirectoryName(e.FullPath));
				}
				watcher.EnableRaisingEvents = true;
			}
		}

		private void Run(IPlugin plugin, IEnumerable<IPlugin> plugins, string workingPath)
		{
            plugin.AddParameter(new KeyValuePair<string, object>("WorkingPath", workingPath));
			var canProcessDependentPlugins = plugin.Run();
			if (!canProcessDependentPlugins)
				return;
			var openGenericType = typeof(IDependsOnPlugin<>);
			var closedGenericType = openGenericType.MakeGenericType(plugin.GetType());
			var dependentPlugins = kernel.GetAll(closedGenericType);
			foreach (dynamic dependentPlugin in dependentPlugins)
			{
                dependentPlugin.AddParameter(new KeyValuePair<string, object>("WorkingPath", workingPath));
				dependentPlugin.Run();
			}
		}
	}
}
