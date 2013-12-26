using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using process = System.Diagnostics;
using Jubilee.Core.Notifications;
using Newtonsoft.Json;
using Jubilee.Core.Plugins;

namespace Jubilee.Core.Workflow.Plugins
{
	public class ScriptCSPlugin : Plugin
	{
		private INotificationService notificationService;
		public ScriptCSPlugin(INotificationService notificationService)
		{
			this.notificationService = notificationService;
		}
		public override bool Run()
		{
			process.Process process = new process.Process();
			process.StartInfo = new process.ProcessStartInfo("scriptcs", String.Format("-scriptname {0} -- {1}", parameters.ScriptName, String.Join(" ", ((IDictionary<string, object>)parameters).Select(x => x.Value))));
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
			return true;
		}
	}
}
