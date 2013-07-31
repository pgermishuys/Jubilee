using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jubilee.Core.Extensions;

namespace Jubilee.Core.Scanners
{
	public class AssemblyScanner : IScanner
	{
		public Type[] GetTypes(string pathToScan, string pattern, params Type[] typesToScanFor)
		{
			var types = new List<Type>();
			foreach (var assemblyFileName in Directory.GetFiles(pathToScan, pattern))
			{
				var loadedAssembly = Assembly.LoadFile(Path.Combine(pathToScan, assemblyFileName));
				var plugins = loadedAssembly.GetTypes().Where(x => (typesToScanFor.Length == 0 ? x != null : x.IsAssignableFrom(typesToScanFor)) && x.IsClass);
				types.AddRange(plugins);
			}
			return types.ToArray();
		}
	}
}
