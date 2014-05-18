using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using process = System.Diagnostics;
using Jubilee.Core.Notifications;
using Newtonsoft.Json;
using Jubilee.Core.Tasks;

namespace Jubilee.Core.Workflow.Tasks
{
	public class ScriptCS : Task
	{
		private INotificationService notificationService;
		public ScriptCS(INotificationService notificationService)
		{
			this.notificationService = notificationService;
		}
		public override bool Run()
		{
			process.Process process = new process.Process();
			process.StartInfo = new process.ProcessStartInfo("scriptcs", String.Format("{0} -- {1}", String.Format("\"{0}\"", parameters.ScriptName), String.Join(" ", ((IDictionary<string, object>)parameters).Select(x => x.Value))));
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			process.WaitForExit();

			string errors = process.StandardError.ReadToEnd();
			string output = process.StandardOutput.ReadToEnd();

			if (!String.IsNullOrEmpty(errors))
			{
				notificationService.Notify(parameters.ScriptName, errors, NotificationType.Error);
				return false;
			}
			if (output.ToLowerInvariant().Contains("error"))
			{
				notificationService.Notify(parameters.ScriptName, output, NotificationType.Error);
				return false;
			}
			else if(!String.IsNullOrEmpty(output))
			{
				notificationService.Notify(parameters.ScriptName, output, NotificationType.Information);
			}
			return true;
		}
	}
}
