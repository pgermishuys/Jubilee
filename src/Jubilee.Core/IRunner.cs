using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core
{
	public interface IRunner
	{
		void Run(string workingPath, string filePatternToWatch = "*.*");
	}
}
