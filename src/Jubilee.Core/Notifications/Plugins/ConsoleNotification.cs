using System;

namespace Jubilee.Core.Notifications.Plugins
{
	public class ConsoleNotification : INotificationPlugin
	{
		public void Notify(string title, string message = "", NotificationType notificationType = NotificationType.None)
		{
			var originalColor = System.Console.ForegroundColor;

			switch (notificationType)
			{
				case NotificationType.Information:
					System.Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case NotificationType.Success:
					System.Console.ForegroundColor = ConsoleColor.Green;
					break;
				case NotificationType.Error:
					System.Console.ForegroundColor = ConsoleColor.Red;
					break;
				case NotificationType.Warning:
					System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
					break;
				default:
					System.Console.ForegroundColor = ConsoleColor.White;
					break;
			}

			Console.WriteLine(title);

			System.Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
			Console.WriteLine(message);

			System.Console.ForegroundColor = originalColor;
		}
	}
}
