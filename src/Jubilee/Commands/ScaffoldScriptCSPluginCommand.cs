using Jubilee.Core.Workflow.Plugins;
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
	public class ScaffoldScriptCSPluginCommand : ICommand
	{
		private string pluginName;
		public ScaffoldScriptCSPluginCommand(string pluginName)
		{
			this.pluginName = pluginName + ".csx";
		}

		public void Execute()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Scaffolding jubilee scriptcs plugin");

			var assemblyLocation = Assembly.GetExecutingAssembly().Location;
			var directoryAssemblyLocatedIn = Path.GetDirectoryName(assemblyLocation);
			var pathForNewPlugin = Path.Combine(directoryAssemblyLocatedIn, pluginName);
			var configurationFilePath = Path.Combine(directoryAssemblyLocatedIn, "configuration.yaml");

			ScriptCSTemplate template = new ScriptCSTemplate();
			var transformedText = template.TransformText();
			File.WriteAllText(pathForNewPlugin, transformedText);

			if (File.Exists(configurationFilePath))
			{
				var configurationFile = File.ReadAllText(configurationFilePath);
				var configurationBuilder = new StringBuilder();
				foreach (var configurationLine in configurationFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					configurationBuilder.AppendLine(configurationLine);
					if (configurationLine.StartsWith("Plugins:"))
					{
						configurationBuilder.AppendLine("- Name: " + typeof(ScriptCS).Name);
						configurationBuilder.AppendLine("  Parameters:");
						configurationBuilder.AppendLine("    ScriptName: " + pathForNewPlugin);
					}
				}
				File.WriteAllText(configurationFilePath, configurationBuilder.ToString());
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(pluginName + " has been created and added to the configuration.yaml");
			Console.ResetColor();
		}
	}
}
