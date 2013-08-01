using Jubilee.Core;
using Jubilee.Core.Configuration;
using Jubilee.Core.Notifications.Plugins;
using Jubilee.Core.Process.Plugins;
using Ninject;
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
			string workingPath = args.FirstOrDefault() ?? AppDomain.CurrentDomain.BaseDirectory;
			var configuration = new ConfigurationBuilder("configuration.yaml");
			var runner = configuration.Build();
			runner.Run(workingPath);
			System.Console.ReadLine();
		}
	}
}
