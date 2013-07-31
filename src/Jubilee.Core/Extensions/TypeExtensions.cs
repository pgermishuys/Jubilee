using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Extensions
{
	public static class TypeExtensions
	{
		public static bool IsAssignableFrom(this Type type, params Type[] types)
		{
			foreach (var interfaceType in types)
			{
				if (interfaceType.IsAssignableFrom(type))
					return true;
			}
			return false;
		}
	}
}
