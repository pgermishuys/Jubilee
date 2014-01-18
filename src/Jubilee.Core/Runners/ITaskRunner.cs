using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	internal interface ITaskRunner
	{
		bool RunTask(ITask pluginToRun);
	}
}
