using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Notifications
{
	public interface INotificationPlugin
	{
		void Notify(string title, string message = "", NotificationType notificationType = NotificationType.None);
	}
}
