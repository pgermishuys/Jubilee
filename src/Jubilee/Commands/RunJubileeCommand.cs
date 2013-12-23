using Jubilee.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Commands
{
	public class RunJubileeCommand : ICommand
	{
		private string folderToWatch;
		public RunJubileeCommand(string folderToWatch)
		{
			this.folderToWatch = folderToWatch;
		}
		public void Execute()
		{
			string workingPath = folderToWatch;
			var configuration = new ConfigurationBuilder("configuration.yaml");
			var runner = configuration.Build();
			runner.Run(workingPath);
			Console.ReadLine();
		}
	}
}
