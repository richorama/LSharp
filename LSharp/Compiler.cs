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
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

namespace LSharp
{
	/// <summary>
	/// Compile an L Sharp program. Actually this is just a proof of concept
	/// at the moment and is not yet implemented.
	/// Maybe this should use CODEDOM ?
	/// </summary>
	public class Compiler
	{


		public static void Compile( string filename, object program) 
		{
			AppDomain appDomain = Thread.GetDomain();
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = Path.GetFileName(filename);
			AssemblyBuilder assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName,AssemblyBuilderAccess.RunAndSave,Path.GetDirectoryName(filename));
		
			
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(Path.GetFileName(filename), Path.GetFileName(filename));

			TypeBuilder typeBuilder = moduleBuilder.DefineType(Path.GetFileName(filename));

			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Static | MethodAttributes.Public,typeof(void),null);

			ILGenerator iLGenerator = methodBuilder.GetILGenerator();

			iLGenerator.EmitWriteLine("The LSharp Compiler is not yet available");

			iLGenerator.Emit(OpCodes.Ret);
			typeBuilder.CreateType();
			assemblyBuilder.SetEntryPoint((moduleBuilder.GetType(Path.GetFileName(filename))).GetMethod("Main"));


			assemblyBuilder.Save(Path.GetFileName(filename));
		}
	}
}
