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
	public class ClassBuilder
	{
		
		public static Type CreateClass(string className, string superClass, string interfaces)
		{

			StringBuilder source = new StringBuilder("using System;");

			
			//source.Append("class T{");
			if (superClass == null)
				source.Append(string.Format("class {0} {{",className));
			else
				source.Append(string.Format("class {0}:{1} {{",className, superClass));
			//source.Append("string _name;");
			//source.Append("public T(string name){");
			//source.Append(string.Format("public {0}(string name) {{", className));
			//source.Append("_name=name;}");
			//source.Append("public void print(){");
			//source.Append("Console.WriteLine(\"Hello \"+_name);}");
			source.Append("}");

			Console.WriteLine(source.ToString());

			CSharpCodeProvider cscompiler = new CSharpCodeProvider();
			ICodeCompiler compiler = cscompiler.CreateCompiler();
			CompilerParameters compparams = new CompilerParameters();
			compparams.GenerateInMemory = true;

//			foreach (Assembly a in AssemblyCache.Instance().Assemblies()) {
//				compparams.ReferencedAssemblies.Add(a.FullName);
//			}
																				 
			CompilerResults compresult = compiler.CompileAssemblyFromSource(
			compparams, source.ToString());
			if ( compresult == null | compresult.Errors.Count > 0 ) 
			{
				foreach (CompilerError e in compresult.Errors) 
				{
					Console.WriteLine(e);
				}
				throw new Exception("class error");
			}

			Object o = TypeCache.Instance().FindType(className);
			if (o != null)
			{
				Assembly a = Assembly.GetAssembly(o as Type);
				AssemblyCache.Instance().Remove(a);
			}

			AssemblyCache.Instance().Add(compresult.CompiledAssembly);

			return compresult.CompiledAssembly.GetType(className);
		}


	}
}
