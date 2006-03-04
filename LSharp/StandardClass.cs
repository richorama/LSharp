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

// Highly experimental and unusable class definition facility !

using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
namespace LSharp
{
	/// <summary>
	/// Experimental, unfinished
	/// </summary>
	public class StandardClass
	{
		//private CompilerResults compresult;
		private string className;

		private Type type;

		// super class
		// implements list
		// slots

		public StandardClass(string className)
		{
			this.className = className;

			StringBuilder source = new StringBuilder("using System;");
			//source.Append("class T{");
			source.Append(string.Format("class {0} {{",className));
			source.Append("string _name;");
			//source.Append("public T(string name){");
			source.Append(string.Format("public {0}(string name) {{", className));
			source.Append("_name=name;}");
			source.Append("public void print(){");
			source.Append("Console.WriteLine(\"Hello \"+_name);}");
			source.Append("}");

			Console.WriteLine(source.ToString());

			CSharpCodeProvider cscompiler = new CSharpCodeProvider();
			ICodeCompiler compiler = cscompiler.CreateCompiler();
			CompilerParameters compparams = new CompilerParameters();
			compparams.GenerateInMemory = true;
			CompilerResults compresult = compiler.CompileAssemblyFromSource(
				compparams, source.ToString());
			if ( compresult == null | compresult.Errors.Count > 0 )
				throw new Exception("class error");

			type = compresult.CompiledAssembly.GetType(className);
			//systemreturn test;

//			object o = compresult.CompiledAssembly.CreateInstance("T", false, 
//				BindingFlags.CreateInstance, null,new object[]{"Filip"}, null, null);
//			Type test = compresult.CompiledAssembly.GetType("T");
//			MethodInfo m = test.GetMethod("print");
//			m.Invoke(o, null);
		}

		public Type Class() 
		{

//			Type test = compresult.CompiledAssembly.GetType(className);
//			return test;

			return type;
		}

//		public void foo() 
//		{
//			object o = compresult.CompiledAssembly.CreateInstance(className, false, 
//				BindingFlags.CreateInstance, null,new object[]{"Filip"}, null, null);
//			Type test = compresult.CompiledAssembly.GetType("T");
//			MethodInfo m = test.GetMethod("print");
//			m.Invoke(o, null);
//		}
	}
}
