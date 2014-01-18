using Jubilee.Core.Notifications;
using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	internal class VerboseOutputPluginRunner : ITaskRunner
	{
		private ITaskRunner pluginRunner;
		private INotificationService notificationService;
		public VerboseOutputPluginRunner(ITaskRunner pluginRunner, INotificationService notificationService)
		{
			this.pluginRunner = pluginRunner;
			this.notificationService = notificationService;
		}

		public bool RunTask(ITask pluginToRun)
		{
			notificationService.Notify(pluginToRun.GetType().Name, "Running", NotificationType.Information);
			var succeeded = pluginToRun.Run();
			notificationService.Notify(pluginToRun.GetType().Name, succeeded ? "Succeeded" : "Failed", NotificationType.Information);
			return succeeded;
		}
	}
}
