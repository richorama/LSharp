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
using System.Text;

namespace LSharp
{
	/// <summary>
	/// A closure is a function defined with a captured environment
	/// </summary>
	public class Closure
	{
		// The captured environment
		private Environment environment;

		// The body is a list of LSharp expressions
		private Cons body; 

		// The list of named arguments
		private Cons argumentNames;

		/// <summary>
		/// Defines a closure (function) in terms of its arguments, body and
		/// the environment in which it is defined
		/// </summary>
		/// <param name="argumentNames">A list of argument names</param>
		/// <param name="body"> An LSharp program which is the body of the function</param>
		/// <param name="environment"></param>
		public Closure(Cons argumentNames, Cons body, Environment environment)
		{
			this.environment = environment;
			this.body = body;
			this.argumentNames = argumentNames;
		}

		/// <summary>
		/// Invokes the closure with no arguments
		/// </summary>
		/// <returns></returns>
		public Object Invoke() 
		{
			return Invoke (null);
		}

		/// <summary>
		/// Invokes the closure with arguments bound to the values specified in the 
		/// argument list
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public Object Invoke(Cons arguments) 
		{
			// Create a new lexical environment
			Environment localEnvironment = new Environment(environment);

			Primitives.ProcessArguments(argumentNames, arguments, localEnvironment);

			// Evaluate the body within this lexical environment
			object result = null;
            if (body != null)
            {
                foreach (object o in body)
                {
                    result = Runtime.Eval(o, localEnvironment);
                }
            }
			return result;

		}

		/// <summary>
		/// Returns a string that describes this closure
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("LSharp.Closure (fn {0} {1})", 
				Printer.WriteToString(argumentNames), 
				Printer.WriteToString(body));
		}
	}
}
