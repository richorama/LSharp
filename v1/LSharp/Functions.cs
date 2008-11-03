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
using System.IO;
using System.Collections;

namespace LSharp
{
	/// <summary>
	/// Definitions of built in L Sharp functions
	/// </summary>
	public class Functions
	{	

		/// <summary>
		/// (add object*)
		/// Returns the sum of all the specified objects. 
		/// Each object must be a numerical type usch as System.In32 or System.Double.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Add(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			Double result = 0;
			foreach (Object item in args) 
			{
				if (item is Double)
					type = item.GetType();

				result += Convert.ToDouble(item);
			}
			return Convert.ChangeType(result,type);

		}

        // TODO

        public static Object Append(Cons args, Environment environment)
        {
            if (args.Rest() == null)
            {
                return args.First();
            }
            else
            {
                Cons result;

                if (args.First() == null)
                {
                    result = (Cons)Append((Cons)args.Rest(), environment);   
                }
                else
                {
                    result = ((Cons)args.First()).CopyList();
                    ((Cons)result.Last()).Rplacd(Append((Cons)args.Rest(), environment));
                }
                return result;
            }
        }

		/// <summary>
		/// (apply function list)
		/// Applies function to a list of arguments. function may be a built-in lsharp function, 
		/// a closure defined by fn a macro defined by macro or the name of a method in the 
		/// .NET framework. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Apply(Cons args, Environment environment) 
		{
			return Runtime.Apply(args.First(), args.Second(),environment);
		}

		/// <summary>
		/// (assoc item alist)
		/// return the first cons in alist whose car is equal to item, 
		/// or nil if no such cons is found.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Assoc(Cons args, Environment environment) 
		{
			object item = args.First();
			Cons list = (Cons) args.Second();

			foreach (Cons c in list) 
			{
				if (c.Car().Equals (item))
					return c;
			}
			return null;
			
		}

		/// <summary>
		/// (caaar list) A shorthand for (car (car (car list)))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Caaar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Caaar();

			throw new LSharpException(string.Format("Caaar: {0} is not a List",o));
		}

		/// <summary>
		/// (caadr list)
		/// A shorthand for (car (car (cdr list)))  
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Caadr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Caadr();

			throw new LSharpException(string.Format("Caadr: {0} is not a List",o));
		}

		/// <summary>
		/// (caar list) 
		/// A shorthand for (car (car list))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Caar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Caar();

			throw new LSharpException(string.Format("Caar: {0} is not a List",o));
		}

		/// <summary>
		/// (cadar list)
		/// A shorthand for (car (cdr (car list)))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cadar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cadar();

			throw new LSharpException(string.Format("Cadar: {0} is not a List",o));
		}

		/// <summary>
		/// (caddr list)
		/// A shorthand for (car (cdr (cdr list))) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Caddr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Caddr();

			if (o is IEnumerable) 
			{
				IEnumerator e = ((IEnumerable) o).GetEnumerator();
				e.MoveNext();
				e.MoveNext();
				e.MoveNext();
				return e.Current;
			}

			throw new LSharpException(string.Format("Caddr: {0} is not a IEnumerable",o));
		}

		/// <summary>
		/// (cadr list)
		/// A shorthand for (car (cdr list)) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cadr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cadr();

			if (o is IEnumerable) 
			{
				IEnumerator e = ((IEnumerable) o).GetEnumerator();
				e.MoveNext();
				e.MoveNext();
				return e.Current;
			}

			throw new LSharpException(string.Format("Cadr: {0} is not a IEnumerable",o));
		}

		/// <summary>
		/// (car list) 
		/// Returns the first element of a list or other IEnumerable data structure
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Car(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).First();

			if (o is IEnumerable) 
			{
				IEnumerator e = ((IEnumerable) o).GetEnumerator();
				e.MoveNext();
				return e.Current;
			}

			throw new LSharpException(string.Format("Car: {0} is not IEnumerable",o));
		}

		/// <summary>
		/// (cdaar list)
		/// A shorthand for (cdr (car (car x))) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cdaar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cdaar();

			throw new LSharpException(string.Format("Cdaar: {0} is not a List",o));
		}

		/// <summary>
		/// (cdadr list)
		/// A shorthand for (cdr (car (cdr x))) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cdadr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cdadr();

			throw new LSharpException(string.Format("Cdadr: {0} is not a List",o));
		}

		/// <summary>
		/// (cdar list)
		/// A shorthand for (cdr (car x)) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cdar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cdar();

			throw new LSharpException(string.Format("Caar: {0} is not a List",o));
		}

		/// <summary>
		/// (cddar list)
		/// A shorthand for (cdr (cdr (car x))) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cddar(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cddar();

			throw new LSharpException(string.Format("Cddar: {0} is not a List",o));
		}

		/// <summary>
		/// (cdddr list)
		/// A shorthand for (cdr (cdr (cdr list)))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cdddr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cdddr();
			if (o is string)
				return ((string)o).Substring(3);
			throw new LSharpException(string.Format("Cdddr: {0} is not a String or a List",o));
		}

		/// <summary>
		/// (cddr list)
		/// A shorthand for (cdr (cdr list))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cddr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Cddr();
			if (o is string)
				return ((string)o).Substring(2);
			throw new LSharpException(string.Format("Cddr: {0} is not a String or a List",o));
		}

		/// <summary>
		/// Returns the Cdr (second part) of a cons
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cdr(Cons args, Environment environment) 
		{
			object o = args.First();
			if (o is Cons)
				return ((Cons)o).Rest();
			if (o is string)
				return ((string)o).Substring(1);
			throw new LSharpException(string.Format("Cdr: {0} is not a String or a List",o));
		}

		/// <summary>
		/// Creates a fresh cons, the car of which is object-1 and the cdr of which is object-2. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cons(Cons args, Environment environment) 
		{
			if (args.Length() == 1)
				return args.First();
            if (args.Length() == 2) 
			    return new Cons(args.First(),Cons((Cons)args.Rest(), environment));

            throw new LSharpException("Too many arguments given to cons");
		}

        
        
        /// <summary>
        /// Returns a shallow copy of the list given as its argument
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object CopyList(Cons args, Environment environment) 
        {
            if (args.First() is Cons)
            {
                return ((Cons)args.First()).CopyList();
            }
            else
            {
                throw new LSharpException(args.First().ToString() + " is not a list.");
            }
        }

		/// <summary>
		/// (/ numerator denominator+) 
		/// Divides a numerator by one or more denominators
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Divide(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			Double result = Convert.ToDouble(args.First());
			foreach (object item in (Cons)args.Rest()) 
			{
				if (item is Double)
					type = item.GetType();

				result /= Convert.ToDouble(item);
			}
			return Convert.ChangeType(result,type);
		}

		/// <summary>
		/// Returns an object representing the curent lexical environment
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Environment(Cons args, Environment environment) 
		{
			return environment;		
		}

		/// <summary>
		/// (eq expression*) Returns true if all expressions are reference equal, 
		/// that is they refer to the same object in memory. As a special case, null is eq to null. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Eq(Cons args, Environment environment) 
		{
			return Primitives.Eq(args);
		}

		/// <summary>
		/// (eql expression*) Returns true if all expressions are equal, that is 
		/// their implementations of equal return true. As a special case, null 
		/// is eql to null. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Eql(Cons args, Environment environment) 
		{
			return Primitives.Eql(args);
		}

		/// <summary>
		/// Evaluates an LSharp expression in a given environment
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Eval(Cons args, Environment environment) 
		{
            if (args.Length() == 1)
                return Runtime.Eval(args.First(), environment);
            else
                throw new LSharpException("Incorrect arguments given to eval");
		}

		/// <summary>
		/// Evaluates an LSharp expression contained in a string within a given
		/// environment
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object EvalString(Cons args, Environment environment) 
		{
			return Runtime.EvalString (args.First().ToString(), environment);		
		}

		/// <summary>
		/// (exit [exit-code])
		/// Terminates the current process and returns the specified exit-code to the 
		/// calling process.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Exit(Cons args, Environment environment) 
		{
			if (args == null)
				System.Environment.Exit(0);
			else
				System.Environment.Exit((int)args.First());
				
			return null;
		}

        /// <summary>
        /// Generates a new symbol
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object Gensym(Cons args, Environment environment)
        {
            return new Symbol("#:G" + Guid.NewGuid());
        }

		/// <summary>
		/// (> object1 object2 object*) Returns true if object1 is greater than 
		/// object2, object2 is greater than object3 and so on. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object GreaterThan(Cons args, Environment environment) 
		{
			Double last = Convert.ToDouble(args.First());

			foreach (object item in (Cons)args.Rest()) 
			{
				Double current = Convert.ToDouble(item);
				if (!(last > current))
					return false;
				last = current;
			}
			return true;
		}

		/// <summary>
		/// (>= object1 object2 object*) Returns true if object1 is greater 
		/// than or eql to object2, object2 is greater than or eql to object3 and so on. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object GreaterThanEqual(Cons args, Environment environment) 
		{
			Double last = Convert.ToDouble(args.First());

			foreach (object item in (Cons)args.Rest()) 
			{
				Double current = Convert.ToDouble(item);
				if (!(last >= current))
					return false;
				last = current;
			}
			return true;
		}

        /// <summary>
        /// (handle-event target eventName handler)
        /// Sets up a new event handler for events named eventName on target. The
        /// handler is an LSharp closure with two arguments, the sender and the
        /// event arguments (defun fn (sender args) (prl "Event Handled")).
        /// Experimental.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object HandleEvent(Cons args, Environment environment)
        {
            return EventAdapter.AddEventHandler(args.First(), (string)args.Second(), (Closure)args.Third());
        }

		/// <summary>
		/// (inspect object) Returns a description of the specified object, using reflection. 
		/// Useful for debugging. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Inspect(Cons args, Environment environment) 
		{
			object o = args.First();
			string s = Inspector.Inspect(o);
			Console.WriteLine(s);
			return null;
		}

		/// <summary>
		/// (is type expression) 
		/// Used to check whether the run-time type of an object is 
		/// compatible with a given type.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Is(Cons args, Environment environment) 
		{
			object obj = args.Second();

            TypeCache typeCache = TypeCache.Instance();
            string typeName = args.First().ToString();
            Type type = typeCache.FindType(typeName);

			object result =  (((Type)type).IsInstanceOfType (obj));

            return result;

		}


		/// <summary>
		/// (length expression)
		/// Returns the length of expression. If expression is null, length returns 0, 
		/// otherwise the length is calculated by calling the length method on the object, 
		/// ensuring that length works for strings, lists and most collection-like objects. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Length(Cons args, Environment environment) 
		{
			object o = args.Car();
			if (o == null)
				return 0;
			else
				return Runtime.Call("length",args);
		}


		/// <summary>
		/// (< object1 object2 object*) Less than
		/// Returns true if object1 is less than object2 and object2 is less than object3 and 
		/// so on.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object LessThan(Cons args, Environment environment) 
		{
			Double last = Convert.ToDouble(args.First());

			foreach (object item in (Cons)args.Rest()) 
			{
				Double current = Convert.ToDouble(item);
				if (!(last < current))
					return false;
				last = current;
			}
			return true;
		}

		/// <summary>
		/// (<= object1 object2 object*) Less than or equal 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object LessThanEqual(Cons args, Environment environment) 
		{
			Double last = Convert.ToDouble(args.First());

			foreach (object item in (Cons)args.Rest()) 
			{
				Double current = Convert.ToDouble(item);
				if (!(last <= current))
					return false;
				last = current;
			}
			return true;
		}

		/// <summary>
		/// (list object*)
		/// Creates a new cons, an ordered list with each object as a member. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object List(Cons args, Environment environment) 
		{
			return args;
		}

		/// <summary>
		/// (load filename) Loads and evaluates all statements in the given 
		/// filename which must be a text file. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Load(Cons args, Environment environment) 
		{
			object filename = args.First();

			if (filename is string)
			{
				TextReader textReader = new StreamReader((string)filename);

				string buffer = textReader.ReadToEnd();

				textReader.Close();

				// We want to evaluate the whole file, so there is an implicit do
				// which we now make explicit
				string expression = string.Format("(do {0}\n)",buffer);

				object input = Reader.Read(new StringReader(expression),ReadTable.DefaultReadTable());
				object output = Runtime.Eval(input, environment);

				return output;
			}
			throw new LSharpException(String.Format("Using: {0} is not a string", filename));
		}

		/// <summary>
		/// (& expression*)
		/// Performs a bitwise logical and operation on its arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object LogAnd(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			object result = args.First();
			foreach (Object item in (Cons)args.Rest()) 
			{

				// The integral types dont define operator overload methods
				// for performace reasons, so we have to implement this
				// operator on each integral type

				if (type == typeof(sbyte))
					result = (sbyte)result & (sbyte)(item);
				else if (type == typeof(byte))
						result = (byte)result & (byte)(item);
				else if (type == typeof(char))
					result = (char)result & (char)(item);
				else if (type == typeof(short))
					result = (short)result & (short)(item);
				else if (type == typeof(ushort))
					result = (ushort)result & (ushort)(item);
				else if (type == typeof(int))
					result = (int)result & (int)(item);
				else if (type == typeof(uint))
					result = (uint)result & (uint)(item);
				else if (type == typeof(long))
					result = (long)result & (long)(item);
				else if (type == typeof(ulong))
					result = (ulong)result & (ulong)(item);
				else 
					return Runtime.Call("op_BitwiseAnd",args);

			}

			return Convert.ChangeType(result,type);
		}


		/// <summary>
		/// (| expression*)
		/// Performs a bitwise logical inclusive or operation on its arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object LogIor(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			object result = args.First();
			foreach (Object item in (Cons)args.Rest()) 
			{

				// The integral types dont define operator overload methods
				// for performace reasons, so we have to implement this
				// operator on each integral type

				if (type == typeof(sbyte))
					result = (sbyte)result | (sbyte)(item);
				else if (type == typeof(byte))
					result = (byte)result | (byte)(item);
				else if (type == typeof(char))
					result = (char)result | (char)(item);
				else if (type == typeof(short))
					result = (short)result | (short)(item);
				else if (type == typeof(ushort))
					result = (ushort)result | (ushort)(item);
				else if (type == typeof(int))
					result = (int)result | (int)(item);
				else if (type == typeof(uint))
					result = (uint)result | (uint)(item);
				else if (type == typeof(long))
					result = (long)result | (long)(item);
				else if (type == typeof(ulong))
					result = (ulong)result | (ulong)(item);
				else 
					return Runtime.Call("op_BitwiseOr",args);

			}

			return Convert.ChangeType(result,type);
		}

		/// <summary>
		/// (^ expression*)
		/// Performs a bitwise logical exclusive or operation on its arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object LogXor(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			object result = args.First();
			foreach (Object item in (Cons)args.Rest()) 
			{

				// The integral types dont define operator overload methods
				// for performace reasons, so we have to implement this
				// operator on each integral type

				if (type == typeof(sbyte))
					result = (sbyte)result ^ (sbyte)(item);
				else if (type == typeof(byte))
					result = (byte)result ^ (byte)(item);
				else if (type == typeof(char))
					result = (char)result ^ (char)(item);
				else if (type == typeof(short))
					result = (short)result ^ (short)(item);
				else if (type == typeof(ushort))
					result = (ushort)result ^ (ushort)(item);
				else if (type == typeof(int))
					result = (int)result ^ (int)(item);
				else if (type == typeof(uint))
					result = (uint)result ^ (uint)(item);
				else if (type == typeof(long))
					result = (long)result ^ (long)(item);
				else if (type == typeof(ulong))
					result = (ulong)result ^ (ulong)(item);
				else 
					return Runtime.Call("op_ExclusiveOr",args);

			}

			return Convert.ChangeType(result,type);
		}




		public static Object MacroExpand(Cons args, Environment environment) 
		{
			Macro macro = (Macro)args.First();
			Cons arguments = (Cons)args.Rest();
			return macro.Expand(arguments);

		}

		/// <summary>
		/// (map function list) Maps function to each element in list return a new 
		/// list of return values. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Map(Cons args, Environment environment) 
		{
            if (args.Second() == null)
                return null;

			Cons temp = null;
			foreach (object o in (IEnumerable)args.Second()) 
			{
				temp = new Cons(
					Runtime.Apply( args.First(),new Cons(o),environment),
					temp);
			}
			return temp.Reverse();
		}

        /// <summary>
        /// (member item list)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object Member(Cons args, Environment environment)
        {
            object value = args.First();
            object list = args.Second();

            // TODO potential speed ups if list is IList or IDictionary

            foreach (object o in (IEnumerable)list)
            {
                if (Primitives.Eql(o,value))
                    return o;
            }

            return null;
        }


        public static Object Mod(Cons args, Environment environment)
        {
            Type type = args.First().GetType();
            Double result = Convert.ToDouble(args.First());
            foreach (object item in (Cons)args.Rest())
            {
                if (item is Double)
                    type = item.GetType();

                result %= Convert.ToDouble(item);
            }
            return Convert.ChangeType(result, type);
        }

		/// <summary>
		/// (* number*)
		/// Returns the result of multiplying all number arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Multiply(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			Double result = 1;
			foreach (Object item in args) 
			{
				if (item is Double)
					type = item.GetType();

				result *= Convert.ToDouble(item);
			}
			return Convert.ChangeType(result,type);
		}



        /// <summary>
        /// (nconc list*)
        /// Returns a list whose elements are the elements of each list in
        /// order. Destructively modifies all but the last list, such that
        /// the cdr of the last cons in each list is the next list. 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object Nconc(Cons args, Environment environment)
        {
            // With no argument, returns null
            if (args == null)
                return null;

            // With one argument, returns that argument
            if (args.Length() < 2)
                return args.First();

            for (int i = 0; i < args.Length() -1; i ++) 
            {
                Cons cons = (Cons)args.Nth(i);
                cons = (Cons)cons.Last();
                cons.Rplacd(args.Nth(i+1));
            }

            return args.First();
        }







		/// <summary>
		/// (new class) Creates a new object, an instance of type class 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object New(Cons args, Environment environment) 
		{
			Type type = TypeCache.Instance().FindType(args.First().ToString());

			return Runtime.MakeInstance(type,args.Rest());
		}

		/// <summary>
		/// (not bool) Returns the logical complement of the given boolean. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Not(Cons args, Environment environment) 
		{
			return !(Conversions.ObjectToBoolean (args.First()));
		}

		/// <summary>
		/// (!= expression*) The logical complement of the eql operator. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object NotEql(Cons args, Environment environment) 
		{
			return !Primitives.Eql(args);
		}

		/// <summary>
		/// (nth n list) Returns the (n+1)th element of list.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Nth(Cons args, Environment environment) 
		{
			int index = (int)args.First();
			object o = args.Second();
			if (o is IEnumerable) 
			{
				IEnumerator e = ((IEnumerable) o).GetEnumerator();
				for (int i = 0; i <= index; i ++ ) 
				{
					e.MoveNext();
				}
				return e.Current;
			}
			throw new LSharpException(string.Format("Nth: {0} is not IEnumerable",o));
		}

		/// <summary>
		/// (pr object*) Prints each object to Console.Out, without a new line. 
		/// This is really just a shorthand for (Write Console object) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Pr(Cons args, Environment environment) 
		{
			if (args == null)
				return null;
			else 
			{
                object last = null;
				foreach (object o in args) 
				{
                    last = o;
					if (o is System.String)
						Console.Write(o);
					else
						Console.Write(Printer.WriteToString(o));
				}
				return last;
			}
		}


		/// <summary>
		/// (prl object*) Prints each object to Console.Out, then prints 
		/// a new line. This is really just a shorthand for (WriteLine Console object) 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Prl(Cons args, Environment environment) 
		{
			if (args == null) 
			{
				Console.WriteLine();
				return null;
			}
			else 
			{
                object last = null;
				foreach (object o in args) 
				{
                    last = o;
					if (o is System.String)
						Console.Write(o);
					else
						Console.Write(Printer.WriteToString(o));
				}
				Console.WriteLine();
				return last;
			}
		}

		/// <summary>
		/// (read TextReader [eof])
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Read(Cons args, Environment environment) 
		{
			ReadTable readTable = (ReadTable)environment.GetValue(Symbol.FromName("*readtable*"));

			TextReader textReader = (TextReader)args.First();
			object eofValue = null;
			if (args.Length() > 1)
				eofValue = args.Second();
			
			return Reader.Read(textReader, readTable, eofValue);		
		}

		/// <summary>
		/// (reference name*) Loads the specified .NET assembly, either from the GAC, 
		/// local directory or from and explicit directory. 
		/// name may either be a fully qualified filename or a partial assembly name. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Reference(Cons args, Environment environment) 
		{
			object result = null;
			foreach (object module in args) 
			{
				if (module is string)
					result = AssemblyCache.Instance().LoadAssembly((string)module);
				else
					throw new LSharpException(String.Format("Reference: {0} is not a string", module));
			}
			return result;
		}

		/// <summary>
		/// (reset) Resets the global environment.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Reset(Cons args, Environment environment) 
		{
			environment.GlobalReset();
			TypeCache.Instance().Clear();
			return null;
		}

		/// <summary>
		/// (- number*) Subtraction
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Subtract(Cons args, Environment environment) 
		{
			Type type = args.First().GetType();
			Double result = Convert.ToDouble(args.First());
			foreach (object item in (Cons)args.Rest()) 
			{
				if (item is Double)
					type = item.GetType();

				result -= Convert.ToDouble(item);
			}
			return Convert.ChangeType(result,type);
		}

        /// <summary>
        /// (symbol-name symbol) Returns the name of a symbol as a string
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object SymbolName(Cons args, Environment environment)
        {
            return ((Symbol)args.First()).Name;
        }

		/// <summary>
		/// (throw exception) Throws a System.Exception
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Throw(Cons args, Environment environment) 
		{
			throw((Exception)args.First());
		}



		/// <summary>
		/// (typeof symbol) Returns the type object of the same name as the given symbol.
		/// </summary>
		/// <example>(typeof Console)</example>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object TypeOf(Cons args, Environment environment) 
		{
			Type type = TypeCache.Instance().FindType(args.First().ToString());

			return (type);
		}

		/// <summary>
		/// Permit the use of types in a namespace, such that, you do not have to 
		/// qualify the use of a type in that namespace
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Using(Cons args, Environment environment) 
		{
			string result = null;
			foreach (object name in args) 
			{
				if (name is String) 
				{
					TypeCache.Instance().Using((string)name);
					result = (string) name;
				} 
				else 
				{
					throw new LSharpException(String.Format("Using: {0} is not a string",name));
				}
			}

			return result;			
		}
	}
}
