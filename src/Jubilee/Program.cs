using Jubilee.Commands;
using Jubilee.Core;
using Jubilee.Core.Configuration;
using Jubilee.Core.Notifications;
using Jubilee.Core.Serialization;
using Ninject;
using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee
{
	class Program
	{
		static void Main(string[] args)
		{
			JubileeArgs parsedArguments = null;
			try
			{
				parsedArguments = Args.Parse<JubileeArgs>(args);
			}
			catch (PowerArgs.UnexpectedArgException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
				return;
			}
			var commandFactory = new CommandFactory();
			ICommand command = commandFactory.Create(parsedArguments);
			if (command != null)
			{
				command.Execute();
			}
		}
	}
}
