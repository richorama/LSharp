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
	public class PerformanceTests
	{

		private object Execute(string filename) 
		{
			string s = String.Format("(load \"..\\\\..\\\\testcases\\\\{0}\")",filename);
			return Runtime.EvalString(s);
		}

		[Test]
		public void Factorial() 
		{
		
			DateTime start = DateTime.Now;
			for (int i = 0 ; i < 1000; i++) 
			{
				Execute("factorial.ls");
			}

			long ticks = DateTime.Now.Ticks - start.Ticks;

			Console.WriteLine("Factorial completed in {0} ticks",ticks);

			Assert.IsTrue(ticks < 6000000);
		}

		[Test]
		public void MillionCons() 
		{
			const int SIZE = 1000000;
			Cons cons = new Cons(0);

			int i;

			for (i= 1 ; i < SIZE; i++) 
			{
				cons = new Cons(i,cons);
			}
			Assert.AreEqual(SIZE,cons.Length());
		}
	}
}
