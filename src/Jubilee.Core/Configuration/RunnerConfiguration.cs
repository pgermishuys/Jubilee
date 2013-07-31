using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class RunnerConfiguration
	{
		public string Name { get; protected set; }
		public RunnerConfiguration(string name)
		{
			this.Name = name;
		}
	}
}
