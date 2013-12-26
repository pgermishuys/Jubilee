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
		public PluginProvider(IKernel kernel)
		{
			this.kernel = kernel;
		}
		public IEnumerable<Plugins.IPlugin> GetAll()
		{
			return kernel.GetAll<IPlugin>();
		}

		public IEnumerable<IPlugin> GetDependentPluginsOn(Plugins.IPlugin plugin)
		{
			var openGenericType = typeof(IDependsOnPlugin<>);
			var closedGenericType = openGenericType.MakeGenericType(plugin.GetType());
			return kernel.GetAll(closedGenericType).Select(pluginObject => { 
				return (IPlugin)pluginObject; 
			});
		}
	}
}
