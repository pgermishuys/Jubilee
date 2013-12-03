﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Process.Plugins
{
	public interface IPlugin
	{
		string Parameters { get; set; }
		bool Process(string workingPath);
	}
}
