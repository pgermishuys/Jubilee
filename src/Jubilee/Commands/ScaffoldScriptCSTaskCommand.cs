using Jubilee.Core.Workflow.Tasks;
using Jubilee.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Commands
{
	public class ScaffoldScriptCSTaskCommand : ICommand
	{
		private string taskName;
		public ScaffoldScriptCSTaskCommand(string taskName)
		{
			this.taskName = taskName + ".csx";
		}

		public void Execute()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Scaffolding Jubilee scriptcs task");

			var assemblyLocation = Assembly.GetExecutingAssembly().Location;
			var directoryAssemblyLocatedIn = Path.GetDirectoryName(assemblyLocation);
			var pathForNewTask = Path.Combine(directoryAssemblyLocatedIn, taskName);
			var configurationFilePath = Path.Combine(directoryAssemblyLocatedIn, "configuration.yaml");

			ScriptCSTemplate template = new ScriptCSTemplate();
			var transformedText = template.TransformText();
			File.WriteAllText(pathForNewTask, transformedText);

			if (File.Exists(configurationFilePath))
			{
				var configurationFile = File.ReadAllText(configurationFilePath);
				var configurationBuilder = new StringBuilder();
				foreach (var configurationLine in configurationFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					configurationBuilder.AppendLine(configurationLine);
					if (configurationLine.StartsWith("Tasks:"))
					{
                        configurationBuilder.AppendLine("- Name: " + Guid.NewGuid());
                        configurationBuilder.AppendLine("  Task: " + typeof(Powershell).Name);
						configurationBuilder.AppendLine("  Parameters:");
						configurationBuilder.AppendLine("    ScriptName: " + pathForNewTask);
					}
				}
				File.WriteAllText(configurationFilePath, configurationBuilder.ToString());
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(taskName + " has been created and added to the configuration.yaml");
			Console.ResetColor();
		}
	}
}
