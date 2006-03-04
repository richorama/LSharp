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
	/// Formats and prints LSharp objects
	/// </summary>
	public class Printer
	{
		public Printer()
		{

		}

		public static string WriteToString(Object x) 
		{

			if (x == null) 
			{
				return "null";
			}

			Type type = x.GetType();

			if (type == typeof (string)) 
			{
				return string.Format("\"{0}\"",(string) x);
			}

			if (type == typeof (char)) 
			{
				return string.Format("#\\{0}", x);
			}

			if (type == typeof (Symbol)) 
			{
				return string.Format("{0}",((Symbol) x).ToString());
			}

			if (type == typeof (Cons)) 
			{
				Cons cons = (Cons) x;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				stringBuilder.Append(WriteToString(cons.Car()));
				stringBuilder.Append(" ");
				Object o;
				o = cons.Cdr();
				while (o != null) 
				{
					if (o.GetType() == typeof (Cons)) 
					{
						cons = (Cons)o;
						stringBuilder.Append(WriteToString(cons.Car()));
						
						o = cons.Cdr();

						if (o !=null)
							stringBuilder.Append(" ");
					} 
					else 
					{
						stringBuilder.Append(". ");
						stringBuilder.Append(WriteToString(o));
						o = null;
					}
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}

			return x.ToString().Trim();
		}

		public static void Write(Object x) 
		{
			Console.WriteLine(WriteToString(x));
		}
			
		
	}
}
