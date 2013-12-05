using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process.Plugins
{
	public class FileCopy : Plugin, IPlugin
	{
		public bool Process(string workingPath)
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
				File.Copy(file, dest);
			}

			foreach (string folder in Directory.GetDirectories(sourcePath))
			{
				string dest = Path.Combine(destinationPath, Path.GetFileName(folder));
				CopyDirectory(folder, dest);
			}
		}
    }
}
