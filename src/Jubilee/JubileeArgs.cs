using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee
{
    [ArgExample("Jubilee.exe", "runs Jubilee using workflow defined in the configuration.yaml")]
	public class JubileeArgs
	{
		[ArgShortcut("?")]
        [ArgDescription("displays the Jubilee help.")]
		public bool Help { get; set; }

		[ArgShortcut("s")]
        [ArgDescription("scaffolds a new scriptcs Jubilee plugin.")]
		public string Scaffold { get; set; }
	}
}
