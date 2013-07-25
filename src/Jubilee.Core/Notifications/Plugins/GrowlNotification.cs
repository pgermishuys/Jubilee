using Growl.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Notifications.Plugins
{
	public class GrowlNotification : INotificationPlugin
	{
		public void Notify(string title, string message = "", NotificationType notificationType = NotificationType.None)
		{
			var simpleGrowl = new GrowlConnector();
			var thisApplication = new Growl.Connector.Application("Jubilee");
			var simpleGrowlType = new Growl.Connector.NotificationType("SIMPLEGROWL");
			simpleGrowl.Register(thisApplication, new Growl.Connector.NotificationType[] { simpleGrowlType });
			var myGrowl = new Notification("Jubilee", "SIMPLEGROWL", title, title, message);
			simpleGrowl.Notify(myGrowl);
		}
	}
}
