using Jubilee.Core.Notifications;
using Ninject;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Jubilee.Core.Process.Plugins
{
	public class MSBuild2013 : Plugin, IPlugin
	{
		private const string buildArguments = "/verbosity:quiet /nologo /clp:ErrorsOnly";
		private const string net4MSBuildPath = @"C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe";

		private INotificationService notificationService;
		public MSBuild2013(INotificationService notificationService)
		{
			this.notificationService = notificationService;
		}

		public bool Process(string workingPath)
		{
            string solutionPath = FindSolutionFile(workingPath, "*.sln");
			if (String.IsNullOrEmpty(solutionPath))
			{
				throw new FileNotFoundException("Solution File could not be found");
			}
			return BuildSolution(solutionPath, buildArguments);
		}

		private string FindSolutionFile(string workingPath, string solutionExtension)
		{
			return Directory.GetFiles(workingPath, solutionExtension).FirstOrDefault();
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
