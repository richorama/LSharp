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
	public class SpecialFormTests
	{

		private object Execute(string filename) 
		{
			string s = String.Format("(load \"..\\\\..\\\\specialformtestcases\\\\{0}\")",filename);
			return Runtime.EvalString(s);
		}

		[Test]
		public void And() 
		{
			
			bool b = (bool)Execute("and.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Backquote() 
		{
			
			bool b = (bool)Execute("backquote.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Call() 
		{
			
			bool b = (bool)Execute("call.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Compile() 
		{
			
			bool b = (bool)Execute("compile.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Cond() 
		{
			
			bool b = (bool)Execute("cond.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Decrement() 
		{
			
			bool b = (bool)Execute("decrement.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Do() 
		{
			
			bool b = (bool)Execute("do.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Each() 
		{
			
			bool b = (bool)Execute("each.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Fn() 
		{
			
			bool b = (bool)Execute("fn.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void For() 
		{
			
			bool b = (bool)Execute("for.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Foreach() 
		{
			
			bool b = (bool)Execute("foreach.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void If() 
		{
			
			bool b = (bool)Execute("if.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Increment() 
		{
			
			bool b = (bool)Execute("increment.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Let() 
		{
			
			bool b = (bool)Execute("let.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Macro() 
		{
			
			bool b = (bool)Execute("macro.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Or() 
		{
			
			bool b = (bool)Execute("or.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Quote() 
		{
			
			bool b = (bool)Execute("quote.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Setq() 
		{
			
			bool b = (bool)Execute("setq.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void The() 
		{
			
			bool b = (bool)Execute("the.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void To() 
		{
			
			bool b = (bool)Execute("to.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void Try() 
		{
			
			bool b = (bool)Execute("try.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void When() 
		{
			
			bool b = (bool)Execute("when.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void While() 
		{
			
			bool b = (bool)Execute("while.ls");
			Assert.IsTrue(b);
		}

		[Test]
		public void With() 
		{
			
			bool b = (bool)Execute("with.ls");
			Assert.IsTrue(b);
		}
	}
}
