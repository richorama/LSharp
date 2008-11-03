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
using LSharp;
using NUnit.Framework;
using System.IO;

namespace LSharp.Tests
{

	[TestFixture]
	public class ReaderTests
	{
		
		
		[Test]
		public void Christian() 
		{
			string expression;
			object o;

			expression =  "(car '((a b)(c d)(e f)))";
			o = Runtime.EvalString(expression);
			Assert.AreEqual("(a b)",Printer.WriteToString((Cons)o));
	
			expression = "(car (quote ((a b)(c d)(e f))))";
			o = Runtime.EvalString(expression);
			Assert.AreEqual("(a b)",Printer.WriteToString((Cons)o));
	
			expression =  "(first (quote ((a b)(c d)(e f))))";
			o = Runtime.EvalString(expression);
			Assert.AreEqual("(a b)",Printer.WriteToString((Cons)o));
	
			expression =  "(cdr (quote ((a b)(c d)(e f))))";
			o = Runtime.EvalString(expression);
			Assert.AreEqual("((c d) (e f))",Printer.WriteToString((Cons)o));
	
		}								

		[Test]
		public void Symbol() 
		{
			string expression = "a";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object symbol = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual(expression,symbol.ToString());
		}

		[Test]
		public void EmptyList() 
		{
			string expression = "()";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.IsNull(result);
		}

		[Test]
		public void OneItemList() 
		{
			string expression = "(a)";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual("LSharp.Cons",result.GetType().ToString());

			Cons c = (Cons)result;

			Assert.AreEqual(1,c.Length());
			Assert.AreEqual("a",c.Car().ToString());
		}

		[Test]
		public void TwoItemList() 
		{
			string expression = "(a b)";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual("LSharp.Cons",result.GetType().ToString());

			Cons c = (Cons)result;

			Assert.AreEqual(2,c.Length());
			Assert.AreEqual("b",c.Cadr().ToString());
		}

		[Test]
		public void ListOfNull() 
		{
			string expression = "(())";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual("LSharp.Cons",result.GetType().ToString());

			Cons c = (Cons)result;

			Assert.AreEqual(1,c.Length());
			Assert.IsNull(c.Car());
		}

		[Test]
		public void ListOfListOfNull() 
		{
			string expression = "((()))";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual("LSharp.Cons",result.GetType().ToString());

			Cons c = (Cons)result;

			Assert.AreEqual(1,c.Length());
			Assert.IsNull(c.Caar());
		}

		[Test]
		public void RightParen() 
		{
			string expression = ")(c";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual(")",((Symbol)result).ToString());
			
		}

		[Test]
		public void ListOfTwoLists() 
		{
			string expression = "((a b)(c d))";

			ReadTable readTable = ReadTable.DefaultReadTable();
			object result = Reader.Read(new StringReader(expression), readTable);
			
			Assert.AreEqual("LSharp.Cons",result.GetType().ToString());

			Cons c = (Cons)result;

			Assert.AreEqual(2,c.Length());
			Assert.AreEqual("a",c.Caar().ToString());
		}
		
	}
}
