using Jubilee.Core.Notifications;
using Jubilee.Core.Notifications.Plugins;
using Jubilee.Core.Process.Plugins;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core
{
	public class ConfigurationBuilder
	{
		private List<Type> plugins;
		private List<Type> notificationPlugins;
		private Type runner;
		private IKernel kernel;
		public ConfigurationBuilder()
		{
			plugins = new List<Type>();
			notificationPlugins = new List<Type>();
			kernel = new StandardKernel();
		}
		public ConfigurationBuilder ScanAssembliesForPlugins(string[] assemblyNames)
		{
			foreach (var assemblyName in assemblyNames)
			{
				
			}
			return this;
		}
		public ConfigurationBuilder WithPlugin<T>() where T : IPlugin
		{
			this.plugins.Add(typeof(T));
			return this;
		}
		public ConfigurationBuilder WithNotificationPlugin<T>() where T : INotificationPlugin
		{
			this.notificationPlugins.Add(typeof(T));
			return this;
		}
		public ConfigurationBuilder WithRunner<T>() where T : IRunner
		{
			this.runner = typeof(T); 
			return this;
		}
		public IRunner Build()
		{
			foreach (var plugin in plugins)
			{
				kernel.Bind<IPlugin>().To(plugin);
			}
			foreach (var notificationPlugin in notificationPlugins)
			{
				kernel.Bind<INotificationPlugin>().To(notificationPlugin);
			}
			kernel.Bind<IRunner>().To(runner);
			kernel.Bind<INotificationService>().To<NotificationService>();
			return kernel.Get<IRunner>();
		}
	}
}
