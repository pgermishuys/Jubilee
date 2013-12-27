using Jubilee.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Guards
{
	public static class Guard
	{
		public static bool AgainstException<TException>(Action action, string message, INotificationService notificationService) where TException : Exception
		{
			try
			{
				action();
				return true;
			}
			catch (TException)
			{
				throw new GuardException(message);
			}
		}
	}
}
