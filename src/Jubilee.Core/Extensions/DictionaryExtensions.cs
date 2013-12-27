
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Extensions
{
    public static class DictionaryExtensions
    {
		public static DynamicDictionary ToDynamic(this IDictionary<string, object> dictionary)
		{
			return new DynamicDictionary((Dictionary<string, object>)dictionary);
		}
    }

	public class DynamicDictionary : DynamicObject
	{
		private readonly Dictionary<string, object> dictionary;

		public DynamicDictionary(Dictionary<string, object> dictionary)
		{
			this.dictionary = dictionary;
		}

		public override bool TryGetMember(
			GetMemberBinder binder, out object result)
		{
			if (dictionary != null && dictionary.ContainsKey(binder.Name))
			{
				dictionary.TryGetValue(binder.Name, out result);
			}
			else
			{
				result = default(object);
			}
			return true;
		}

		public override bool TrySetMember(
			SetMemberBinder binder, object value)
		{
			dictionary[binder.Name] = value;

			return true;
		}
	}
}
