using Jubilee.Core.Notifications;
using Jubilee.Core.Notifications.Plugins;
using Jubilee.Core.Process.Plugins;
using Jubilee.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Jubilee.Core.Extensions;
using Jubilee.Core.Scanners;

namespace Jubilee.Core.Configuration
{
	public class ConfigurationBuilder
	{
		private IKernel kernel;
		private ISerializer serializer;
		private IScanner scanner;
		public ConfigurationBuilder()
		{
			this.scanner = new AssemblyScanner();
			this.serializer = new Jubilee.Core.Serialization.JsonSerializer();
			this.kernel = new StandardKernel();
		}
		public ConfigurationBuilder(string settingsFilePath) :this()
		{
			if (!File.Exists(settingsFilePath))
			{
				throw new Exception(String.Format("The settings file provided does not exist in the following location : {0}", settingsFilePath));
			}
			var configurationSettings = serializer.Deserialize<ConfigurationSettings>(File.ReadAllText(settingsFilePath));
			
			var types = scanner.GetTypes(AppDomain.CurrentDomain.BaseDirectory, "*.dll", typeof(IPlugin), typeof(INotificationPlugin), typeof(IRunner));

			var runnerType = types.First(x => x.Name == configurationSettings.RunnerConfiguration.Name);
			kernel.Bind<IRunner>().To(runnerType);

			foreach (var notificationConfiguration in configurationSettings.NotificationsConfiguration)
			{
				RegisterNotification(types, notificationConfiguration);
			}

			foreach (var pluginConfiguration in configurationSettings.PluginsConfiguration)
			{
				RegisterPlugin(types, pluginConfiguration, pluginConfiguration.DependentPlugins);
			}
		}
		public void RegisterPlugin(Type[] knownTypes, PluginConfiguration pluginConfiguration, IEnumerable<PluginConfiguration> pluginsConfiguration)
		{
			var plugin = knownTypes.First(x => x.Name == pluginConfiguration.Name);
			kernel.Bind<IPlugin>().To(plugin);
			foreach (var pluginConfig in pluginsConfiguration)
			{
				var openGenericPluginType = typeof(IDependsOnPlugin<>);
				var closedGenericPluginType = openGenericPluginType.MakeGenericType(plugin);
				kernel.Bind(closedGenericPluginType).To(knownTypes.First(x=>x.Name.StartsWith(pluginConfig.Name)));
			}
		}
		public void RegisterNotification(Type[] knownTypes, NotificationConfiguration notificationConfiguration)
		{
			var notificationType = knownTypes.First(x => x.Name.StartsWith(notificationConfiguration.Name));
			kernel.Bind<INotificationPlugin>().To(notificationType);
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
