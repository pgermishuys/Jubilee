using Jubilee.Core;
using Jubilee.Core.Configuration;
using Jubilee.Core.Notifications;
using Jubilee.Core.Notifications.Plugins;
using Jubilee.Core.Process.Plugins;
using Jubilee.Core.Serialization;
using Ninject;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee
{
	class Program
	{
		static void Main(string[] args)
		{
			var parsedArguments = Args.Parse<JubileeArgs>(args);
			var commandFactory = new CommandFactory();
			var command = commandFactory.Create(parsedArguments);
			if (command != null)
			{
				command.Execute();
			}
		}
	}
}
