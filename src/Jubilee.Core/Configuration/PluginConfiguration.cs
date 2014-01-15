using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class PluginConfiguration
	{
		public string Name { get; protected set; }
		public Dictionary<string, object> Parameters { get; set; }
		public string DependsOn { get; protected set; }
		public PluginConfiguration() {
		}
        public PluginConfiguration(string name, Dictionary<string, object> parameters, string dependsOn)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.DependsOn = dependsOn;
		}
	}
}
