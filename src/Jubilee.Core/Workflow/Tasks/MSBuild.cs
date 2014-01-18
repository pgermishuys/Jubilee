using Jubilee.Core.Notifications;
using Jubilee.Core.Tasks;
using Ninject;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Jubilee.Core.Workflow.Tasks
{
	public class MSBuild : Task
	{
		private const string buildArguments = "/verbosity:quiet /nologo /clp:ErrorsOnly";
		private const string net4MSBuildPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";

		private INotificationService notificationService;
		public MSBuild(INotificationService notificationService)
		{
			this.notificationService = notificationService;
		}

		public override bool Run()
		{
			string solutionPath = FindSolutionFile(parameters.WorkingPath, "*.sln");
			if (String.IsNullOrEmpty(solutionPath))
			{
				throw new FileNotFoundException("Solution File could not be found");
			}
			return BuildSolution(solutionPath, buildArguments);
		}

		private string FindSolutionFile(string workingPath, string solutionExtension)
		{
			var solutionDirectory = DirectorySearch(workingPath, solutionExtension);
			return Directory.GetFiles(solutionDirectory, solutionExtension).FirstOrDefault();
		}

		private string DirectorySearch(string directoryToSearch, string solutionExtension)
		{
			if (new DirectoryInfo(directoryToSearch).Parent == null)
			{
				return null;
			}
			if (Directory.GetFiles(directoryToSearch, solutionExtension).Count() == 0)
			{
				var parentDirectory = Directory.GetParent(directoryToSearch);
				return DirectorySearch(parentDirectory.FullName, solutionExtension);
			}
			else
			{
				return directoryToSearch;
			}
		}

		private bool BuildSolution(string solutionFilePath, string buildArguments)
		{
			var result = true;
			string output = String.Empty;
			string errorOutput = String.Empty;
			ProcessStartInfo processStartInfo = new ProcessStartInfo(net4MSBuildPath, string.Join(" ", buildArguments, solutionFilePath));
			processStartInfo.UseShellExecute = false;
			processStartInfo.ErrorDialog = false;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.RedirectStandardOutput = true;
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.EnableRaisingEvents = true;
			process.StartInfo = processStartInfo;
			process.OutputDataReceived += (sender, args1) => output += args1.Data;
			process.ErrorDataReceived += (sender, args2) => errorOutput += args2.Data;

			bool processStarted = process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			process.WaitForExit();

			if (!String.IsNullOrEmpty(errorOutput) || !String.IsNullOrEmpty(output))
			{
				result = false;
				notificationService.Notify("Build Failed", errorOutput + Environment.NewLine + output, NotificationType.Error);
			}
			return result;
		}
    }
}
