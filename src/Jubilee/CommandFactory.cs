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
			if (!String.IsNullOrEmpty(args.ScaffoldScriptCSTask))
			{
				return new ScaffoldScriptCSTaskCommand(args.ScaffoldScriptCSTask);
			}
            if (!String.IsNullOrEmpty(args.ScaffoldPowershellTask))
            {
                return new ScaffoldPowershellTaskCommand(args.ScaffoldPowershellTask);
            }
			return new RunJubileeCommand(args);
		}
	}
}
