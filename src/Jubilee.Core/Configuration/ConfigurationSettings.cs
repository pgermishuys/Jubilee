using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class ConfigurationSettings
	{
		public RunnerConfiguration Runner { get; protected set; }
		public IEnumerable<PluginConfiguration> Plugins { get; protected set; }
		public IEnumerable<NotificationConfiguration> Notifications { get; protected set; }
		public ConfigurationSettings() { }
		public ConfigurationSettings(RunnerConfiguration runnerConfiguration, IEnumerable<PluginConfiguration> pluginsConfiguration, IEnumerable<NotificationConfiguration> notificationsConfiguration)
		{
			this.Runner = runnerConfiguration;
			this.Plugins = pluginsConfiguration;
			this.Notifications = notificationsConfiguration;

		}
	}
}
