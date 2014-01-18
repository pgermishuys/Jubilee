using Jubilee.Core.Notifications;
using Jubilee.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	internal class VerboseOutputPluginRunner : ITaskRunner
	{
		private ITaskRunner taskRunner;
		private INotificationService notificationService;
		public VerboseOutputPluginRunner(ITaskRunner taskRunner, INotificationService notificationService)
		{
			this.taskRunner = taskRunner;
			this.notificationService = notificationService;
		}

		public bool RunTask(ITask taskToRun)
		{
			notificationService.Notify(taskToRun.GetType().Name, "Running", NotificationType.Information);
			var succeeded = taskToRun.Run();
			notificationService.Notify(taskToRun.GetType().Name, succeeded ? "Succeeded" : "Failed", NotificationType.Information);
			return succeeded;
		}
	}
}
