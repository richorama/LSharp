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
using System.Text;

namespace LSharp
{
	/// <summary>
	/// Special forms take their arguents un-evaluated so that
	/// they can themselves decide upon the evaluation process.
	/// </summary>
	public class SpecialForms
	{

		/// <summary>
		/// Returns true if all arguments are true, false otherwise.
		/// Performs short circuit evaluation on its arguments.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object And(Cons args, Environment environment) 
		{
			foreach (Object item in args) 
			{
				if (Conversions.ObjectToBoolean(Runtime.Eval(item,environment)) == false)
					return false;
			}
			return true;
		}

		public static Object BackQuote(Cons args, Environment environment) 
		{
			return Runtime.BackQuoteExpand(args.First(), environment);
		}

		/// <summary>
		/// (call method object argument*)
		/// Calls a .NET method on a given object with given arguments. 
		/// This is useful if the method name clashes with a variable which is already 
		/// bound in the current L Sharp lexical environment. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Call(Cons args, Environment environment) 
		{
			return Runtime.Call(args.Car().ToString(),
				(Cons)Runtime.EvalList(args.Rest(),environment));
		}

		/// <summary>
		/// (compile filename expression*)
		/// Compiles the given stament to IL, storing the result in a new EXE packaged 
		/// assembly filename. (Actually this dosnt work yet). 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Compile(Cons args, Environment environment) 
		{
			Compiler.Compile(args.First().ToString(),args.Second());
			return null;
		}

		/// <summary>
		/// (cond (test expression)* [default])
		/// Evaluates tests until one returns true. If a test returns true, 
		/// then evaluates its corresponding expression. If no test return true, 
		/// then and optionally specified default expression is evaluated. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Cond(Cons args, Environment environment) 
		{
			Cons clauses = args;

			while (clauses.Length() > 0)
			{
				if (clauses.Length() == 1) 
				{
					// This is a default (else) clause, so just execute it
					return Runtime.Eval(clauses.First(),environment);
				}

				if (clauses.Length() >= 2) 
				{
					if (Conversions.ObjectToBoolean(Runtime.Eval(clauses.First(),environment)))
						return Runtime.Eval(clauses.Second(),environment);
					else
						clauses = (Cons)clauses.Cddr();
				}
			}	
			return null;
		}

		/// <summary>
		/// (-- symbol)
		/// Subtracts one from the variable represented by symbol. 
		/// A shorthand for (= symbol (- symbol 1))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Decrement(Cons args, Environment environment) 
		{
			object o = Runtime.Eval(args.First(), environment);
			object result;
			if (o is Double)
				result = (Double)o - 1;
			else
				result = (Int32)o - 1;
			return environment.Assign((Symbol) args.First(), result);
		}

		public static Object DefClass(Cons args, Environment environment) 
		{
			string className = args.First().ToString();

			Cons superClasses = args.Cadr() as Cons;
			string superClass = null;
			string interfaces = null;

			if (superClasses != null) 
			{
				superClass = superClasses.First().ToString();

			
				if (superClasses.Length() >= 2) 
				{
					StringBuilder b = new StringBuilder();
					b.Append(superClasses.Second());

					foreach (object item in (Cons)superClasses.Cddr()) 
					{
						b.Append(", " +item);
					}
					interfaces = b.ToString();
				}
			}

			return ClassBuilder.CreateClass(className, superClass, interfaces);
		}


		/// <summary>
		/// (do expression*)
		/// Evaluates each expression, one at a time. 
		/// Returns the result of the evaluation of the last expression. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Do(Cons args, Environment environment) 
		{
			object result = null;
			foreach (object item in args) 
			{
				result = Runtime.Eval(item, environment);
			}
			return result;
		}

		/// <summary>
		/// (fn arguments body)
		/// Defines a closure with the specified list of arguments and the specified body, 
		/// an L Sharp expression. Not unlike a lambda expression in Common Lisp.NB arguments 
		/// is a simple list, we dont yet support keyword or optional arguments. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Fn(Cons args, Environment environment) 
		{
			return new Closure((Cons)args.First(),(Cons)args.Cdr(),environment);
		}

		/// <summary>
		/// (for initialiser test iterator statement)
		/// The for special form corresponds to the for construct found in most algebraic 
		/// programming languages. The initialiser is executed. The statement is executed 
		/// while test is true. The iterator is executed at the end of each statement execution. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object For(Cons args, Environment environment) 
		{
			Environment localEnvironment = new Environment(environment);
			Runtime.Eval(args.First(),localEnvironment);
			object test;
			while ((Conversions.ObjectToBoolean(test = Runtime.Eval(args.Second(),localEnvironment)))) 
			{
				foreach (object item in (Cons)args.Cdddr()) 
				{
					Runtime.Eval(item, localEnvironment);
				}
				Runtime.Eval(args.Third(),localEnvironment);
			}
			return test;
		}

		/// <summary>
		/// (each symbol IEnumerable expression)
		/// Iterates over any object which impelements IEnumerablewith succesive 
		/// elements being assigned to a variable named symbol; exceutes expression 
		/// on each iteration. Cons (LSharp lists), as well as many .NET collections 
		/// are IEnumerable. Foreach is a synonym for each. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object ForEach(Cons args, Environment environment) 
		{
			Environment localEnvironment = new Environment(environment);

			Symbol variable = (Symbol) args.First();
			Object list = Runtime.Eval(args.Second(),localEnvironment);

			foreach (object o in (System.Collections.IEnumerable)list) 
			{
				localEnvironment.AssignLocal(variable, o);
				//Runtime.Eval(args.Third(),localEnvironment);
				foreach (object item in (Cons)args.Cddr()) 
				{
					Runtime.Eval(item, localEnvironment);
				}
			}

			return null;
		}


		/// <summary>
		/// (if test then [else])
		/// The if special form corresponds to the if-then-else construct found in 
		/// most algebraic programming languages. First the form test is evauated, 
		/// if true then the form then is evaluated.Otherwise, optionally the form 
		/// else is evaluated. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object If(Cons args, Environment environment) 
		{
			if (Conversions.ObjectToBoolean(Runtime.Eval(args.First(),environment)))
				// Evaluate the then part
				return Runtime.Eval(args.Second(),environment);
			else
				if (args.Length() > 2)
				// Evaluate the optional else part
				return Runtime.Eval(args.Third(),environment);
			else
				return null;
		}

		/// <summary>
		/// (++ symbol)
		/// Adds one to the variable represented by symbol. A shorthand for 
		/// (= symbol (+ symbol 1))
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Increment(Cons args, Environment environment) 
		{
			object o = Runtime.Eval(args.First(), environment);
			object result;
			if (o is Double)
				result = (Double)o + 1;
			else
				result = (Int32)o + 1;
			return environment.Assign((Symbol) args.First(), result);
		}

		/// <summary>
		/// (let symbol value expression*)
		/// Binds a new local variable symbol to value in a new local lexical environment, 
		/// before evaluating expressions. Similar to with, but often more convenient for 
		/// decalring a single local variable. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Let(Cons args, Environment environment) 
		{
			Environment localEnvironment = new Environment(environment);
			localEnvironment.AssignLocal((Symbol) args.First(), Runtime.Eval(args.Second(),environment));
			
			object result = null;
			foreach (object item in (Cons)args.Cddr()) 
			{
				result = Runtime.Eval(item, localEnvironment);
			}
			//return Runtime.Eval(args.Third(),localEnvironment);
			return result;
		}

		/// <summary>
		/// (macro arguments expression*)
		/// Defines a macro. Similar in some respects to defining a clsoure using fn 
		/// except that the expressions are expanded using macroexpand in the current 
		/// environment before being evaluated. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Macro(Cons args, Environment environment) 
		{
			return new Macro((Cons)args.First(),(Cons)args.Cdr(),environment);
		}

		/// <summary>
		/// (or expression*)
		/// Evaluates the expressions in order, returning true if any expression is true, 
		/// and false if all expressions are false. If a true expression is encountered, 
		/// then the remaining expressions are not evaluated (short circuit evaluation). 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Or(Cons args, Environment environment) 
		{
			foreach (Object item in args)
			{
				if (Conversions.ObjectToBoolean(Runtime.Eval(item,environment)) == true)
					return true;
			}
			return false;
		}

		/// <summary>
		/// (quote object)
		/// Returns object without evaluating it. object may be any .NET object. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Quote(Cons args, Environment environment) 
		{
			return args.First();
		}

		/// <summary>
		/// (= { symbol value}*)
		/// Setq (Set Quote) is the variable assignment operator. 
		/// Sets each variable symbol to value in the current environment. 
		/// The abbreviation = is more commonly used in L Sharp.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Setq(Cons args, Environment environment) 
		{
			object v = null;
			while (args != null) 
			{
				Symbol s = (Symbol)args.First();
				v = Runtime.Eval(args.Second(),environment);
				environment.Assign(s,v);
				args = (Cons)args.Cddr();
			}

			return v;

		}

        /// <summary>
        /// (spawn expression) is like eval except that the expression
        /// is evaluated on a new thread. Returns immediately with
        /// the new thread object, but execution of expressions
        /// continues synchronously. Experimental.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Object Spawn(Cons args, Environment environment)
        {
            if (args.Length() == 1)
                return ThreadAdapter.Fork(args.First(), environment);
            else
                throw new LSharpException("Incorrect arguments given to spawn");
        }

		/// <summary>
		/// (the type value)
		/// Returns value converted or cast to an object of the specified type. 
		/// Throws an exception is the cast is not achievable. 
		/// The allows type casting and type conversion. This is much more than 
		/// a wrapper for the System.Convert class, it has special meaning for 
		/// conversions to and from Lists and certain common .NET data structures.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object The(Cons args, Environment environment) 
		{
			Type o = TypeCache.Instance().FindType(args.First().ToString());
			return Conversions.The (o, Runtime.Eval(args.Second(),environment));
		}

		/// <summary>
		/// (to variable limit expression)
		/// Starting at 0, assigns variable to succesive integers upto and 
		/// including limit. Executes expression on each iteration. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object To(Cons args, Environment environment) 
		{
			Environment localEnvironment = new Environment(environment);
			localEnvironment.AssignLocal((Symbol) args.First(), 0);
			int endStop = int.Parse(Runtime.Eval(args.Second(),localEnvironment).ToString());
			while ((int)localEnvironment.GetValue((Symbol) args.First()) < endStop)
			{
				foreach (object item in (Cons)args.Cddr()) 
				{
					Runtime.Eval(item, localEnvironment);
				}
				localEnvironment.AssignLocal((Symbol) args.First(), ((int)Runtime.Eval(args.First(), localEnvironment)) + 1);
				
			}
			return null;
		}

		/// <summary>
		/// (trace filename expression*) 
		/// Traces an evaluation of expression* (as if in an implicit do), 
		/// documenting all call and return steps; writes the output as an 
		/// XML file in filename. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Trace(Cons args, Environment environment) 
		{
			string filename = (String)Runtime.Eval(args.First(),environment);
			
			try 
			{
				Runtime.Profiler = new XmlTracer(filename);

				object result = null;;

				foreach (object item in (Cons)args.Rest()) 
				{
					result = Runtime.Eval(item, environment);
				}

				return result;
			}
			catch (Exception e) 
			{
				throw;
			}
			finally
			{
				Runtime.Profiler.Close();
				Runtime.Profiler = new DefaultProfiler();
			}
		
		}

		/// <summary>
		/// (try expression catch [finally])
		/// The try special form corresponds to the try-catch-finally construct found 
		/// in C Sharp. If   catch is null then there is deemed to be no catch block 
		/// at all. If an exception occurs, the variable it is bound to the Exception 
		/// object in the local environment. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object Try(Cons args, Environment environment) 
		{
			try 
			{
				return Runtime.Eval(args.First(),environment);
			} 
			catch (Exception e) 
			{
				environment.AssignLocal(Symbol.IT,e);
						
				// If a catch form is specified then evaluate it
				if (args.Second() == Symbol.NULL)
					throw;
				return Runtime.Eval(args.Second(),environment);
			}
			finally 
			{
				// If a finally form was specified then evaluate it
				if  (args.Length() > 2)
					Runtime.Eval(args.Third(),environment);
			}
		}
		

		/// <summary>
		/// (when test expression*) Similar to if, but more convenient if there 
		/// is no else case. Evalautes expressions if the evaluation of test 
		/// is true. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object When(Cons args, Environment environment) 
		{
			object test = null;
			if ((Conversions.ObjectToBoolean(Runtime.Eval(args.First(),environment)))) 
			{
				foreach (object item in (Cons)args.Rest()) 
				{
					test = Runtime.Eval(item, environment);
				}
			}
			return test;
		}

		/// <summary>
		/// (while test expression*) 
		/// The while special form corresponds to the while construct found 
		/// in most algebraic programming languages. First test is evauated, 
		/// if true then expression* is evaluated. The process continues until 
		/// the evaluation of test is false. 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object While(Cons args, Environment environment) 
		{
			object test;
			while ((Conversions.ObjectToBoolean(test = Runtime.Eval(args.First(),environment)))) 
			{
				foreach (object item in (Cons)args.Rest()) 
				{
					Runtime.Eval(item, environment);
				}
			}
			return test;
		}

		/// <summary>
		/// (with ((symbol value)* ) expression*) 
		/// Binds new local variables symbols to values in a new local 
		/// lexical environment, before evaluating expressions. Similar to 
		/// let, but allows multiple local variables to be bound.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static Object With(Cons args, Environment environment) 
		{
			Environment localEnvironment = new Environment(environment);

			Cons bindings = (Cons)args.First();

			while ((bindings != null) && (bindings.Length() > 1))
			{
				localEnvironment.AssignLocal((Symbol) bindings.First(), Runtime.Eval(bindings.Second(),environment));
				bindings = (Cons)bindings.Cddr();
			}	
			
			object result = null;
			foreach (object item in (Cons)args.Cdr()) 
			{
				result = Runtime.Eval(item, localEnvironment);
			}
			return result;
		}	
	}
}
