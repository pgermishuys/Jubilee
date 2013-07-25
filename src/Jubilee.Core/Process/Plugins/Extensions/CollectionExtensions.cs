using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process.Plugins.Extensions
{
	public static class CollectionExtensions
	{
		public static IQueryable<IPlugin> NonDependentPlugins(this IEnumerable<IPlugin> plugins)
		{
			return plugins.Where(x => x.GetType().GetInterface(typeof(IDependsOnPlugin<>).Name) == null).AsQueryable();
		}

		public static IQueryable<IPlugin> DependentPlugins(this IPlugin plugin, IEnumerable<IPlugin> plugins)
		{
			return plugins.Where(x => x.GetType().GetInterfaces()
								 .Any(contract => contract.GetGenericArguments()
													.Any(argument => argument.Name == plugin.GetType().Name))
								 ).AsQueryable();
		}
	}
}
