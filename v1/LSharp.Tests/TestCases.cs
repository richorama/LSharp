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
	public class TestCases
	{
		
		private object Execute(string filename) 
		{
			string s = String.Format("(load \"..\\\\..\\\\testcases\\\\{0}\")",filename);
			return Runtime.EvalString(s);
		}


		[Test]
		public void Identity() 
		{
			
			string s = (String)Execute("Identity.ls");
			Assert.AreEqual("Hello World",s);
		}

		[Test]
		public void Closure() 
		{
			
			object d = Execute("closure.ls");
			Assert.AreEqual(15,d);
		}

		[Test]
		public void NoArgs() 
		{
			 
			Object d = Execute("noargs.ls");
			Assert.AreEqual(3,d);
		}

		[Test]
		public void Format() 
		{
			
			String result = (string)Execute("format.ls");
			Assert.AreEqual("A,B,C", result);
		}

		[Test]
		public void Global() 
		{
			
			int result = (int)Execute("global.ls");
			Assert.AreEqual(20,result);	
		}

		[Test]
		public void Local() 
		{
			
			int result = (int)Execute("local.ls");
			Assert.AreEqual(10,result);
		}

		[Test]
		public void RuntimeTypeTest() 
		{
			string result = (string)Execute("runtimetype.ls");
			Assert.AreEqual("System.DateTime",result);
		}

		[Test]
		public void Upcasting() 
		{
			Type  result = (Type)Execute("upcasting.ls");
			Assert.AreEqual(typeof(System.String),result);
		}
		
		
	}
}
