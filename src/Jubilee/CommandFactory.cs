﻿using Jubilee.Commands;
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
			if (!String.IsNullOrEmpty(args.Scaffold))
			{
				return new ScaffoldScriptCSPluginCommand(args.Scaffold);
			}
			if(!String.IsNullOrEmpty(args.FolderToMonitor))
			{
				return new RunJubileeCommand(args.FolderToMonitor);
			}
			return new DisplayHelpCommand();
		}
	}
}
