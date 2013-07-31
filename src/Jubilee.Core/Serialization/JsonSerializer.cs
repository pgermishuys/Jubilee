using Jubilee.Core.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Serialization
{
	public class JsonSerializer : ISerializer
	{
		public T Deserialize<T>(string json)
		{
			var contractResolver = new DefaultContractResolver();
			contractResolver.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
			var settings = new JsonSerializerSettings { ContractResolver = contractResolver };

			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}
