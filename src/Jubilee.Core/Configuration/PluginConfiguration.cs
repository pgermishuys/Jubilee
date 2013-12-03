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
		public string Parameters { get; set; }
		public IEnumerable<PluginConfiguration> DependentPlugins { get; protected set; }
		public PluginConfiguration() { }
		public PluginConfiguration(string name, string parameters, params PluginConfiguration[] dependentPlugins)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.DependentPlugins = dependentPlugins;
		}
	}
}
