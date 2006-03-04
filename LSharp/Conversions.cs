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
using System.Data;
using System.Text;

namespace LSharp
{
	/// <summary>
	/// Allows conversions between various .net data structures and LSharp types
	/// </summary>
	public class Conversions
	{
		/// <summary>
		/// The is Cast on steroids
		/// </summary>
		/// <param name="type">The type to cast to</param>
		/// <param name="value">The value to be cast</param>
		/// <returns></returns>
		public static object The (object type, object value) 
		{
			
			// String to List
			if ((type == typeof (Cons)) && (value is System.String))
				return Cons.FromString(value as string);

			// List to String
			if ((type == typeof (System.String)) && (Primitives.IsList(value)))
				return ConsToString(value);

			// List to Array
			if ((type == typeof (Array)) && (value is Cons))
				return ((Cons)value).ToArray();

			// Array to List
			if ((type == typeof (Cons)) && (value is Array ))
				return Cons.FromArray(value as System.Object[]);

			// List to Hashtable
			if ((type == typeof (Hashtable)) && (Primitives.IsList(value)))
				return Conversions.ConsToHashtable(value);

			// Hashtable to List
			if ((type == typeof (LSharp.Cons)) && (value is Hashtable))
				return Cons.FromHashtable((Hashtable)value);

			// SortedList to List
			if ((type == typeof (LSharp.Cons)) && (value is SortedList))
				return Cons.FromSortedList((SortedList)value);

			// DataTable to List
			if ((type == typeof (LSharp.Cons)) && (value is System.Data.DataTable))
				return Cons.FromDataTable((System.Data.DataTable)value);

			// List to Stack
			if ((type == typeof (Stack)) && (Primitives.IsList(value)))
				return Conversions.ConsToStack(value);

			// List to Queue
			if ((type == typeof (Queue)) && (Primitives.IsList(value)))
				return ConsToQueue(value);

			// List to SortedList
			if ((type == typeof (SortedList)) && (Primitives.IsList(value)))
				return Conversions.ConsToSortedList(value);

			// Collection to List
			if ((type == typeof (LSharp.Cons)) && (value is ICollection))
				return Cons.FromICollection((ICollection)value);

			// DataRow to List
			if ((type == typeof (LSharp.Cons) && (value is DataRow)))
				return (Cons.FromDataRow((DataRow) value));

			// Anything to String
			if (type == typeof (System.String))
				return value.ToString();

			// Anything to Boolean true or false
			if (type == typeof (System.Boolean))
				return ObjectToBoolean(value);

			// Allow upcasting for types eg (the Type (typeof string))  
			if (((Type)type).IsInstanceOfType (value))
				return value;

			// Revert to standard type cast
			return Convert.ChangeType(value, (Type)type);
			
		}

		/// <summary>
		/// Converts any LSharp form (actually any object) to a
		/// Boolean true or false.
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static bool ObjectToBoolean(Object arg) 
		{
			// Null counts as false
			if (arg == null)
				return false;

			// The boolean false is clearly false
			if ((arg is bool) && (!(bool)arg))
				return false;

			// Empty strings are false
			if ((arg is string) && (arg.ToString() == ""))
				return false;

			// Zero is false
			if ((arg.GetType() == typeof(System.Double)) && ((Double)arg == 0))
				return false;
			if ((arg.GetType() == typeof(System.Int32)) && ((int)arg == 0))
				return false;
			

			return true;
		}

		/// <summary>
		/// Converts a list to a string
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static string ConsToString(Object o) 
		{
			if (o == null)
				return "";
			else 
			{
				StringBuilder stringBuilder = new StringBuilder();
			
				stringBuilder.Append(((Cons)o).Car());
				stringBuilder.Append(ConsToString(((Cons)o).Cdr()));
				return stringBuilder.ToString();
			}

		}

		/// <summary>
		/// Converts a list to a Queue
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Queue ConsToQueue(Object o) 
		{
	
			Queue queue = new Queue();

			Object temp = o;
			while (temp!= null) 
			{
				queue.Enqueue(((Cons)temp).First());
				temp = ((Cons)temp).Rest();
			}
			return queue;
		}

		


		/// <summary>
		/// Returns a new Stack poluated with data items from the given
		/// LSharp.List object
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Stack ConsToStack(Object o) 
		{
	
			Stack stack = new Stack();

			// null signifies an empty stack
			if (o == null)
				return stack;

			Object temp = o; 
			while (temp!= null) 
			{
				stack.Push(((Cons)temp).First());
				temp = ((Cons)temp).Rest();
			}
			return stack;
		}


		/// <summary>
		/// Converts a list to a SortedList
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static SortedList ConsToSortedList(Object o) 
		{
			SortedList sortedList = new SortedList();

			Object temp = o;
			while (temp!= null) 
			{
				Cons element = (Cons) ((Cons)temp).First();
				sortedList.Add(element.First(), element.Second());
				temp = ((Cons)temp).Rest();
			}
			return sortedList;
		}



		/// <summary>
		/// Converts a list to a Hashtable
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Hashtable ConsToHashtable(Object o) 
		{
			Hashtable hashtable = new Hashtable();

			Object temp = o;
			while (temp!= null) 
			{
				Cons element = (Cons) ((Cons)temp).First();

				hashtable[element.First()] = element.Second();
				temp = ((Cons)temp).Rest();
			}
			return hashtable;
		}
	}
}
