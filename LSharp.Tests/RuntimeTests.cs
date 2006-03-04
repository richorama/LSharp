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

namespace LSharp.Tests
{
	[TestFixture]
	public class RuntimeTests
	{
		[Test]
		public void Consp() 
		{
			Cons cons = new Cons(100, 100);
			Assert.IsTrue(Primitives.IsList(cons));
		}

		[Test]
		public void ConsLength() 
		{
			const int SIZE = 10;
			Cons cons = new Cons(0);

			int i;

			for (i= 1 ; i < SIZE; i++) 
			{
				cons = new Cons(i,cons);
				Console.WriteLine(i);
			}

			Assert.AreEqual(SIZE, cons.Length());
		}
	}
}
