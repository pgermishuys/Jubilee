using Jubilee.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee
{
	public class CommandFactory
	{
		public ICommand Create(JubileeArgs args)
		{
			if(args.Help)
			{
				return new DisplayHelpCommand();
			}
			if (!String.IsNullOrEmpty(args.ScaffoldScriptCSPlugin))
			{
				return new ScaffoldScriptCSPluginCommand(args.ScaffoldScriptCSPlugin);
			}
            if (!String.IsNullOrEmpty(args.ScaffoldPowershellPlugin))
            {
                return new ScaffoldPowershellPluginCommand(args.ScaffoldPowershellPlugin);
            }
			return new RunJubileeCommand(args);
		}
	}
}
