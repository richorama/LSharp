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
using NUnit.Framework;
using System.IO;

namespace LSharp.Tests
{

	[TestFixture]
	public class PrinterTests
	{

		[Test]
		public void TestList() 
		{
			string expression = "(a b c)";
			string result = Printer.WriteToString(Reader.Read(new StringReader(expression),ReadTable.DefaultReadTable()));

			Assert.AreEqual(expression, result);

		}


		[Test]
		public void TestString() 
		{
			string expression = "\"string\"";
			string result = Printer.WriteToString(Reader.Read(new StringReader(expression),ReadTable.DefaultReadTable()));

			Assert.AreEqual(expression, result);

		}

		[Test]
		public void TestNumber() 
		{
			string expression = "123.45";
			string result = Printer.WriteToString(Reader.Read(new StringReader(expression),ReadTable.DefaultReadTable()));

			Assert.AreEqual(expression, result);

		}

		[Test]
		public void TestQuote() 
		{
			string expression = "'a";
			string result = Printer.WriteToString(Reader.Read(new StringReader(expression),ReadTable.DefaultReadTable()));

			Assert.AreEqual("(quote a)", result);
		}
	}
}
