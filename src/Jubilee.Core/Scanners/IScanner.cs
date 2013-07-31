using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Scanners
{
	public interface IScanner
	{
		Type[] GetTypes(string pathToScan, string pattern, params Type[] typesToScanFor);
	}
}
