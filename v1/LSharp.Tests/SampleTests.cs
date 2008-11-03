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
	public class SampleTests
	{
		private object Execute(string filename) 
		{
			string s = String.Format("(load \"..\\\\..\\\\..\\\\LSharp.Org\\\\download\\\\{0}\")",filename);
			return Runtime.EvalString(s);
		}
		[Test]
		public void Directory() 
		{
			Execute("Directory.ls");
		}

		[Test]
		public void Factorial() 
		{
			
			Execute("Factorial.ls");
		}

		[Test]
		public void Fibonacci() 
		{
			Execute("Fibonacci.ls");
		}

		[Test]
		public void File() 
		{
			Execute("File.ls");
		}

		[Test]
		public void Greeting() 
		{
			
			Execute("Greeting.ls");
		}

		[Test]
		public void Guid() 
		{
			
			Execute("Guid.ls");
		}

		[Test]
		public void Hashtable() 
		{
			
			Execute("Hashtable.ls");
		}

		[Test]
		public void HelloWorld() 
		{
			
			Execute("HelloWorld.ls");
		}

		[Test]
		public void Http() 
		{
			
			Execute("Http.ls");
		}

		[Test]
		public void Iteration() 
		{
			
			Execute("Iteration.ls");
		}

		[Test]
		public void List() 
		{
			
			Execute("List.ls");
		}

		[Test]
		public void Squares() 
		{
			
			Execute("Squares.ls");
		}

		[Test]
		public void Maths() 
		{
			
			Execute("Maths.ls");
		}
	}
}
