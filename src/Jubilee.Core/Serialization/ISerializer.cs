using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Serialization
{
	public interface ISerializer
	{
		T Deserialize<T>(string json);
	}
}
