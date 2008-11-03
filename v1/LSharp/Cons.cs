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
using System.Data;
using System.Collections;

namespace LSharp
{
	/// <summary>
	/// A Cons is a compound object having two components, called the car 
	/// and the cdr, each of which can be any object. Lists are created
	/// by setting the cdr to another cons. Trees are created by setting the car to
	/// another cons.
	/// </summary>
	public class Cons : ICollection
	{
		private Object car;
		private Object cdr;

		/// <summary>
		/// Constructs a new List with one element, the car
		/// </summary>
		/// <param name="car"></param>
		public Cons(Object car) 
		{
			this.car = car;
			this.cdr = null;
		}

		/// <summary>
		/// Constructs a new List with cat at the head and cdr at the tail.
		/// </summary>
		/// <param name="car"></param>
		/// <param name="cdr"></param>
		public Cons(Object car, Object cdr)
		{
			this.car = car;
			this.cdr = cdr;
		}

		/// <summary>
		/// Returns an enumerator for enumerating all elements of the List
		/// started by this cons.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() 
		{
			return (new ListEnumerator(this));
		}

		/// <summary>
		/// Returns the Car (First) of this cons
		/// </summary>
		/// <returns></returns>
		public Object Car() 
		{
			return this.car;
		}

		/// <summary>
		/// Returns the Cdr (Rest) of this cons
		/// </summary>
		/// <returns></returns>
		public Object Cdr() 
		{
			return this.cdr;
		}

		/// <summary>
		/// Returns the Car of the Car of this cons 
		/// </summary>
		/// <returns></returns>
		public Object Caar() 
		{
			return ((Cons)this.Car()).Car();
		}

		/// <summary>
		/// Returns the Car of the Cdr of this cons
		/// </summary>
		/// <returns></returns>
		public Object Cadr() 
		{
			return ((Cons)this.Cdr()).Car();
		}

		/// <summary>
		/// Returns the Cdr of the Car of this cons
		/// </summary>
		/// <returns></returns>
		public Object Cdar() 
		{
			return ((Cons)this.Car()).Cdr();
		}

		/// <summary>
		/// Returns the Cdr or the Cdr of this cons
		/// </summary>
		/// <returns></returns>
		public Object Cddr() 
		{
			return ((Cons)Cdr()).Cdr();
		}

		public Object Caaar() 
		{
			return ((Cons)this.Caar()).Car();
		}

		public Object Caadr() 
		{
			return ((Cons)this.Cdr()).Caar();
		}

		public Object Cadar() 
		{
			return ((Cons)this.Car()).Cadr();
		}

		public Object Caddr() 
		{
			return ((Cons)this.Cdr()).Cadr();
		}
		public Object Cdaar() 
		{
			return ((Cons)this.Car()).Cdar();
		}

		public Object Cdadr() 
		{
			return ((Cons)this.Cdr()).Cdar();
		}

		public Object Cddar() 
		{
			return ((Cons)this.Car()).Cddr();
		}

		public Object Cdddr() 
		{
			return ((Cons)this.Cdr()).Cddr();
		}
		
		public Object Rest() 
		{
			return this.cdr;
		}

		public Object Nth(int n) 
		{
			if (n == 0) return Car();
				else return ((Cons)Cdr()).Nth(n -1);

		}

		public Object First() 
		{
			return this.car;
		}

		public Object Second() 
		{
			return (this.Nth(1));
		}

		public Object Third() 
		{
			return (this.Nth(2));
		}

		public Cons Rplaca(Object obj) 
		{
			this.car = obj;
			return this;
		}

		public Cons Rplacd(Object obj) 
		{
			this.cdr = obj;
			return this;
		}

		public override bool Equals(object obj) 
		{
			if (obj is Cons) 
			{
				Cons that = (Cons)obj;
				
				bool carsEqual = Primitives.Eql(this.Car(), that.Car()); 
				bool cdrsEqual = Primitives.Eql(this.Cdr(), that.Cdr());

				return carsEqual && cdrsEqual;

			} else
				return false;
		}

		/// <summary>
		/// Creates a list of characters from a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Cons FromString(string value) 
		{
			Object cons = null;
			for (int i = value.Length -1 ; i >= 0 ; i--) 
			{
				cons = new Cons(value[i],cons);
			}
			return (Cons) cons;	
		}

		/// <summary>
		/// Creates a list from a one dimensional array
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Cons FromArray(object[] value) 
		{
			Object cons = null;
			for (int i = value.Length -1 ; i >= 0 ; i--) 
			{
				cons = new Cons(value[i],cons);
			}
			return (Cons) cons;	
		}

		/// <summary>
		/// Creates a list given a DataRow
		/// </summary>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		public static Cons FromDataRow (DataRow dataRow) 
		{
			object list = null;
			foreach (object field in dataRow.ItemArray) 
			{
				list = new Cons(field, list);
			}
			return (Cons)list;
		}

		/// <summary>
		/// Creates a list given a DataTable
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		public static Cons FromDataTable (DataTable dataTable) 
		{
			object list = null;
			foreach (DataRow row in dataTable.Rows) 
			{
				object subList = null;
				foreach ( DataColumn column in dataTable.Columns) 
				{
					subList = new Cons(row[column], subList);
				}
				list = new Cons(((Cons)subList).Reverse(), list);
			}
			return (Cons)list;
		}

		/// <summary>
		/// Creates a list given a Hashtable
		/// </summary>
		/// <param name="hashtable"></param>
		/// <returns></returns>
		public static Cons FromHashtable(Hashtable hashtable) 
		{
			object list = null;
			foreach (object key in hashtable.Keys) 
			{
				object value = hashtable[key];
				list = new Cons(new Cons(key, new Cons(value)), list);
			}
			return (Cons)list;
		}

		/// <summary>
		/// Creates a list from a SortedList
		/// </summary>
		/// <param name="sortedList"></param>
		/// <returns></returns>
		public static Cons FromSortedList(SortedList sortedList) 
		{
			object list = null;
			foreach (object key in sortedList.Keys) 
			{
				object value = sortedList[key];
				list = new Cons(new Cons(key, new Cons(value)), list);
			}
			return (Cons)list;
		}

		/// <summary>
		/// Creates a list from any ICollection
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static Cons FromICollection (ICollection collection) 
		{
			object list = null;
			foreach (object o in collection) 
			{
				list = new Cons(o, list);
			}
			return (Cons)list;
		}

		/// <summary>
		/// Returns the length of the list
		/// </summary>
		/// <returns></returns>
		public int Length() 
		{
			int i=0;
			object o = this;
			while (o != null) 
			{
				i += 1;
				o = ((Cons)o).Cdr();
			}
			return i;
		}

		/// <summary>
		/// Returns the list expressed as an array of objects
		/// </summary>
		/// <returns></returns>
		public object[] ToArray() 
		{
			object[] objects = new object[this.Length()];

			int i =0;
			object o = this;
			while (o != null) 
			{
				objects[i] = ((Cons)o).Car();
				i += 1;
				o = ((Cons)o).Cdr();
			}
			return objects;
		}

		/// <summary>
		/// Returns the last item in a list
		/// </summary>
		/// <returns></returns>
		public object Last() 
		{

			object o = this;
			while ((o is Cons) && (((Cons)o).Cdr() != null)) 
			{
				o = ((Cons)o).Cdr();
			}

            return o;
		}

		/// <summary>
		/// Returns a new list with the items reversed
		/// </summary>
		/// <returns></returns>
		public Cons Reverse () 
		{
			object list = null;
			object o = this;
			while ( o != null) 
			{
				list = new Cons(((Cons) o).Car(),list);
				o = ((Cons)o).Cdr();
			}
			return (Cons)list;
		}

        public Cons Copy()
        {
            return new Cons(this.Car(), this.Cdr());
        }

        /// <summary>
        /// Returns a shallow copy of the list given as its argument
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public Cons CopyList()
        {
            Cons result;
            Cons dest;
           
            object nextCons = this;

            // Copy the first cons cell
            dest = ((Cons)nextCons).Copy();
            result = dest;
            nextCons = ((Cons)nextCons).Cdr();

            // Copy succesive cells
            while (nextCons is Cons)
            {
                dest.Rplacd(((Cons)nextCons).Copy());
                dest = (Cons)dest.Cdr();
                nextCons = ((Cons)nextCons).Cdr();
            }
            
            return result;
        }

        public void CopyTo(Array array, int index)
        {
            int i =0;
            object o = this;
            while (o != null) 
            {
                if (i >= index)
                {
                    array.SetValue(((Cons)o).Car(), i);
                }
                i += 1;
                o = ((Cons)o).Cdr();
            }
        }

        public int Count
        {
            get { return Length(); }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Overrides the default ToString to return the string representation of the list
        /// as produced by the L Sharp Printer.
        /// </summary>
        /// <returns>The string represention of the list.</returns>
        public override string ToString()
        {
            return Printer.WriteToString(this);
        }

    }
}
