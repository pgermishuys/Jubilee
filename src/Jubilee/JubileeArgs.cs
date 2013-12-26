using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee
{
	[ArgExample("jubilee -w c:/PathToWatch", "asks jubilee to watch the given path for changes and kick off the workflow defined in the configuration.yaml")]
	public class JubileeArgs
	{
		[ArgShortcut("?")]
		[ArgDescription("displays the jubilee help.")]
		public bool Help { get; set; }

		[ArgShortcut("s")]
		[ArgDescription("scaffolds a new scriptcs jubilee plugin.")]
		public string Scaffold { get; set; }
	}
}
