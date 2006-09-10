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
using System.Text;

namespace LSharp
{
	/// <summary>
	/// An environment is essentially a hashtable that maps variables to values.
	/// In addition, environments can be nested to support local variables.
	/// </summary>
	public class Environment
	{
		private const int CAPACITY = 10;
		private Hashtable hashtable = new Hashtable(CAPACITY);

		// Maintain a reference to a previous environment to allow nesting
		// of environments, thus supporting local variables and recursion
		private Environment previousEnvironment;

		public void GlobalReset() 
		{
			hashtable = new Hashtable(CAPACITY);
			InitialiseLSharpBindings();
		}

		public void InitialiseLSharpBindings() 
		{
			// Define the L Sharp Special Forms
			this.AssignLocal(Symbol.FromName("and"),new SpecialForm(SpecialForms.And));
			this.AssignLocal(Symbol.BACKQUOTE,new SpecialForm(SpecialForms.BackQuote));
			this.AssignLocal(Symbol.FromName("call"),new SpecialForm(SpecialForms.Call));
			this.AssignLocal(Symbol.FromName("compile"),new Function(SpecialForms.Compile));
			this.AssignLocal(Symbol.FromName("cond"),new SpecialForm(SpecialForms.Cond));
			this.AssignLocal(Symbol.FromName("--"),new SpecialForm(SpecialForms.Decrement));
			this.AssignLocal(Symbol.FromName("defclass"),new SpecialForm(SpecialForms.DefClass));
			this.AssignLocal(Symbol.FromName("do"),new SpecialForm(SpecialForms.Do));
			this.AssignLocal(Symbol.FromName("each"),new SpecialForm(SpecialForms.ForEach));
			this.AssignLocal(Symbol.FromName("fn"),new SpecialForm(SpecialForms.Fn));
			this.AssignLocal(Symbol.FromName("for"),new SpecialForm(SpecialForms.For));
			this.AssignLocal(Symbol.FromName("foreach"),new SpecialForm(SpecialForms.ForEach));
			this.AssignLocal(Symbol.FromName("if"),new SpecialForm(SpecialForms.If));
			this.AssignLocal(Symbol.FromName("++"),new SpecialForm(SpecialForms.Increment));
			this.AssignLocal(Symbol.FromName("let"),new SpecialForm(SpecialForms.Let));
			this.AssignLocal(Symbol.FromName("macro"),new SpecialForm(SpecialForms.Macro));
			this.AssignLocal(Symbol.FromName("or"),new SpecialForm(SpecialForms.Or));
			this.AssignLocal(Symbol.FromName("quote"),new SpecialForm(SpecialForms.Quote));
			this.AssignLocal(Symbol.FromName("'"),new SpecialForm(SpecialForms.Quote));
			this.AssignLocal(Symbol.FromName("setf"),new SpecialForm(SpecialForms.Setq));
            this.AssignLocal(Symbol.FromName("setq"), new SpecialForm(SpecialForms.Setq));
            this.AssignLocal(Symbol.FromName("spawn"), new SpecialForm(SpecialForms.Spawn));
			this.AssignLocal(Symbol.FromName("="),new SpecialForm(SpecialForms.Setq));
			this.AssignLocal(Symbol.FromName("the"),new SpecialForm(SpecialForms.The));
			this.AssignLocal(Symbol.FromName("to"),new SpecialForm(SpecialForms.To));
			this.AssignLocal(Symbol.FromName("trace"),new SpecialForm(SpecialForms.Trace));
			this.AssignLocal(Symbol.FromName("try"),new SpecialForm(SpecialForms.Try));
			this.AssignLocal(Symbol.FromName("when"),new SpecialForm(SpecialForms.When));
			this.AssignLocal(Symbol.FromName("while"),new SpecialForm(SpecialForms.While));
			this.AssignLocal(Symbol.FromName("with"),new SpecialForm(SpecialForms.With));
			
			// Define the L Sharp Built in Functions
			this.AssignLocal(Symbol.FromName("+"),new Function(Functions.Add));
            this.AssignLocal(Symbol.FromName("append"), new Function(Functions.Append));
			this.AssignLocal(Symbol.FromName("apply"),new Function(Functions.Apply));
			this.AssignLocal(Symbol.FromName("assoc"),new Function(Functions.Assoc));
			this.AssignLocal(Symbol.FromName("caaar"),new Function(Functions.Caaar));
			this.AssignLocal(Symbol.FromName("caadr"),new Function(Functions.Caadr));
			this.AssignLocal(Symbol.FromName("caar"),new Function(Functions.Caar));
			this.AssignLocal(Symbol.FromName("cadar"),new Function(Functions.Cadar));
			this.AssignLocal(Symbol.FromName("caddr"),new Function(Functions.Caddr));
			this.AssignLocal(Symbol.FromName("cadr"),new Function(Functions.Cadr));
			this.AssignLocal(Symbol.FromName("car"),new Function(Functions.Car));
			this.AssignLocal(Symbol.FromName("cdaar"),new Function(Functions.Cdaar));
			this.AssignLocal(Symbol.FromName("cdar"),new Function(Functions.Cdar));
			this.AssignLocal(Symbol.FromName("cddar"),new Function(Functions.Cddar));
			this.AssignLocal(Symbol.FromName("cdadr"),new Function(Functions.Cdadr));
			this.AssignLocal(Symbol.FromName("cdddr"),new Function(Functions.Cdddr));
			this.AssignLocal(Symbol.FromName("cddr"),new Function(Functions.Cddr));
			this.AssignLocal(Symbol.FromName("cdr"),new Function(Functions.Cdr));
            this.AssignLocal(Symbol.FromName("cons"),new Function(Functions.Cons));
            this.AssignLocal(Symbol.FromName("copy-list"), new Function(Functions.CopyList));
            this.AssignLocal(Symbol.FromName("/"),new Function(Functions.Divide));
            this.AssignLocal(Symbol.FromName("%"), new Function(Functions.Mod));
			this.AssignLocal(Symbol.FromName("environment"),new Function(Functions.Environment));
			this.AssignLocal(Symbol.FromName("eq"),new Function(Functions.Eq));
			this.AssignLocal(Symbol.FromName("eql"),new Function(Functions.Eql));
			this.AssignLocal(Symbol.FromName("=="),new Function(Functions.Eql));
			this.AssignLocal(Symbol.FromName("eval"),new Function(Functions.Eval));
			this.AssignLocal(Symbol.FromName("evalstring"),new Function(Functions.EvalString));
			this.AssignLocal(Symbol.FromName("exit"),new Function(Functions.Exit));
			this.AssignLocal(Symbol.FromName("first"),new Function(Functions.Car));
            this.AssignLocal(Symbol.FromName("gensym"), new Function(Functions.Gensym));
			this.AssignLocal(Symbol.FromName(">"),new Function(Functions.GreaterThan));
			this.AssignLocal(Symbol.FromName(">="),new Function(Functions.GreaterThanEqual));
            this.AssignLocal(Symbol.FromName("handle-event"), new Function(Functions.HandleEvent));
			this.AssignLocal(Symbol.FromName("inspect"),new Function(Functions.Inspect));
			this.AssignLocal(Symbol.FromName("is"),new Function(Functions.Is));
			this.AssignLocal(Symbol.FromName("length"),new Function(Functions.Length));
			this.AssignLocal(Symbol.FromName("<"),new Function(Functions.LessThan));
			this.AssignLocal(Symbol.FromName("<="),new Function(Functions.LessThanEqual));
			this.AssignLocal(Symbol.FromName("list"),new Function(Functions.List));
			this.AssignLocal(Symbol.FromName("load"),new Function(Functions.Load));
			this.AssignLocal(Symbol.FromName("&"),new Function(Functions.LogAnd));
			this.AssignLocal(Symbol.FromName("|"),new Function(Functions.LogIor));
			this.AssignLocal(Symbol.FromName("^"),new Function(Functions.LogXor));
			this.AssignLocal(Symbol.FromName("macroexpand"),new Function(Functions.MacroExpand));
			this.AssignLocal(Symbol.FromName("map"),new Function(Functions.Map));
            this.AssignLocal(Symbol.FromName("member"), new Function(Functions.Member));
            this.AssignLocal(Symbol.FromName("*"),new Function(Functions.Multiply));
            this.AssignLocal(Symbol.FromName("nconc"), new Function(Functions.Nconc));
			this.AssignLocal(Symbol.FromName("new"),new Function(Functions.New));
			this.AssignLocal(Symbol.FromName("not"),new Function(Functions.Not));
			this.AssignLocal(Symbol.FromName("!="),new Function(Functions.NotEql));
			this.AssignLocal(Symbol.FromName("nth"),new Function(Functions.Nth));
			this.AssignLocal(Symbol.FromName("pr"),new Function(Functions.Pr));
			this.AssignLocal(Symbol.FromName("prl"),new Function(Functions.Prl));
			this.AssignLocal(Symbol.FromName("read"),new Function(Functions.Read));
			this.AssignLocal(Symbol.FromName("reference"),new Function(Functions.Reference));
			this.AssignLocal(Symbol.FromName("reset"),new Function(Functions.Reset));
			this.AssignLocal(Symbol.FromName("rest"),new Function(Functions.Cdr));
			this.AssignLocal(Symbol.FromName("second"),new Function(Functions.Cadr));
            
			this.AssignLocal(Symbol.FromName("-"),new Function(Functions.Subtract));
            this.AssignLocal(Symbol.FromName("symbol-name"), new Function(Functions.SymbolName));
			this.AssignLocal(Symbol.FromName("third"),new Function(Functions.Caddr));
			this.AssignLocal(Symbol.FromName("throw"),new Function(Functions.Throw));
            this.AssignLocal(Symbol.FromName("typeof"),new Function(Functions.TypeOf));
			this.AssignLocal(Symbol.FromName("using"),new Function(Functions.Using));
			
			// read table
			this.AssignLocal(Symbol.FromName("*readtable*"),ReadTable.DefaultReadTable());

			// Macros
            this.AssignLocal(Symbol.FromName("defevent"), Macros.DefEvent(this));
			this.AssignLocal(Symbol.FromName("defun"),Macros.Defun(this));
			this.AssignLocal(Symbol.FromName("defmacro"),Macros.DefMacro(this));
			this.AssignLocal(Symbol.FromName("listp"),Macros.ListP(this));
			
			
		}

		/// <summary>
		/// Creates a new, global environment
		/// </summary>
		public Environment() 
		{
			InitialiseLSharpBindings();
		}

		/// <summary>
		/// Creates a new environment which has access to a previous environment
		/// </summary>
		/// <param name="environment"></param>
		public Environment(Environment environment) 
		{
			this.previousEnvironment = environment;
		}

		public object GetValue(Symbol symbol) 
		{
			object o = hashtable[symbol];

			if ((o == null) && (previousEnvironment != null))
				o = previousEnvironment.GetValue(symbol);

			return o;
		}

		
		/// <summary>
		/// Determines whether the environment contains a definition for
		/// a variable with the given symbol
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns>True or false</returns>
		public bool Contains(Symbol symbol) 
		{
			if (hashtable.Contains(symbol))
				return true;

			if (previousEnvironment != null)
				return previousEnvironment.Contains(symbol);

			return false;

		}
		
		private Environment GetEnvironment (Symbol symbol) 
		{
			if (hashtable.Contains(symbol))
				return this;

			if (previousEnvironment == null)
				return null;
			
			return previousEnvironment.GetEnvironment(symbol);

		}

		/// <summary>
		/// Sets a variable with given symbol to a given value
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public object Assign(Symbol symbol, object value) 
		{
			Environment environment = GetEnvironment(symbol);

			if (environment == null)
				environment = this;

			return environment.AssignLocal(symbol, value);
		}

		/// <summary>
		/// Asssigns value to a local variable symbol in this
		/// local environment (irrespective of whether symbol
		/// is defined in any parent environments).
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public object AssignLocal(Symbol symbol, object value) 
		{
			hashtable[symbol] = value;
			return value;
		}

		/// <summary>
		/// Returns the contents of the environment as a string suitable for use
		/// in a debugger or IDE.
		/// </summary>
		/// <returns></returns>
		public string Contents() 
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach(string key in hashtable.Keys) 
			{
				stringBuilder.AppendFormat("{0}:{1}\r\n",key.ToString(),hashtable[key]);
			}
			return stringBuilder.ToString();
		}
	}
}
