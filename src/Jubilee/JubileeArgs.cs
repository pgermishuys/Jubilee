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
        [ArgDescription("scaffolds a new ScriptCS Jubilee task.")]
		public string ScaffoldScriptCSTask { get; set; }

        [ArgShortcut("p")]
        [ArgDescription("scaffolds a new Powershell Jubilee task.")]
        public string ScaffoldPowershellTask { get; set; }
	}
}
