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

		/// <summary>
        /// Process the arguments passed to a closure, and add them to the given enviroment.
		/// </summary>
        /// <param name="argumentNameList">The list of names and kewords the closure was created with.</param>
        /// <param name="argumentList">The arguments passed to the closure.</param>
        /// <param name="localEnvironment">The closure's local variables.</param>
		/// <returns>Nothing.</returns>
        public static void ProcessArguments(Cons argumentNameList, Cons argumentList, Environment localEnvironment) 
		{
			while (argumentNameList != null) 
			{
                // Get the name for the closure's parameter.  Then check to see if it's a keyword, if it is then
                // process the keyword.  Otherwise set up that parameter in the closure's enviroment with the
                // caller specified value.

				Symbol argumentName = (Symbol)argumentNameList.Car();

                switch (argumentName.ToString())
                {
                    case "&rest":
                        argumentName = (Symbol)argumentNameList.Cadr();
                        localEnvironment.AssignLocal(argumentName, argumentList);
                        argumentNameList = null;
                        argumentList = null;
                        break;

                    case "&optional":
                        ProcessOptionalArguments((Cons)argumentNameList.Cdr(), argumentList, localEnvironment);
                        argumentNameList = null;
                        argumentList = null;
                        break;

                    case "&key":
                        ProcessKeyArguments((Cons)argumentNameList.Cdr(), argumentList, localEnvironment);
                        argumentNameList = null;
                        argumentList = null;
                        break;

                    default:
                        if (argumentList == null)
                        {
                            throw new LSharpException("Not enough parameters given.");
                        }

                        localEnvironment.AssignLocal(argumentName, argumentList.Car());
                        argumentList = (Cons)argumentList.Cdr();
                        argumentNameList = (Cons)argumentNameList.Cdr();
                        break;
                }
			}


            // Looks like the caller has supplied more parameters than the closure can use.

            if (argumentList != null)
            {
                throw new LSharpException("Too many parameters given.");
            }
		}



        private static void ProcessOptionalArguments(Cons argumentNameList, Cons argumentList,
                                                     Environment localEnvironment)
        {
            // We need to add all the arguments to the closure's environment.

            while (argumentNameList != null)
            {
                Symbol argumentName = null;
                object argumentValue = null;


                // We need to get the name of the argument, it can either be just the name or, it can be
                // it's own Cons with the name and an expression for the default value.

                if (argumentNameList.Car().GetType() == typeof(Cons))
                {
                    // It is a Cons, so extract the name and the default value.

                    argumentName = (Symbol)argumentNameList.Caar();
                    argumentValue = argumentNameList.Cadar();
                }
                else
                {
                    argumentName = (Symbol)argumentNameList.Car();
                }


                // Now, if the caller has specified a value for this argument, get it now.

                if (argumentList != null)
                {
                    argumentValue = argumentList.Car();
                    argumentList = (Cons)argumentList.Cdr();
                }


                // Finally add the parameter to the closure's list and then move onto the next argument.
                // Because the default can be any expression, we need to evaluate the value every time the
                // function is called.

                localEnvironment.AssignLocal(argumentName, Runtime.Eval(argumentValue, localEnvironment));

                argumentNameList = (Cons)argumentNameList.Cdr();
            }


            // Looks like the caller has supplied more parameters than the closure can use.

            if (argumentList != null)
            {
                throw new LSharpException("Too many parameters given.");
            }
        }


        private static void ProcessKeyArguments(Cons argumentNameList, Cons argumentList,
                                                Environment localEnvironment)
        {
            // Make sure that all of the defined key arguments are inserted to the local enviroment with their
            // defaults.

            while (argumentNameList != null)
            {
                Symbol argumentName = null;
                object argumentValue = null;


                // We need to get the name of the argument, it can either be just the name or, it can be
                // it's own Cons with the name and an expression for the default value.

                if (argumentNameList.Car().GetType() == typeof(Cons))
                {
                    // It is a Cons, so extract the name and the default value.  Because the default can be
                    // any expression, we need to evaluate the value every time the function is called.

                    argumentName = (Symbol)argumentNameList.Caar();
                    argumentValue = Runtime.Eval(argumentNameList.Cadar(), localEnvironment);
                }
                else
                {
                    argumentName = (Symbol)argumentNameList.Car();
                }


                // Add this variable to the closure's environment, then advance to the next parameter.

                localEnvironment.AssignLocal(argumentName, argumentValue);

                argumentNameList = (Cons)argumentNameList.Cdr();
            }


            // Now that the parameters and their defaults have been added to the environment we can now
            // process the supplied arguments.

            while (argumentList != null)
            {
                // Because these are keyed parameters, the caller needs to specify the name of each
                // parameter.

                if (argumentList.Car().GetType() != typeof(Symbol))
                {
                    throw new LSharpException("Key parameters must be specified by name.");
                }


                // Grab the current parameter and the value associated with it.  Then make sure that this
                // is a keyword.

                Symbol keywordName = (Symbol)argumentList.Car();
                object argumentValue = argumentList.Cadr();

                if (keywordName.Name[0] != ':')
                {
                    throw new LSharpException(keywordName + " is not a valid keyword.");
                }


                // Now that we know they supplied a keyword, create a symbol out of it and make sure that
                // it exists.

                //keywordName = new Symbol(keywordName.Name.Substring(1));
                keywordName = Symbol.FromName(keywordName.Name.Substring(1));

                if (localEnvironment.Contains(keywordName) == false)
                {
                    throw new LSharpException(keywordName + " is not a recognised keyword.");
                }


                // Update the parameter with the value that the user specified and then move onto the next
                // argument in the list.

                localEnvironment.AssignLocal(keywordName, argumentValue);
                argumentList = (Cons)argumentList.Cddr();
            }

        }
	}
}
