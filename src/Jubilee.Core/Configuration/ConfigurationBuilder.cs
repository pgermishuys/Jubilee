using Jubilee.Core.Notifications;
using Jubilee.Core.Plugins;
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
using Jubilee.Core.Process;
using Jubilee.Core.Runners;
using Jubilee.Core.Workflow.Plugins;

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
		public ConfigurationBuilder(string settingsFilePath)
			: this()
		{
			if (!File.Exists(settingsFilePath))
			{
				throw new Exception(String.Format("The settings file provided does not exist in the following location : {0}", settingsFilePath));
			}

			kernel.Bind<IPluginProvider>().To<PluginProvider>();

			var configurationSettings = serializer.Deserialize<ConfigurationSettings>(File.ReadAllText(settingsFilePath));

			var types = scanner.GetTypes(AppDomain.CurrentDomain.BaseDirectory, "*.dll", typeof(IPlugin), typeof(INotificationPlugin), typeof(IRunner));

			var runnerType = types.GetType(configurationSettings.Runner.Name);
			kernel.Bind<IRunner>().To(runnerType).OnActivation((activatedPlugin) =>
			{
				((dynamic)activatedPlugin).Initialise(configurationSettings.Runner.Parameters);
			});

			foreach (var notificationConfiguration in configurationSettings.Notifications)
			{
				RegisterNotification(types, notificationConfiguration);
			}

			foreach (var pluginConfiguration in configurationSettings.Plugins)
			{
				RegisterPlugin(types, pluginConfiguration, pluginConfiguration.DependentPlugins);
			}
		}
		public void RegisterPlugin(Type[] knownTypes, PluginConfiguration pluginConfiguration, IEnumerable<PluginConfiguration> pluginsConfiguration)
		{
			if (pluginConfiguration.Name.EndsWith("csx"))
			{
				RegisterScriptCSPlugin(pluginConfiguration);
			}
			else
			{
				var plugin = knownTypes.GetType(pluginConfiguration.Name);
				kernel.Bind<IPlugin>().To(plugin).OnActivation((activatedPlugin) =>
				{
					((dynamic)activatedPlugin).Initialise(pluginConfiguration.Parameters);
				});
				foreach (var pluginConfig in pluginsConfiguration)
				{
					var openGenericPluginType = typeof(IDependsOnPlugin<>);
					var closedGenericPluginType = openGenericPluginType.MakeGenericType(plugin);
					kernel.Bind(closedGenericPluginType).To(knownTypes.First(x => x.Name == pluginConfig.Name)).OnActivation((activatedPlugin) =>
					{
						((dynamic)activatedPlugin).Initialise(pluginConfig.Parameters);
					});
				}
			}
		}

		private void RegisterScriptCSPlugin(PluginConfiguration pluginConfiguration)
		{
			kernel.Bind<IPlugin>().To<ScriptCSPlugin>().OnActivation((activatedPlugin) =>
			{
				((dynamic)activatedPlugin).Initialise(pluginConfiguration.Parameters);
				((dynamic)activatedPlugin).AddParameter("ScriptName", pluginConfiguration.Name);
			});
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
