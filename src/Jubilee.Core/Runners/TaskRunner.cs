using Jubilee.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	internal class TaskRunner : ITaskRunner
	{
		public bool RunTask(ITask task)
		{
			return task.Run();
		}
	}
}
