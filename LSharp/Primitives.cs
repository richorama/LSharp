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

namespace LSharp
{
	/// <summary>
	/// LSharp primitives and utilities.
	/// </summary>
	public class Primitives
	{
		public static bool Eql(object x, object y) 
		{
			
			if ( x == null | y == null)
				return x == y;

			return (x.Equals(y));
			
		}

		public static bool Eql(Cons args)
		{
			object last = args.First();

			foreach (object item in (Cons)args.Rest()) 
			{
				if (!(Eql(last,item)))
					return false;
				last = item;
			}
			return true;
		}

		public static bool Eq(Cons args)
		{
			object last = args.First();

			foreach (object item in (Cons)args.Rest()) 
			{
				if (!(object.ReferenceEquals(last,item)))
					return false;
				last = item;
			}
			return true;
		}
		/// <summary>
		/// Returns true if the given object is an atom, false otherwise
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsAtom(Object x) 
		{
			if (x == null)
				return true;
			else
				return (x.GetType() != typeof(Cons));
		}

		/// <summary>
		/// Returns true if the Object is a List, false otherwise
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsList(Object x) 
		{
			if (x == null)
				return true;
			else
				return (x.GetType() == typeof(Cons));
		}

		public static void ProcessArguments(Cons argumentNameList, Cons argumentList, Environment localEnvironment) 
		{
			while (argumentNameList != null) 
			{
				Symbol argumentName = (Symbol)argumentNameList.Car();
				if (argumentName.ToString() == "&rest") 
				{
					argumentName = (Symbol)argumentNameList.Cadr();
					localEnvironment.AssignLocal(argumentName, argumentList);
					argumentNameList = null;
				} 
				else 
				{
					localEnvironment.AssignLocal(argumentName, argumentList.Car());
					argumentList = (Cons)argumentList.Cdr();
					argumentNameList = (Cons)argumentNameList.Cdr();
				}

			}
		}
		

	}
}
