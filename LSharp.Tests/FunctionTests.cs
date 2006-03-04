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
	public class FunctionTests
	{
		private object Execute(string filename) 
		{
			string s = String.Format("(load \"..\\\\..\\\\functiontestcases\\\\{0}\")",filename);
			return Runtime.EvalString(s);
		}

		[Test]
		public void Add() 
		{
			bool b = (bool)Execute("add.ls");

			Assert.IsTrue(b);
		}

        [Test]
        public void Append()
        {
            bool b = (bool)Execute("append.ls");

            Assert.IsTrue(b);
        }

		[Test]
		public void Apply() 
		{
			bool b = (bool)Execute("apply.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Assoc() 
		{
			bool b = (bool)Execute("assoc.ls");

			Assert.IsTrue(b);
		}
		[Test]
		public void Caaar() 
		{
			bool b = (bool)Execute("caaar.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Caadr() 
		{
			bool b = (bool)Execute("caadr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Caar() 
		{
			bool b = (bool)Execute("caar.ls");

			Assert.IsTrue(b);
		}

		
		[Test]
		public void Cadar() 
		{
			bool b = (bool)Execute("cadar.ls");

			Assert.IsTrue(b);
		}
		

		[Test]
		public void Caddr() 
		{
			bool b = (bool)Execute("caddr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cadr() 
		{
			bool b = (bool)Execute("cadr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Car() 
		{
			bool b = (bool)Execute("car.ls");

			Assert.IsTrue(b);
		}
		
		[Test]
		public void Cdaar() 
		{
			bool b = (bool)Execute("cdaar.ls");

			Assert.IsTrue(b);
		}
		
		[Test]
		public void Cdar() 
		{
			bool b = (bool)Execute("cdar.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cddar() 
		{
			bool b = (bool)Execute("cddar.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cdddr() 
		{
			bool b = (bool)Execute("cdddr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cddr() 
		{
			bool b = (bool)Execute("cddr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cdr() 
		{
			bool b = (bool)Execute("cdr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Cons() 
		{
			bool b = (bool)Execute("cons.ls");

			Assert.IsTrue(b);
		}

        [Test]
        public void CopyList()
        {
            bool b = (bool)Execute("copy-list.ls");

            Assert.IsTrue(b);
        }

		[Test]
		public void Divide() 
		{
			bool b = (bool)Execute("divide.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Environment() 
		{
			bool b = (bool)Execute("environment.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Eq() 
		{
			bool b = (bool)Execute("eq.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Eql() 
		{
			bool b = (bool)Execute("eql.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Eval() 
		{
			bool b = (bool)Execute("eval.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Exit() 
		{
			bool b = (bool)Execute("exit.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void First() 
		{
			bool b = (bool)Execute("first.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void GreaterThan() 
		{
			bool b = (bool)Execute("greaterthan.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void GreaterThanEqual() 
		{
			bool b = (bool)Execute("greaterthanequal.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Inspect() 
		{
			bool b = (bool)Execute("inspect.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Is() 
		{
			bool b = (bool)Execute("is.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void LessThan() 
		{
			
			bool b = (bool)Execute("lessthan.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void LessThanEqual() 
		{
			
			bool b = (bool)Execute("lessthanequal.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void List() 
		{
			
			bool b = (bool)Execute("list.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Load() 
		{
			string currrentDirectory = System.Environment.CurrentDirectory;

			System.Environment.CurrentDirectory = "..\\\\..\\\\functiontestcases\\\\";
			bool b = (bool)Runtime.EvalString("(load \"load.ls\")");

			System.Environment.CurrentDirectory = currrentDirectory;

		}

		[Test]
		public void MacroExpand() 
		{
			
			bool b = (bool)Execute("macroexpand.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Map() 
		{
			
			bool b = (bool)Execute("map.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Multiply() 
		{
			
			bool b = (bool)Execute("multiply.ls");

			Assert.IsTrue(b);
		}

        [Test]
        public void NConc()
        {

            bool b = (bool)Execute("nconc.ls");

            Assert.IsTrue(b);
        }

		[Test]
		public void New() 
		{
			
			bool b = (bool)Execute("new.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Not() 
		{
			
			bool b = (bool)Execute("not.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void NotEql() 
		{
			
			bool b = (bool)Execute("noteql.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Nth() 
		{
			
			bool b = (bool)Execute("nth.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Pr() 
		{
			
			bool b = (bool)Execute("pr.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Prl() 
		{
			
			bool b = (bool)Execute("prl.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Read() 
		{
			
			bool b = (bool)Execute("read.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Reference() 
		{
			
			bool b = (bool)Execute("reference.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Reset() 
		{
			
			bool b = (bool)Execute("reset.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Rest() 
		{
			
			bool b = (bool)Execute("rest.ls");

			Assert.IsTrue(b);
		}

        [Test]
        public void SymbolName()
        {

            bool b = (bool)Execute("symbol-name.ls");

            Assert.IsTrue(b);
        }

		[Test]
		public void Subtract() 
		{
			
			bool b = (bool)Execute("subtract.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Trace() 
		{
			
			bool b = (bool)Execute("trace.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Throw() 
		{
			
			bool b = (bool)Execute("throw.ls");

			Assert.IsTrue(b);
		}

		[Test]
		public void Using() 
		{
			
			bool b = (bool)Execute("using.ls");

			Assert.IsTrue(b);
		}
	}
}
