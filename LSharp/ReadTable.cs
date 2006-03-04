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
	/// The ReadTable is a lookup table which directs the reader on
	/// how to react to the presence of certain character combinations
	/// in the input stream
	/// </summary>
	public class ReadTable
	{
		private const int CAPACITY = 50;
		private Hashtable readTable = new Hashtable(CAPACITY);
		private Hashtable dispatchTable = new Hashtable(CAPACITY);

		public ReadTable()
		{
		}

		public void SetMacroCharacter(int c, ReaderMacro f) 
		{
			readTable.Add(c,f);
		}

		public ReaderMacro GetMacroCharacter(int c) 
		{
			return (ReaderMacro) readTable[c];
		}

		public void SetDispatchCharacter(int c, ReaderMacro f) 
		{
			dispatchTable.Add(c,f);
		}

		public ReaderMacro GetDispatchCharacter(int c) 
		{
			return (ReaderMacro) dispatchTable[c];
		}

		public static ReadTable DefaultReadTable() 
		{
			ReadTable readTable = new ReadTable();

			readTable.SetMacroCharacter('(',new ReaderMacro(Reader.LParReader));
			readTable.SetMacroCharacter('\"',new ReaderMacro(Reader.StringReader));
			readTable.SetMacroCharacter(';',new ReaderMacro(Reader.LineCommentReader));
			readTable.SetMacroCharacter('#',new ReaderMacro(Reader.DispatchReader));
			readTable.SetMacroCharacter('\'',new ReaderMacro(Reader.QuoteReader));

			readTable.SetMacroCharacter('`',new ReaderMacro(Reader.BackQuoteReader));
			readTable.SetMacroCharacter(',',new ReaderMacro(Reader.UnQuoteReader));

			readTable.SetDispatchCharacter('|',new ReaderMacro(Reader.MultiLineCommentReader));
			readTable.SetDispatchCharacter('\\',new ReaderMacro(Reader.CharacterReader));

			return readTable;
		}
	}
}
