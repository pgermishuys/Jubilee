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
using Jubilee.Core.Workflow.Tasks;

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

			kernel.Bind<ITaskProvider>().To<TaskProvider>();

			var configurationSettings = serializer.Deserialize<ConfigurationSettings>(File.ReadAllText(settingsFilePath));

            kernel.Bind<ConfigurationSettings>().ToConstant(configurationSettings);

			var types = scanner.GetTypes(AppDomain.CurrentDomain.BaseDirectory, "*.dll", typeof(ITask), typeof(INotificationPlugin), typeof(IRunner));

			var runnerType = types.GetType(configurationSettings.Runner.Name);
			kernel.Bind<IRunner>().To(runnerType).OnActivation((activatedTask) =>
			{
				((dynamic)activatedTask).Initialise(configurationSettings.Runner.Parameters);
			});

			foreach (var notificationConfiguration in configurationSettings.Notifications)
			{
				RegisterNotification(types, notificationConfiguration);
			}

			foreach (var taskConfiguration in configurationSettings.Tasks)
			{
				RegisterTask(types, taskConfiguration, taskConfiguration.DependsOn);
			}
		}
		public void RegisterTask(Type[] knownTypes, TaskConfiguration taskConfiguration, string dependsOn)
		{
			var task = knownTypes.GetType(taskConfiguration.Task);
			kernel.Bind<ITask>().To(task).Named(taskConfiguration.Name).OnActivation((activatedTask) =>
			{
				((dynamic)activatedTask).Initialise(taskConfiguration.Name, taskConfiguration.Parameters);
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
