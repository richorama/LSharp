#region Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
//
// L Sharp .NET, a powerful lisp-based scripting language for .NET.
// Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
// 
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the Free
// Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
#endregion

using System;

namespace LSharp
{
	/// <summary>
	/// A Macro is transforms its arguments into a new expression
	/// which can be passed to the reader. In this way it is
	/// possible to write programs that write programs.
	/// </summary>
	public class Macro
	{

		// The body is a list of LSharp expressions
		private Cons body; 

		// The list of named arguments
		private Cons argumentNames;

		private Environment environment;

		public Macro(Cons argumentNames, Cons body, Environment environment)
		{			
			this.body = body;
			this.argumentNames = argumentNames;
			this.environment = environment;
		}

		public Object Expand(Cons arguments) 
		{
			// Create a new lexical environment
			Environment localEnvironment = new Environment(environment);

			Primitives.ProcessArguments(argumentNames, arguments, localEnvironment);

			object result = null;
			foreach (object o in body) 
			{
				result = Runtime.Eval(o,localEnvironment);
			}
			return result;

		}

		/// <summary>
		/// Returns a string that describes this macro
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("(macro {0} {1})", 
				Printer.WriteToString(argumentNames), 
				Printer.WriteToString(body));
		}

	}
}
