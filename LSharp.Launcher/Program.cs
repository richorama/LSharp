#region Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
//
// L Sharp .NET, a powerful lisp-based scripting language for .NET.
// Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
#endregion

using System;
using System.Windows.Forms;

namespace LSharp.Launcher
{
	/// <summary>
	/// A Launcher program which executes L Sharp scripts without
	/// creaing a Console window
	/// </summary>
	public class Program
	{
		
		//[STAThread]
		static void Main(string[] args)
		{
			
			if (args.Length > 0) 
			{			
				string filename = args[0];

				// Windows uses backslash as directory separator, so
				// we must escape it for use with L Sharp
				filename = filename.Replace("\\","\\\\");

				// Create a new global environment
				Environment environment = new Environment();
				
				// Load the script file in that environment
				Runtime.EvalString(string.Format("(load \"{0}\")",filename), environment);
			}
		}
	}
}
