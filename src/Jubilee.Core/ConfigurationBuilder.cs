using Jubilee.Core.Notifications;
using Jubilee.Core.Notifications.Plugins;
using Jubilee.Core.Process.Plugins;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core
{
	public class ConfigurationBuilder
	{
		private IKernel kernel;
		public ConfigurationBuilder()
		{
			kernel = new StandardKernel();
		}
		public ConfigurationBuilder ScanAssembliesForPlugins(params string[] assemblyNames)
		{
			foreach (var assemblyName in assemblyNames)
			{
				if (File.Exists(assemblyName))
				{
					var loadedAssembly = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName));
					var plugins = loadedAssembly.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x) && x.IsClass);
					foreach (var plugin in plugins)
					{
						kernel.Bind<IPlugin>().To(plugin);
					}
				}
			}
			return this;
		}
		public ConfigurationBuilder WithPlugin<T>() where T : IPlugin
		{
			kernel.Bind<IPlugin>().To(typeof(T));
			return this;
		}
		public ConfigurationBuilder WithNotificationPlugin<T>() where T : INotificationPlugin
		{
			kernel.Bind<INotificationPlugin>().To(typeof(T));
			return this;
		}
		public ConfigurationBuilder WithRunner<T>() where T : IRunner
		{
			kernel.Bind<IRunner>().To(typeof(T)); 
			return this;
		}
		public IRunner Build()
		{
			kernel.Bind<INotificationService>().To<NotificationService>();
			return kernel.Get<IRunner>();
		}
	}
}
