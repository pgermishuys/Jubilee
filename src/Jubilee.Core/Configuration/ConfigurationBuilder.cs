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
			this.serializer = new Jubilee.Core.Serialization.YamlSerializer();
			this.kernel = new StandardKernel();
		}
		public ConfigurationBuilder(string settingsFilePath) : this()
		{
			if (!File.Exists(settingsFilePath))
			{
				throw new Exception(String.Format("The settings file provided does not exist in the following location : {0}", settingsFilePath));
			}
			var configurationSettings = serializer.Deserialize<ConfigurationSettings>(File.ReadAllText(settingsFilePath));
			
			var types = scanner.GetTypes(AppDomain.CurrentDomain.BaseDirectory, "*.dll", typeof(IPlugin), typeof(INotificationPlugin), typeof(IRunner));

			var runnerType = types.GetType(configurationSettings.RunnerConfiguration.Name);
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
			var plugin = knownTypes.GetType(pluginConfiguration.Name);
			kernel.Bind<IPlugin>().To(plugin);
			foreach (var pluginConfig in pluginsConfiguration)
			{
				var openGenericPluginType = typeof(IDependsOnPlugin<>);
				var closedGenericPluginType = openGenericPluginType.MakeGenericType(plugin);
				if (!String.IsNullOrEmpty(pluginConfig.Parameters))
				{
					kernel.Bind(closedGenericPluginType).To(knownTypes.First(x => x.Name.StartsWith(pluginConfig.Name))).WithConstructorArgument("parameters", pluginConfig.Parameters);
				}
				else
				{
					kernel.Bind(closedGenericPluginType).To(knownTypes.First(x => x.Name.StartsWith(pluginConfig.Name)));
				}
			}
		}
		public void RegisterNotification(Type[] knownTypes, NotificationConfiguration notificationConfiguration)
		{
			var notificationType = knownTypes.GetType(notificationConfiguration.Name);
			kernel.Bind<INotificationPlugin>().To(notificationType);
		}
		public IRunner Build()
		{
			kernel.Bind<INotificationService>().To<NotificationService>();
			return kernel.Get<IRunner>();
		}
	}
}
