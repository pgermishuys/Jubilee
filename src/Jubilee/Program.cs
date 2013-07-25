using Jubilee.Core;
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
			string workingPath = args.FirstOrDefault() ?? @"F:\projects\crossfit";
			var configuration = new ConfigurationBuilder()
										.WithNotificationPlugin<GrowlNotification>()
										.WithNotificationPlugin<ConsoleNotification>()
										.WithPlugin<MSBuild>()
										.WithPlugin<MSpecTestRunner>()
										.WithPlugin<NUnitTestRunner>()
										.WithRunner<FileSystemWatchingRunner>();

			var runner = configuration.Build();
			runner.Run(workingPath);
			System.Console.ReadLine();
		}
	}
}
