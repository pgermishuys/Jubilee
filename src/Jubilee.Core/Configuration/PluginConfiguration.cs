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
		public IEnumerable<PluginConfiguration> DependentPlugins { get; protected set; }
		public PluginConfiguration() {
			DependentPlugins = new PluginConfiguration[] { };
		}
		public PluginConfiguration(string name, Dictionary<string, object> parameters, params PluginConfiguration[] dependentPlugins)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.DependentPlugins = dependentPlugins;
		}
	}
}
