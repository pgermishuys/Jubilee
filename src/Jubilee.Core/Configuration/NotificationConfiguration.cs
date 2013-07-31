using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Configuration
{
	public class NotificationConfiguration
	{
		public string Name { get; protected set; }
		public NotificationConfiguration(string name)
		{
			this.Name = name;
		}
	}
}
