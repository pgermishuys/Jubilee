using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jubilee.Core.Extensions;

namespace Jubilee.Core.Process.Plugins
{
	public abstract class Plugin : IPlugin
	{
        protected dynamic parameters;
        public void Initialise(Dictionary<string, object> parameters)
        {
            this.parameters = parameters.ToExpando();
        }
        public abstract bool Process(string workingDirectory);
	}
}
