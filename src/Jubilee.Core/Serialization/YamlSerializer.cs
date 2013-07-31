using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel.Serialization;

namespace Jubilee.Core.Serialization
{
	public class YamlSerializer : ISerializer
	{
		public T Deserialize<T>(string json)
		{
			var deserializer = new Deserializer();
			return deserializer.Deserialize<T>(new StringReader(json));
		}
	}
}
