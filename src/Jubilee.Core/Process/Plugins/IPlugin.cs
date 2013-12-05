using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process.Plugins
{
	public interface IPlugin
	{
        void Initialise(Dictionary<string, object> parameters);
		bool Process(string workingPath);
	}
}
