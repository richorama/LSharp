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
	/// An IEnumerator implementation allowing Lists to be enumerated
	/// </summary>
	public class ListEnumerator : System.Collections.IEnumerator
	{
		private Cons list;
		private Cons originalList;

		public ListEnumerator(Cons list)
		{
			// The null at the head is a dummy which the first MoveNext discards
			this.originalList = new Cons(null,list);
			this.list = originalList;
		}

		public Object Current
		{
			get
			{
				return list.Car();
			}
		}

		public bool MoveNext()
		{
			object o = list.Cdr();
			if (o==null)
				return false;
			
			if (o is Cons)
			{
				list = (Cons)o;
			} 
			else 
			{
				list = new Cons(o);
			}

			return true;
		}

		public void Reset() 
		{
			list = this.originalList;
		}
		
	}
}
