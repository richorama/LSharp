#region Copyright (c) 2008, Rob Blackwell.  All rights reserved.
// Software License Agreement (BSD License)

// Copyright (c) 2008, Rob Blackwell.  All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:

//   * Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.

//   * Redistributions in binary form must reproduce the above
//     copyright notice, this list of conditions and the following
//     disclaimer in the documentation and/or other materials
//     provided with the distribution.

// THIS SOFTWARE IS PROVIDED BY THE AUTHOR 'AS IS' AND ANY EXPRESSED
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System.Collections;
using System.Collections.Generic;

namespace LSharp
{
    /// <summary>
	/// Symbols are used for their object identity to name various entities
	/// including variables and functions.
	/// The symbol table is also encapsulated staticly within this Symbol class.
	/// </summary>
	public class Symbol
	{
		private const int CAPACITY = 1000;
        private static Dictionary<object, object> symbolTable = new Dictionary<object, object>(CAPACITY);

		private string name;

		public string Name
		{
			get
			{
				return name;
			}
		}

        /// <summary>
        /// Constructs a new *uninterned* symbol
        /// </summary>
		public Symbol(string name)
		{
            this.name = name;
		}

		public override string ToString()
		{
			return name;
		}

		/// <summary>
		/// Returns a symbol given its name. If necessary the symbol is
		/// created and interned.
		/// </summary>
		public static Symbol FromName(string name)
		{
            Symbol symbol;

            if (symbolTable.ContainsKey(name)) 
            {
                symbol = (Symbol)symbolTable[name];
            }
			else
			{
                symbol = new Symbol(name);
				symbolTable.Add(name, symbol);
			}
			return symbol;
		}

	}
}
