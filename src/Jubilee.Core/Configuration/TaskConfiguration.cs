using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class TaskConfiguration
	{
		public string Task { get; protected set; }
        public string Name { get; protected set; }
		public Dictionary<string, object> Parameters { get; set; }
		public string DependsOn { get; protected set; }
		public TaskConfiguration() {
		}
        public TaskConfiguration(string name, string task, Dictionary<string, object> parameters, string dependsOn)
		{
            this.Name = name;
            this.Task = task;
			this.Parameters = parameters;
			this.DependsOn = dependsOn;
		}
	}
}
