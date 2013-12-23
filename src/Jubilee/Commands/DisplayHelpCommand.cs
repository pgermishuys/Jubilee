using PowerArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Commands
{
	public class DisplayHelpCommand : ICommand
	{
		public void Execute()
		{
			ArgUsage.GetStyledUsage<JubileeArgs>().Write();
		}
	}
}
