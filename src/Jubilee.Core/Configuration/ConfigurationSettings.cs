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
		public IEnumerable<TaskConfiguration> Tasks { get; protected set; }
		public IEnumerable<NotificationConfiguration> Notifications { get; protected set; }
        public IEnumerable<string> NonDependentTasks
        {
            get
            {
                return Tasks.Where(x => String.IsNullOrEmpty(x.DependsOn)).Select(x => x.Task);
            }
        }
		public ConfigurationSettings() { }
		public ConfigurationSettings(RunnerConfiguration runnerConfiguration, IEnumerable<TaskConfiguration> tasksConfiguration, IEnumerable<NotificationConfiguration> notificationsConfiguration)
		{
			this.Runner = runnerConfiguration;
			this.Tasks = tasksConfiguration;
			this.Notifications = notificationsConfiguration;

		}
	}
}
