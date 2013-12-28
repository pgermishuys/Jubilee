using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	internal class PluginRunner : IPluginRunner
	{
		public bool RunPlugin(IPlugin plugin)
		{
			return plugin.Run();
		}
	}
}
