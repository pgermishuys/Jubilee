using Jubilee.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jubilee.Core.Workflow.Tasks
{
	public class FileCopy : Task
	{
		public override bool Run()
		{
			try
			{
				CopyDirectory(parameters.From, parameters.To);
			}
			catch
			{
				return false;
			}
			return true;
		}

		private void CopyDirectory(string sourcePath, string destinationPath)
		{
			if (!Directory.Exists(destinationPath))
			{
				Directory.CreateDirectory(destinationPath);
			}

			foreach (string file in Directory.GetFiles(sourcePath))
			{
				string dest = Path.Combine(destinationPath, Path.GetFileName(file));
				File.Copy(file, dest, true);
			}

			foreach (string folder in Directory.GetDirectories(sourcePath))
			{
				string dest = Path.Combine(destinationPath, Path.GetFileName(folder));
				CopyDirectory(folder, dest);
			}
		}
    }
}
