using System;

namespace Jubilee.Core.Notifications
{
	public class ConsoleNotification : INotificationPlugin
	{
		public void Notify(string title, string message = "", NotificationType notificationType = NotificationType.None)
		{
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
			Console.WriteLine(message);
			Console.WriteLine();

			Console.ResetColor();
		}
	}
}
