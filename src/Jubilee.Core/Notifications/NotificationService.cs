using Jubilee.Core.Notifications.Plugins;
using System.Threading.Tasks;

namespace Jubilee.Core.Notifications
{
	public class NotificationService : INotificationService
	{
		private INotificationPlugin[] notificationPlugins;
		
		public NotificationService(INotificationPlugin[] notificationPlugins)
		{
			this.notificationPlugins = notificationPlugins;
		}

		public void Notify(string title, string message = "", NotificationType notificationType = NotificationType.None)
		{
			Parallel.ForEach(notificationPlugins, (notificationPlugin) =>
			{
				notificationPlugin.Notify(title, message, notificationType);
			});
		}
	}
}
