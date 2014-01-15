using Jubilee.Core.Configuration;
using Jubilee.Core.Plugins;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process
{
	public class PluginProvider : IPluginProvider
	{
		private IKernel kernel;
        private ConfigurationSettings configurationSettings;
		public PluginProvider(IKernel kernel, ConfigurationSettings configurationSettings)
		{
			this.kernel = kernel;
            this.configurationSettings = configurationSettings;
		}
		public IEnumerable<IPlugin> GetNonDependentPlugins()
		{
			return kernel.GetAll<IPlugin>().Where(plugin => {
                return configurationSettings.NonDependentPlugins.Contains(plugin.GetType().Name);
            });
		}

		public IEnumerable<IPlugin> GetPluginsThatDependOn(IPlugin plugin)
		{
            var pluginsThatDependOn = configurationSettings.Plugins.Where(x => x.DependsOn == plugin.GetType().Name);
            return kernel.GetAll<IPlugin>().Where(x => pluginsThatDependOn.Count(pluginConfig => pluginConfig.Name == x.GetType().Name) > 0);
		}
	}
}
