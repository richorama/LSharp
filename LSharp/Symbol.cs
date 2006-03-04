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
using System.Collections;

namespace LSharp
{
	/// <summary>
	/// Symbols are used for their object identity to name various entities 
	/// including (but not limited to) linguistic entities such as variables 
	/// and functions. 
	/// In L Sharp all symbols are interned. The symbol table is also
	/// encapsulated within this Symbol class.
	/// </summary>
	public class Symbol 
	{
		private const int CAPACITY = 500;
		private static Hashtable symbolTable = new Hashtable(CAPACITY);

		private string name;

		/// <summary>
		/// Returns the Symbol's name
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		public Symbol(string name)
		{
			this.name = name.ToLower();
		}


		/// <summary>
		/// Returns a string represntation of the Symbol
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			return name;
		}

		/// <summary>
		/// Returns a symbol given its name. If necessary the symbol is
		/// created and interned.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Symbol FromName(string name) 
		{
            name = name.ToLower();
			Symbol symbol = (Symbol)symbolTable[name];
			if(symbol == null) 
			{
				symbol = new Symbol(name);
				symbolTable.Add(name, symbol);
			}
			return symbol;
		}

		// Define some commonly used symbols
		public static Symbol TRUE = Symbol.FromName("true");
		public static Symbol FALSE = Symbol.FromName("false");
		public static Symbol NULL = Symbol.FromName("null");
		public static Symbol IT = Symbol.FromName("it");
		public static Symbol SPLICE = Symbol.FromName("splice");
		public static Symbol UNQUOTE = Symbol.FromName("unquote");
		public static Symbol BACKQUOTE = Symbol.FromName("backquote");
	
	}
}
