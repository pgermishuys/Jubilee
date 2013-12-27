using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Runners
{
	public interface IRunner
	{
		void Initialise(Dictionary<string, object> parameters);
		void AddParameter(string parameterName, object parameterValue);
		void AddParameter(KeyValuePair<string, object> parameter);
		void AddParameters(Dictionary<string, object> parameter);
		bool Run();
	}
}
