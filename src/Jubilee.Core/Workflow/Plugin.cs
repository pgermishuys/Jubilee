using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jubilee.Core.Extensions;
using System.Dynamic;
using Jubilee.Core.Runners;

namespace Jubilee.Core.Plugins
{
	public abstract class Plugin : IPlugin
	{
		protected dynamic parameters;
		public Plugin()
		{
			parameters = new ExpandoObject();
		}
		public void Initialise(Dictionary<string, object> parameters)
		{
			this.parameters = parameters.ToExpando();
		}
		public void AddParameters(Dictionary<string, object> parameters)
		{
			foreach (var parameter in parameters)
			{
				AddParameter(parameter);
			}
		}
		public void AddParameter(string parameterName, object parameterValue)
		{
			AddParameter(new KeyValuePair<string, object>(parameterName, parameterValue));
		}
		public void AddParameter(KeyValuePair<string, object> parameter)
		{
			if (!((IDictionary<string, object>)this.parameters).ContainsKey(parameter.Key))
			{
				((IDictionary<string, object>)this.parameters).Add(parameter.Key, parameter.Value);
			}
			else
			{
				((IDictionary<string, object>)this.parameters)[parameter.Key] = parameter.Value;
			}
		}
		public abstract bool Run();
    }
}
