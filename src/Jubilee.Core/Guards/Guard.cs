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
		public static void AgainstException<TException>(Action action, string message) where TException : Exception
		{
			try
			{
				action();
			}
			catch (TException)
			{
				throw new GuardException(message);
			}
		}

		public static void Catch<TException>(Action action) where TException : Exception{
			try
			{
				action();
			}
			catch (TException)
			{
				//shallow
			}
		}
	}
}
