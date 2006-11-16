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
using System.Collections;
using System.Reflection;
using System.IO;

namespace LSharp
{
	/// <summary>
	/// Keeps a list of the assemblies that are loaded. Ensures that we don't
	/// load an assembly more than once. Uses a Singleton design pattern (there can
	/// only be one AssemblyCache).
	/// </summary>
	public class AssemblyCache
	{
		private static AssemblyCache instance;

		private const int CAPACITY = 20;
		private Hashtable assemblyTable = new Hashtable(CAPACITY);

		/// <summary>
		/// Private constructor ensures singleton design pattern
		/// </summary>
		private AssemblyCache()
		{
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Add(assembly);
            }
		}

		/// <summary>
		/// Returns the singleton instance (creating it if necessary).
		/// </summary>
		/// <returns></returns>
		public static AssemblyCache Instance() 
		{
			if (instance == null)
				instance = new AssemblyCache();
			return instance;
		}

		/// <summary>
		/// Loads an assembly, either from the GAC or from a file
		/// </summary>
		/// <param name="assembly">An assembly name or assembly file name</param>
		/// <returns>An Assembly object</returns>
		public Assembly LoadAssembly (string assembly) 
		{
			object o = assemblyTable[assembly];
			if (o == null) 
			{
                if (Path.IsPathRooted(assembly))
                    o = Assembly.LoadFrom(assembly);
                else
                    // TODO change this to Assembly.Load
                    // o = Assembly.Load(assembly);
					o = Assembly.LoadWithPartialName(assembly);
				
				assemblyTable[assembly] = o;
			}

			return (Assembly) o;
		}

		/// <summary>
		/// Adds a new assembly to the assembly cache
		/// </summary>
		/// <param name="assembly"></param>
		public void Add(Assembly assembly) 
		{
			assemblyTable[assembly.FullName] = assembly;
		}

		/// <summary>
		/// Removes an assembly from the assembly cache
		/// </summary>
		/// <param name="assembly"></param>
		public void Remove(Assembly assembly) 
		{
			assemblyTable.Remove(assembly.FullName);
		}


		/// <summary>
		/// Returns an array of all loaded assemblies
		/// </summary>
		/// <returns></returns>
		public Assembly[] Assemblies () 
		{
			return (Assembly[]) new ArrayList(assemblyTable.Values).ToArray(typeof(Assembly));
		}
	}
}
