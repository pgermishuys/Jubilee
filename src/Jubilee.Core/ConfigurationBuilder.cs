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
		private IKernel kernel;
		public ConfigurationBuilder()
		{
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
