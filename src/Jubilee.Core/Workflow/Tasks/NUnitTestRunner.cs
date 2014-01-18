using Jubilee.Core.Notifications;
using Jubilee.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jubilee.Core.Workflow.Tasks
{
	public class NUnitTestRunner : Task
	{
		private string testRunnerExecutableName = @"nunit-console-x86.exe";
		private INotificationService notificationService;

		public NUnitTestRunner(INotificationService notificationService)
		{
			this.notificationService = notificationService;
		}

		public override bool Run()
		{
			return RunTests(parameters.WorkingPath);
		}

		public bool RunTests(string workingPath)
		{
			var result = true;
			var testOutput = new TestOutput();
			var testAssemblies = GetTestAssemblies(workingPath);
			testAssemblies = testAssemblies.GroupBy(x => Path.GetFileName(x)).Select(group => group.First());
			if (testAssemblies.Count() == 0)
				return result;

			var testRunnerPath = Directory.GetFiles(workingPath, testRunnerExecutableName, SearchOption.AllDirectories).FirstOrDefault();
			if (String.IsNullOrEmpty(testRunnerPath))
			{
				result = false;
				return result;
			}

			int numberOfTestsFailed = 0;
			foreach (string testAssembly in testAssemblies)
			{
				var processOutput = RunProcess(testRunnerPath, testAssembly);
				var specificationResults = ParseResultsFromTest(processOutput);
				if (numberOfTestsFailed > 0)
				{
					notificationService.Notify(String.Format("(NUnit) - {0} contains failing tests", Path.GetFileName(testAssembly)), message: String.Format("{0} tests failed", numberOfTestsFailed), notificationType: NotificationType.Error);
				}
				foreach (var specificationResult in specificationResults)
				{
					result = false;
					numberOfTestsFailed++;
					notificationService.Notify(String.Format("{0} - {1}", specificationResult.Context, specificationResult.Specification), message: specificationResult.Detail, notificationType: NotificationType.Error);
				}
				numberOfTestsFailed = 0;
			}
			return result;
		}

		private IEnumerable<string> GetTestAssemblies(string workingPath)
		{
			return Directory.GetFiles(workingPath, "*.dll", SearchOption.AllDirectories)
						.Where(x => Path.GetFileName(x).ToLower()
								.EndsWith("specs.dll") || Path.GetFileName(x).ToLower()
								.EndsWith("tests.dll"));
		}

		private ProcessOutput RunProcess(string processToRun, params string[] arguments)
		{
			string output = String.Empty;
			string errorOutput = String.Empty;
			ProcessStartInfo processStartInfo = new ProcessStartInfo(processToRun, string.Join(" ", arguments));
			processStartInfo.UseShellExecute = false;
			processStartInfo.ErrorDialog = false;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.RedirectStandardOutput = true;
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.EnableRaisingEvents = true;
			process.StartInfo = processStartInfo;
			process.OutputDataReceived += (sender, args1) => output += args1.Data + Environment.NewLine;
			process.ErrorDataReceived += (sender, args2) => errorOutput += args2.Data + Environment.NewLine;

			bool processStarted = process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			process.WaitForExit();
			return new ProcessOutput(output, errorOutput);
		}

		private IEnumerable<SpecificationResult> ParseResultsFromTest(ProcessOutput output)
		{
			var specificationResults = new List<SpecificationResult>();
			bool startParsingResults = false;

			string context = String.Empty;
			string specification = String.Empty;
			string detail = String.Empty;

			foreach (string line in output.Output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
			{
				if (Regex.Match(line, "Failures:").Success)
				{
					startParsingResults = true;
					continue;
				}

				if (startParsingResults == false)
					continue;

				if (String.IsNullOrEmpty(line))
				{
					if (!String.IsNullOrEmpty(context))
					{
						specificationResults.Add(new SpecificationResult(context, specification, detail));
					}
					context = String.Empty;
					continue;
				}

				if (Regex.Match(line, "Test Failure : ").Success)
				{
					context = line.Replace("Test Failure : ", "");
					continue;
				}

				if (!String.IsNullOrEmpty(context))
				{
					detail += line + Environment.NewLine;
				}
			}
			return specificationResults;
		}

		public class SpecificationResult
		{
			public string Context { get; set; }
			public string Specification { get; set; }
			public string Detail { get; set; }

			public SpecificationResult(string context, string specification, string detail)
			{
				this.Context = context;
				this.Specification = specification;
				this.Detail = detail;
			}
		}

		private struct ProcessOutput
		{
			public string Output;
			public string ErrorOutput;
			public ProcessOutput(string output, string errorOutput)
			{
				this.Output = output;
				this.ErrorOutput = errorOutput;
			}
		}

		private class TestOutput
		{
			public List<SpecificationResult> SpecificationResults { get; set; }
			public string Errors { get; set; }
			public bool HasErrors
			{
				get
				{
					return !String.IsNullOrEmpty(Errors) || SpecificationResults.Count > 0;
				}
			}
			public TestOutput()
			{
				SpecificationResults = new List<SpecificationResult>();
				Errors = String.Empty;
			}
			public TestOutput(string errors)
				: this()
			{
				this.Errors = errors;
			}
			public TestOutput(IEnumerable<SpecificationResult> specificationResults)
			{
				this.SpecificationResults = new List<SpecificationResult>(specificationResults);
			}
			public void AddSpecificationResults(IEnumerable<SpecificationResult> specificationResults)
			{
				SpecificationResults.AddRange(specificationResults);
			}
		}
	}
}
