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
using System.IO;
using System.Text;
using System.Globalization;

namespace LSharp
{
	/// <summary>
	/// Reads arbitrary LSharp objects from textual descriptions
	/// </summary>
	public class Reader
	{
		
		public static Object DispatchReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			int c = textReader.Read();


			Object f = readTable.GetDispatchCharacter(c);

			if (f != null) 
			{
				return ((ReaderMacro) f)(textReader, readTable);
			} 
			else
				return AtomReader(c, textReader);
		}

		public static Object MultiLineCommentReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			int c;

			do
			{
				do 
				{
					c = textReader.Read();
				} while (c != -1 && c != '|' );

				c = textReader.Read();
			} while (c != -1 & c != '#');

			return Read(textReader, readTable, null);

		}


		public static Object CharacterReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			int c;

			c = textReader.Read();
			
			return (char) c;

		}


		public static Object LineCommentReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			int c;

			do 
			{
				c = textReader.Read();
			} while (c != -1 && c != '\n' && c != '\r');

			return Read(textReader, readTable, null);
		}

		public static Object QuoteReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			return new Cons(Symbol.FromName("quote"), new Cons(Read(textReader, readTable, null),null));
		}

		public static Object BackQuoteReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			return new Cons(Symbol.FromName("backquote"), new Cons(Read(textReader, readTable, null),null));
		}

		public static Object UnQuoteReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];

			if (textReader.Peek() == '@') 
			{
				textReader.Read();
				return new Cons(Symbol.FromName("splice"), new Cons(Read(textReader, readTable, null),null));
			}
			return new Cons(Symbol.FromName("unquote"), new Cons(Read(textReader, readTable, null),null));
		}

		public static Object LParReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			ReadTable readTable = (ReadTable) args[1];
			int c;

						
			while (Char.IsWhiteSpace((Char)textReader.Peek()))
			{
				c = textReader.Read();
			}

			object o = Read(textReader, readTable, null);
			if (o != Symbol.FromName(")"))
				return new Cons(o, LParReader(textReader, readTable));

			return null;

		}

		private static bool TerminatingCharP(char c) 
		{

			bool b = (Char.IsWhiteSpace(c) || c == '\"' || c == ')' || c=='(' );
			return b;
		}

		public static Object AtomReader(int c, TextReader textReader)
		{
			if (((Char) c) == ')') 
			{
				return Symbol.FromName(")");
			}

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append((char)c);
			bool done = false;
			
			do 
			{
				int nextChar = textReader.Peek();

				
				if ((nextChar == -1) || (TerminatingCharP((Char)nextChar)))
					done = true;
				else 
				{
					c = textReader.Read();
					stringBuilder.Append((char) c);
				}

			} while (!done);

			string token = stringBuilder.ToString();
			
			
			Double d;

			// Try reading the number as an integer
			if (Double.TryParse(token,System.Globalization.NumberStyles.Integer,NumberFormatInfo.InvariantInfo,out d)) 
			{
				return (int)d;
			} 

			// Try reading the number as a double
			if (Double.TryParse(token,System.Globalization.NumberStyles.Any,NumberFormatInfo.InvariantInfo,out d)) 
			{
				return d;
			} 
			else 
			{
				return Symbol.FromName(token);
			}
		}




		public static Object StringReader(params Object[] args)
		{
			TextReader textReader = (TextReader) args[0];
			StringBuilder stringBuilder = new StringBuilder();
			bool done = false;
			int c;

			do 
			{
				c = textReader.Read();
		
				if (c == -1) throw new Exception("EOF Reached");

				if (c == '\"') done = true;

				if(c == '\\')	//escape
				{
					c = textReader.Read();
					if(c == -1)
					{
						throw new Exception("Read error - eof found before matching: \"");
					}
					switch(c)
					{
						case 't':
							c = '\t';
							break;
						case 'r':
							c = '\r';
							break;
						case 'n':
							c = '\n';
							break;
						case '\\':
							break;
						case '"':
							break;
						default:
							throw new Exception("Unsupported escape character: \\" + (Char)c);
					}
				}

				if (!done)
					stringBuilder.Append((char) c);

			} while (!done);


			return stringBuilder.ToString();
		}

		public static Object Read(TextReader textReader, ReadTable readTable) 
		{
			return Read (textReader, readTable, null);
		}

		public static Object Read(TextReader textReader, ReadTable readTable, object eofValue) 
		{
			int c = textReader.Read();


			while (Char.IsWhiteSpace((Char)c)) 
			{
				c = textReader.Read();
			}

			if (c == -1) 
			{
				// End of file
				return eofValue;
			}

			Object f = readTable.GetMacroCharacter(c);

			if (f != null) 
			{
				return ((ReaderMacro) f)(textReader, readTable);
			} 
			else 
			{
				return AtomReader(c, textReader);
			}
		}
	}
}
