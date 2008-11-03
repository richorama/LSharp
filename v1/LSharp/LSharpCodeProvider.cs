using System;
using System.CodeDom.Compiler;
using System.IO;

namespace LSharp
{
	/// <summary>
	/// Summary description for LSharpCodeProvider.
	/// </summary>
	public class LSharpCodeProvider : CodeDomProvider
	{
		public LSharpCodeProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override ICodeCompiler CreateCompiler() 
		{
			return (new Microsoft.CSharp.CSharpCodeProvider()).CreateCompiler();
		}


		public override ICodeGenerator CreateGenerator() 
		{
			return (new Microsoft.CSharp.CSharpCodeProvider()).CreateGenerator();
		}

		public override ICodeGenerator CreateGenerator(string s) 
		{
			return (new Microsoft.CSharp.CSharpCodeProvider()).CreateGenerator(s);
		}

		public override ICodeGenerator CreateGenerator(TextWriter t) 
		{
			return (new Microsoft.CSharp.CSharpCodeProvider()).CreateGenerator(t);
		}
	}
}
