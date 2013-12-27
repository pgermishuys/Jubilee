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

			ScriptCSTemplate template = new ScriptCSTemplate();
			var transformedText = template.TransformText();
			File.WriteAllText(pathForNewPlugin, transformedText);

			if (File.Exists("configuration.yaml"))
			{
				var configurationFile = File.ReadAllText("configuration.yaml");
				var configurationBuilder = new StringBuilder();
				foreach (var configurationLine in configurationFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					configurationBuilder.AppendLine(configurationLine);
					if (configurationLine.StartsWith("Plugins:"))
					{
						configurationBuilder.AppendLine("- Name: " + pathForNewPlugin);
					}
				}
				File.WriteAllText("configuration.yaml", configurationBuilder.ToString());
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(pluginName + " has been created and added to the configuration.yaml");
			Console.ResetColor();
		}
	}
}
