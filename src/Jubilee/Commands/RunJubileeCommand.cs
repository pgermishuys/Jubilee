using Jubilee.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Commands
{
	public class RunJubileeCommand : ICommand
	{
		private JubileeArgs arguments;
		public RunJubileeCommand(JubileeArgs arguments)
		{
			this.arguments = arguments;
		}
		public void Execute()
		{
			ConfigurationBuilder configuration = null;
			try
			{
				configuration = new ConfigurationBuilder("configuration.yaml");
			}
			catch (SerializationException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(String.Format("{0} \n{1}", "There was a problem with the deserializing of your configuration file. Here are the details.", ex.Message));
				Console.ResetColor();
				return;
			}
			var runner = configuration.Build();
			runner.Run();
			Console.ReadLine();
		}
	}
}
