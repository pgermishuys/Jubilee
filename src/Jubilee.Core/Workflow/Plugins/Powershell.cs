using Jubilee.Core.Notifications;
using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using process = System.Diagnostics;

namespace Jubilee.Core.Workflow.Plugins
{
    public class Powershell : Plugin
    {
        private INotificationService notificationService;
        public Powershell(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        public override bool Run()
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
            using (var runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
            {
                runspace.Open();

                var pipeline = runspace.CreatePipeline();
                var powershellCommand = new Command(new FileInfo(parameters.ScriptName).FullName);
                foreach (var parameter in ((IDictionary<string, object>)parameters))
                {
                    powershellCommand.Parameters.Add(new CommandParameter(parameter.Key, parameter.Value.ToString()));
                }
                pipeline.Commands.Add(powershellCommand);
                try
                {
                    var psObjects = pipeline.Invoke();
                    foreach (var obj in psObjects)
                    {
                        notificationService.Notify(parameters.ScriptName, obj == null ? String.Empty : obj.ToString(), NotificationType.Information);
                    }
                }
                catch (Exception ex)
                {
                    notificationService.Notify(parameters.ScriptName, ex.Message, NotificationType.Error);
                }
                runspace.Close();
            }
            return true;
        }

        private ErrorRecord GetPossibleErrorRecord(PSObject value)
        {
            if (value != null)
            {
                //get the ErrorRecord
                var errorRecord = value.BaseObject as ErrorRecord;
                return errorRecord;
            }
            return null;
        }
    }
}
