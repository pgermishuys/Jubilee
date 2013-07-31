using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class ConfigurationSettings
	{
		public RunnerConfiguration RunnerConfiguration { get; protected set; }
		public IEnumerable<PluginConfiguration> PluginsConfiguration { get; protected set; }
		public IEnumerable<NotificationConfiguration> NotificationsConfiguration { get; protected set; }
		public ConfigurationSettings() { }
		public ConfigurationSettings(RunnerConfiguration runnerConfiguration, IEnumerable<PluginConfiguration> pluginsConfiguration, IEnumerable<NotificationConfiguration> notificationsConfiguration)
		{
			this.RunnerConfiguration = runnerConfiguration;
			this.PluginsConfiguration = pluginsConfiguration;
			this.NotificationsConfiguration = notificationsConfiguration;

		}
	}
}
