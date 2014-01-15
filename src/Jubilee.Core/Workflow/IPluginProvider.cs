using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process
{
	public interface IPluginProvider
	{
		IEnumerable<IPlugin> GetNonDependentPlugins();
		IEnumerable<IPlugin> GetPluginsThatDependOn(IPlugin plugin);
	}
}
