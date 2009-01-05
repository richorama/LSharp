#region Copyright (c) 2008, Rob Blackwell.  All rights reserved.
// Software License Agreement (BSD License)

// Copyright (c) 2008, Rob Blackwell.  All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:

//   * Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.

//   * Redistributions in binary form must reproduce the above
//     copyright notice, this list of conditions and the following
//     disclaimer in the documentation and/or other materials
//     provided with the distribution.

// THIS SOFTWARE IS PROVIDED BY THE AUTHOR 'AS IS' AND ANY EXPRESSED
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;

namespace LSharp
{
    /// <summary>
    /// The LSharp Runtime
    /// </summary>
    public class Runtime
    {
        private Environment globalEnvironment = null;
        private static List<string> namespaces = new List<string>() { "", "System" };

        // Default streams
        private TextWriter stdout;
        private TextReader stdin;
        private TextWriter stderr;

        // Define some commonly used symbols
        public static Symbol T = Symbol.FromName("t");
        public static Symbol NIL = Symbol.FromName("nil");
        public static Symbol UNQUOTE_SPLICING = Symbol.FromName("unquote-splicing");
        public static Symbol UNQUOTE = Symbol.FromName("unquote");
        public static Symbol QUASIQUOTE = Symbol.FromName("quasiquote");
        public static Symbol PLUS = Symbol.FromName("+");
        public static Symbol PLUSPLUS = Symbol.FromName("++");
        public static Symbol EXIT = Symbol.FromName("exit");

        /// <summary>
        /// Creates a new runtime
        /// </summary>
        public Runtime(TextReader textReader, TextWriter writer, TextWriter errorWriter)
        {
            // Initialise the input, output and error streams
            this.stdin = textReader;
            this.stdout = writer;
            this.stderr = errorWriter;

            // Install a new global environemt
            globalEnvironment = new LSharp.Environment();

            // Set up a default definition for the language ..

            // LISP true and false
            globalEnvironment.AssignLocal(T, true);
            globalEnvironment.AssignLocal(NIL, null);

            // .NET true and false are booleans
            globalEnvironment.AssignLocal(Symbol.FromName("true"), true);
            globalEnvironment.AssignLocal(Symbol.FromName("false"), false);

            // Null
            globalEnvironment.AssignLocal(Symbol.FromName("null"), null);

            // Wire up the functions that are defined in C# ..

            globalEnvironment.AssignLocal(Symbol.FromName("+"),
                new Function(new Func<IEnumerable, object>(Runtime.Add),
                    "& xs",
                    "If the first x is a number, returns the sum of xs, otherwise the concatenation of all xs. (+) returns 0.",
                    true));

            globalEnvironment.AssignLocal(Symbol.FromName("-"),
                new Function(new Func<IEnumerable, object>(Runtime.Subtract),
                    "& xs",
                    "Subtraction.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("/"),
                new Function(new Func<object[], object>(Runtime.Divide),
                    "& xs",
                    "Division.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("*"),
                new Function(new Func<object[], object>(Runtime.Multiply),
                    "& xs",
                    "Returns the product of the xs. (*) returns 1.", true));

            globalEnvironment.AssignLocal(Symbol.FromName(">"),
                new Function(new Func<object[], object>(Runtime.GreaterThan),
                    "& xs",
                    "Greater than.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("<"),
                new Function(new Func<object[], object>(Runtime.LessThan),
                    "& xs",
                    "Less than.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("atom?"),
                new Function(new Func<object, bool>(Runtime.IsAtom),
                    "x",
                    "Returns true if x is an atom.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("car"),
                 new Function(new Func<object, object>(Runtime.Car),
                     "seq",
                     "Returns the first item in a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("caar"),
                 new Function(new Func<object, object>(Runtime.Caar),
                     "seq",
                     "Returns the first item of the first item in a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("cadr"),
                new Function(new Func<object, object>(Runtime.Cadr),
                    "seq",
                    "Returns the second item in a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("cddr"),
                new Function(new Func<object, object>(Runtime.Cddr),
                    "seq",
                    "Returns the rest of the rest of a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("cdr"),
                new Function(new Func<object, object>(Runtime.Cdr),
                    "seq",
                    "Returns the rest of a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("coerce"),
                new Function(new Func<object, object, object>(Runtime.Coerce),
                    "x t",
                    "Converts objext x to type t.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("compile"),
                new Function(new Func<object, object>(Compile),
                    "expr",
                    "Compiles the expression and returns executable code.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("cons"),
                new Function(new Func<object, object, object>(Runtime.Cons),
                    "x seq",
                    "Creates a new sequence whose head is x and tail is seq.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("do"),
                new Function(new Func<object[], object>(Runtime.Progn),
                    "& body",
                    "Executes body forms in order, returns the result of the last body.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("do1"),
                new Function(new Func<object[], object>(Runtime.Do1),
                    "& body",
                    "Executes body forms in order, returns the result of the first body.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("empty?"),
                new Function(new Func<object, bool>(Runtime.IsEmpty),
                    "x",
                    "Returns true if x is an empty sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("err"),
                new Function(new Func<object, object>(Runtime.Err),
                    "exception",
                    "Raises an exception", false));

            globalEnvironment.AssignLocal(Symbol.FromName("eval"),
                new Function(new Func<object, object>(Eval),
                    "expr",
                    "Evaluates an expression.", false));

            //globalEnvironment.AssignLocal(Symbol.FromName("help"),
            //    new Function(new Func<object, object>(Runtime.Help),
            //        "x",
            //        "Displays help text for x.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("idfn"),
                new Function(new Func<object, object>(Runtime.IdFn),
                    "x",
                    "The identity function, returns x.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("is"),
                new Function(new Func<object, object, bool>(Runtime.Is),
                    "a b",
                    "Returns true if a and b are the same.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("last"),
                new Function(new Func<object, object>(Runtime.Last),
                    "seq",
                    "Returns the last item in a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("length"),
                new Function(new Func<object, int>(Runtime.Len),
                    "seq",
                    "Returns the length of the sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("list"),
                new Function(new Func<object[], Pair>(Runtime.List),
                    "& xs",
                    "Creates a list of xs.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("load"),
                new Function(new Func<string, object>(Load),
                    "filename",
                    "Loads the lsharp expressions from filename.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("member?"),
                new Function(new Func<object, object, bool>(Runtime.Member),
                    "item seq",
                    "Returns true is item is a member of seq.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("mod"),
                new Function(new Func<object[], object>(Runtime.Mod),
                    "& xs",
                    "Returns the remainder when dividing the args.", true));

            // TODO: Should new be a special form that compiles the constructor call?
            globalEnvironment.AssignLocal(Symbol.FromName("new"),
               new Function(new Func<object[], object>(Runtime.New),
                   "t & xs ",
                   "Constructs a new object of type t with constructir arguments xs.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("not"),
                new Function(new Func<object, bool>(Runtime.Not),
                    "n",
                    "Returns true if n is false, false otherwise.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("nth"),
                new Function(new Func<object, int, object>(Runtime.Nth),
                    "n seq",
                    "Returns the nth element in sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("progn"),
               new Function(new Func<object[], object>(Runtime.Progn),
                   "& xs",
                   "progn xs", true));

            globalEnvironment.AssignLocal(Symbol.FromName("reference"),
               new Function(new Func<object[], object>(Runtime.Reference),
                   "& xs",
                   "Loads the given list of assemblies.", true));

            globalEnvironment.AssignLocal(Symbol.FromName("reverse"),
               new Function(new Func<object, object>(Runtime.Reverse),
                   "seq",
                   "Reverses the sequence.", false));

            //globalEnvironment.AssignLocal(Symbol.FromName("sleep"),
            //   new Function(new Func<double, double>(Runtime.Sleep),
            //       "n",
            //       "Sleeps for n seconds.", false)); 

            globalEnvironment.AssignLocal(Symbol.FromName("seq"),
               new Function(new Func<object, ISequence>(Runtime.Seq),
                   "x",
                   "Returns x if x is a sequence, otherwise returns a Sequence represenation of x.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("seq?"),
               new Function(new Func<object, bool>(Runtime.IsSeq),
                   "x",
                   "Return true if x is a sequence.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("stdin"),
                new Function(new Func<TextReader>(StdIn),
                    "",
                    "Returns the standard input stream.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("stdout"),
                new Function(new Func<TextWriter>(StdOut),
                    "",
                    "Returns the standard output stream.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("stderr"),
                new Function(new Func<TextWriter>(StdErr),
                    "",
                    "Returns the standard error stream.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("str"),
                new Function(new Func<object[], string>(Runtime.Str),
                    "& xs",
                    "xxx", true));


            globalEnvironment.AssignLocal(Symbol.FromName("toarray"),
                new Function(new Func<IEnumerable, object>(Runtime.AsArray),
                    "seq",
                    "Returns an object[] containing all the members of a sequence.",
                    false));

            globalEnvironment.AssignLocal(Symbol.FromName("tolist"),
                new Function(new Func<IEnumerable, Pair>(Runtime.ToList),
                    "seq",
                    "Returns a list containing all the members of a sequence..",
                    false));

            globalEnvironment.AssignLocal(Symbol.FromName("type"),
                new Function(new Func<object, Type>(Runtime.Type),
                    "x",
                    "Returns the Type of x.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("typeof"),
                new Function(new Func<object, Type>(Runtime.TypeOf),
                    "t",
                    "Returns the Type object named t.", false));

            globalEnvironment.AssignLocal(Symbol.FromName("uniq"),
                new Function(new Func<Symbol>(Runtime.Uniq),
                    "",
                    "", false));

            globalEnvironment.AssignLocal(Symbol.FromName("using"),
                new Function(new Func<object[], object>(Runtime.Using),
                    "xs",
                    "XXX", true));

            // Bootstrap L# ..

            EvalString("(LSharp.Runtime.VarSet (quote set) (LSharp.Runtime.MakeMacro '(x y) (quote `(LSharp.Runtime.VarSet (quote ,x) ,y  environment)) \"Set a variable\" environment) environment)");
            EvalString("(set map (fn (f s) \"Maps a function f over a sequence.\" (LSharp.Runtime.map f s environment)))");

            EvalString("(set bound (fn (sym) \"Returns true if sym is bound in the current environment.\" (LSharp.Runtime.Bound sym environment)))");
            EvalString("(set safeset (macro (var val) `(do (if (bound ',var) (prn \"*** redefining \" ',var)) (set ,var ,val))))");
            EvalString("(set mac (macro (name parms & body) \"Creates a new macro.\" `(safeset ,name (macro ,parms ,@body))))");
            EvalString("(mac def (name parms & body) \"Defines a new function.\" `(safeset ,name (fn ,parms ,@body)))");

            // Some synonyms
            EvalString("(set first car)");
            EvalString("(set rest cdr)");
            EvalString("(set len length)");
            EvalString("(set no not)");
            EvalString("(set throw err)");
            EvalString("(set = set)");

            // Define the rest of the language ..

            EvalString("(def apply (f x) (LSharp.Runtime.Apply f x environment))");
            EvalString("(def even (n) \"Returns true if n is even\" (is (mod n 2) 0))");

            //EvalString("(LSharp.Runtime.VarSet (quote if) (LSharp.Runtime.MakeMacro '(& xs) (quote `(LSharp.Runtime.If (map compile (quote ,xs)) environment)) \"if\" environment) environment)");
            EvalString("(def inspect (x) \"Inspects the object x for debugging purposes.\"  (LSharp.Runtime.Inspect x (stdout)))");
            EvalString("(def msec () \"Returns the current time in milliseconds.\" (/ (.ticks (DateTime.Now)) 10000))");
            EvalString("(def sleep (n) \"Sleeps for n seconds\" (System.Threading.Thread.Sleep (coerce (* n 1000) \"Int32\")))");
            EvalString("(def macex (x) (LSharp.Runtime.MacroExpand x false environment))");
            EvalString("(def macex1 (x) (LSharp.Runtime.MacroExpand x true environment))");
            EvalString("(def range (a b (c 1)) (LSharp.Runtime.Range a b c))");
            EvalString("(def pr (& xs) (LSharp.Runtime.Pr xs (stdout)))");
            EvalString("(def prn (& xs) (LSharp.Runtime.Prn xs (stdout)))");
            EvalString("(def help (f) \"Prints help documentation for f\" (LSharp.Runtime.Help f (stdout)))");
            EvalString("(def sqrt (n) (Math.Sqrt (coerce n \"Double\")))");
            EvalString("(def expt (x y) (Math.Pow x y))");
            EvalString("(def odd (n) \"Returns true if n is odd\" (no (even n)))");
            EvalString("(def isa (x t) \"Returns true if x is of type t.\" (is (type x) t))");
            EvalString("(def pair (xs (f list))(if (no xs) nil (no (cdr xs))(list (list (car xs)))(cons (f (car xs) (cadr xs))(pair (cddr xs) f))))");
            EvalString("(def reduce (f s) (LSharp.Runtime.reduce f s environment))");
            EvalString("(mac and (& xs) `(LSharp.Runtime.And (map compile (quote ,xs))  environment))");
            EvalString("(mac with (parms & body) `((fn ,(map car (pair parms)) ,@body) ,@(map cadr (pair parms))))");
            EvalString("(mac let (var val & body) `(with (,var ,val) ,@body))");
            EvalString("(mac or (& xs) `(LSharp.Runtime.Or (map compile (quote ,xs))  environment))");
            EvalString("(mac each (x xs & body) `(LSharp.Runtime.EachLoop ,xs (fn (,x) ,@body) environment))");
            EvalString("(mac while (test & body) `(LSharp.Runtime.WhileLoop (fn () ,test) (fn () ,@body) environment))");
            EvalString("(mac for (x start finish & body) `(LSharp.Runtime.ForLoop ,start ,finish (fn (,x) ,@body) environment))");
            EvalString("(mac nor args `(no (or ,@args)))");
            EvalString("(mac when (test & body) `(if ,test (do ,@body)))");
            EvalString("(mac unless (test & body) `(if (no ,test) (do ,@body)))");
            EvalString("(mac time (expr) \"Times how long it takes to run the expression.\" `(LSharp.Runtime.Time (quote ,expr) (stdout) environment))");
            EvalString("(def iso (x y) \"Isomorphic comparison of x and y.\"(or (is x y)(and (seq? x)(seq? y)(iso (car x) (car y))(iso (cdr x) (cdr y)))))");
            EvalString("(def testify (x) (if (is x (typeof \"LSharp.Function\")) x (fn (a) (is a x))))");
            EvalString("(def some? (f xs) (apply or (map (testify f) xs)))");
            EvalString("(def every? (f xs) (apply and (map (testify f) xs)))");
        }

        public Environment GlobalEnvironment
        {
            get { return globalEnvironment; }
        }

        public static List<string> Namespaces
        {
            get { return namespaces; }
        }

        public static object Help(object x, TextWriter writer)
        {
            if (x is Macro)
                writer.Write("Macro : ");

            if (x is Function)
            {
                writer.WriteLine("{0} : {1}", ((Function)x).Signature, ((Function)x).Documentation);
                return x;
            }
            else
            {
                writer.WriteLine("No help available");
                return null;
            }
        }


        public static Sequence Seq(object o)
        {
            if (o is Sequence)
                return (Sequence)o;

            if (o is string)
                return new StringSequence((string)o);

            if (o is Array)
                return new ArraySequence((Array)o);

            if (o is IEnumerable)
                return new EnumerableSequence((IEnumerable)o);

            throw new LSharpException("Not a sequence");
        }

        public static bool IsSeq(object o)
        {
            return ((o is ISequence) || (o is IEnumerable));
        }

        public static object Last(object arg)
        {
            if (arg == null)
                return null;

            if (arg is Sequence)
                return ((Sequence)arg).Last();

            return Last(Seq(arg));

        }

        public static bool Member(object item, object seq)
        {
            if (seq == null)
                return false;

            if (seq is Sequence)
                return ((Sequence)seq).IsMember(item);

            return Member(item, Seq(seq));
        }

        public static object Apply(object f, IEnumerable args, Environment environment)
        {

            if (args == null)
            {
                return Compiler.FunCall(f, new object[0], environment);
            }

            if (args is Sequence)
            {
                return Compiler.FunCall(f, ((Sequence)args).ToArray(), environment);
            }

            return Apply(f, Seq(args), environment);
        }

        public static object MacroExpand(object expression, bool once, Environment environment)
        {
            if (expression is Pair)
            {
                Pair args = (Pair)expression;

                if (args.First() is Symbol)
                {
                    object value = environment.GetValue((Symbol)args.First());

                    if (value is Macro)
                    {
                        // If its a Macro then invoke it to get the expansion
                        object expansion = ((Macro)value).Call(Runtime.AsArray(((Pair)args).Rest()));

                        // The expansion itself may be a call to a macro - expand that?
                        if (!once)
                            expansion = MacroExpand(expansion, false, environment);

                        return expansion;
                    }
                }


            }

            return expression;

        }
        /// <summary>
        /// Changes a Lisp object into the string of characters that READ 
        /// would need to reconstruct it
        /// </summary>
        public static string PrintToString(Object expression)
        {
            if (expression is Symbol)
                return string.Format("{0}", ((Symbol)expression).ToString());

            if (expression is ISequence)
            {
                ISequence sequence = (ISequence)expression;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("(");
                bool tail = false;
                foreach (object item in sequence)
                {
                    if (tail)
                        stringBuilder.Append(" ");
                    else
                        tail = true;

                    stringBuilder.Append(PrintToString(item));
                }

                stringBuilder.Append(")");
                return stringBuilder.ToString();

            }

            if (expression == null)
                return "null";

            if (expression is string)
                return string.Format("\"{0}\"", (string)expression);

            if (expression is char)
            {
                if ((char)expression == '\n')
                    return "#\\newline";

                // TODO: deal with other named characters here
                return string.Format("#\\{0}", expression);
            }

            return expression.ToString();
        }

        public static object[] AsArray(object list)
        {
            if (list == null)
                return new object[0];

            if (list is Sequence)
                return ((Sequence)list).ToArray();

            return AsArray(Seq(list));


        }

        public static Pair ToList(object list)
        {
            if (list == null)
                return null;

            if (list is Sequence)
                return ((Sequence)list).ToList();

            return ToList(Seq(list));
        }


        private static object Reverse(object arg)
        {
            if (arg == null)
                return null;

            if (arg is Sequence)
                return ((Sequence)arg).Reverse();

            return Reverse(Seq(arg));

        }

        public static bool Is(object a, object b)
        {
            if (a == null)
                return (b == null);

            return (a.Equals(b));

        }

        public static bool Not(object a)
        {
            if (a == null)
                return true;

            if (a is bool)
                return !(bool)a;

            return false;

        }



        public static bool IsAtom(object arg)
        {
            return !(IsSeq(arg));
        }

        // NB This is a Special Form
        public static bool And(IEnumerable args, Environment environment)
        {
            if (args == null)
                return true;

            foreach (object arg in args)
            {
                if (!Boolify(Compiler.Eval(arg, environment)))
                    return false;
            }
            return true;
        }

        public static object If(object args, Environment environment)
        {
            if (args == null)
                return null;

            if (args is Sequence)
            {
                ISequence s = (ISequence)args;

                while (s != null)
                {
                    object testPart = s.First();

                    if (s.Rest() == null)
                        return (Compiler.Eval(testPart, environment));

                    s = s.Rest();

                    bool b = Boolify(Compiler.Eval(testPart, environment));
                    if (b)
                    {
                        object thenPart = s.First();
                        return (Compiler.Eval(thenPart, environment));
                    }

                    s = s.Rest();
                }

            }

            return false;
        }

        public static bool Or(IEnumerable args, Environment environment)
        {
            if (args == null)
                return false;

            foreach (object item in args)
            {
                if (Boolify(Compiler.Eval(item, environment)))
                    return true;
            }

            return false;
        }

        public static object Add(IEnumerable args)
        {

            if (args == null)
                return 0;

            if ((args is Array) && ((Array)args).Length == 0)
                return 0;


            if (AllString(args))
            {
                return StringAppend(args);
            }

            if (AllSeq(args))
            {
                return Join(args);
            }

            Type type = typeof(double);

            if (AllInt(args))
            {
                type = typeof(int);
            }


            Double result = 0;
            foreach (Object item in args)
            {
                result += Convert.ToDouble(item);
            }
            return Convert.ChangeType(result, type, CultureInfo.InvariantCulture);


        }

        /// <summary>
        /// (- number*) Subtraction
        /// </summary>
        public static Object Subtract(IEnumerable args)
        {
            if (args == null)
                return 0;

            if ((args is Array) && ((Array)args).Length == 0)
                return 0;

            Type type = typeof(double);

            if (AllInt(args))
            {
                type = typeof(int);
            }

            Double result = 0;
            bool first = true;
            foreach (Object item in args)
            {
                if (first)
                {
                    first = false;
                    result += Convert.ToDouble(item);
                }
                else
                {
                    result -= Convert.ToDouble(item);
                }


            }
            return Convert.ChangeType(result, type, CultureInfo.InvariantCulture);

        }




        public static Type Type(object o)
        {
            return o.GetType();
        }

        /// <summary>
        /// Returns true if all arguments are integers
        /// </summary>
        public static bool AllInt(IEnumerable args)
        {
            foreach (object arg in args)
            {
                if (!(arg is int))
                    return false;
            }
            return true;
        }


        public static bool AllSeq(IEnumerable args)
        {
            foreach (object arg in args)
            {
                if (!IsSeq(arg))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if all arguments are strings
        /// </summary>
        public static bool AllString(IEnumerable args)
        {
            foreach (object arg in args)
            {
                if (!(arg is string))
                    return false;
            }
            return true;
        }

        public static string StringAppend(IEnumerable args)
        {
            StringBuilder s = new StringBuilder();
            foreach (object arg in args)
            {
                s.Append(arg as string);
            }
            return s.ToString();

        }

        public static Object Cons(object a, object b)
        {
            if (b == null)
                return new Pair(a);

            if (b is ISequence)
                return ((ISequence)b).Cons(a);

            return Cons(a, (Seq(b)));
        }

        public static Object Disp(object arg, TextWriter textWriter)
        {
            if (arg is string)
                textWriter.Write(arg);
            else
                textWriter.Write(PrintToString(arg));

            return null;
        }

        public static Object WriteC(char c, TextWriter textWriter)
        {
            textWriter.Write(c);
            return c;
        }

        public static Object Caar(object arg)
        {
            return (Car(Car(arg)));
        }

        public static Object Cadr(object arg)
        {
            return (Car(Cdr(arg)));
        }

        public static Object Cddr(object arg)
        {
            return (Cdr(Cdr(arg)));
        }

        public static Object Car(object arg)
        {
            if (arg == null)
                return null;

            if (arg is ISequence)
                return ((ISequence)arg).First();

            return Car(Seq(arg));

        }

        public static Object Cdr(object arg)
        {
            if (arg == null)
                return null;

            if (arg is ISequence)
                return ((ISequence)arg).Rest();

            return Cdr(Seq(arg));

        }

        /// <summary>
        /// (> object1 object2 object*) Returns true if object1 is greater than 
        /// object2, object2 is greater than object3 and so on. 
        /// </summary>
        public static Object GreaterThan(IEnumerable args)
        {
            bool first = true;
            Double last = 0;

            foreach (object item in args)
            {
                if (first)
                {
                    first = false;
                    last = Convert.ToDouble(item);
                }
                else
                {
                    Double current = Convert.ToDouble(item);
                    if (!(last > current))
                        return false;
                    last = current;
                }
            }
            return true;
        }

        /// <summary>
        /// (< object1 object2 object*) Less than
        /// Returns true if object1 is less than object2 and object2 is less than object3 and 
        /// so on.
        /// </summary>
        public static Object LessThan(IEnumerable args)
        {
            bool first = true;
            Double last = 0;

            foreach (object item in args)
            {
                if (first)
                {
                    first = false;
                    last = Convert.ToDouble(item);
                }
                else
                {
                    Double current = Convert.ToDouble(item);
                    if (!(last < current))
                        return false;
                    last = current;
                }
            }
            return true;
        }

        /// <summary>
        /// Length
        /// </summary>

        public static object Nth(object arg, int n)
        {
            if (arg == null)
                return null; ;

            if (arg is Sequence)
                return ((Sequence)arg).Nth(n);

            return Nth(Seq(arg), n);
        }


        public static Object IdFn(object arg)
        {
            return arg;
        }

        public static bool IsEmpty(object arg)
        {
            if (arg == null)
                return true; ;

            if (arg is Sequence)
                return ((Sequence)arg).IsEmpty();

            return IsEmpty(Seq(arg));
        }

        public static int Len(object arg)
        {
            if (arg == null)
                return 0;

            if (arg is Sequence)
                return ((Sequence)arg).Length();

            return Len(Seq(arg));

        }

        private static int SequenceLength(ISequence sequence)
        {
            int i = 1;
            ISequence o = sequence;
            while (o.Rest() != null)
            {
                i += 1;
                o = o.Rest();
            }
            return i;
        }

        public static object ReadFromString(string s)
        {
            TextReader textReader = new StringReader(s);

            return Reader.Read(textReader);
        }

        /// <summary>
        /// (* number*)
        /// Returns the result of multiplying all number arguments
        /// </summary>
        public static Object Multiply(object[] args)
        {
            if (args.Length == 0)
                return 1; // The identity for multiplication

            Type type = args[0].GetType();
            Double result = 1;
            foreach (Object item in args)
            {
                if (item is Double)
                    type = item.GetType();

                result *= Convert.ToDouble(item);
            }
            return Convert.ChangeType(result, type, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// (/ numerator denominator+) 
        /// Divides a numerator by one or more denominators
        /// </summary>
        public static Object Divide(object[] args)
        {
            Type type = args[0].GetType();
            Double result = Convert.ToDouble(args[0]);
            foreach (object item in args.Skip<object>(1))
            {
                if (item is Double)
                    type = item.GetType();

                result /= Convert.ToDouble(item);
            }
            return Convert.ChangeType(result, type, CultureInfo.InvariantCulture);
        }

        public static Object Mod(object[] args)
        {
            Type type = args[0].GetType();
            Double result = Convert.ToDouble(args[0]);
            foreach (object item in args.Skip<object>(1))
            {
                if (item is Double)
                    type = item.GetType();

                result %= Convert.ToDouble(item);
            }
            return Convert.ChangeType(result, type, CultureInfo.InvariantCulture);
        }

        public static Object New(object[] args)
        {
            Type tt;

            if (args[0] is string)
                tt = TypeOf(args[0]);
            else
                tt = (Type)args[0];

            return ClrGlue.MakeInstance(tt, args.Skip<object>(1).ToArray());

        }

        public static Object Using(IEnumerable n)
        {
            object result = null;
            foreach (string s in n)
            {

                if (!namespaces.Contains(s))
                {
                    namespaces.Add(s);
                    result = s;
                }


            }
            return result;
        }

        public static Object Reference(object[] assemblyNames)
        {
            object result = null;
            foreach (string assemblyName in assemblyNames)
            {
                result = ClrGlue.LoadAssembly(assemblyName);

            }
            return result;
        }


        public static Object Progn(object[] args)
        {
            int i = args.Length;
            return args[i - 1];
        }

        public static Object Do1(object[] args)
        {
            return args[0];
        }

        public static Object Pr(IEnumerable args, TextWriter textWriter)
        {
            object last = null;
            foreach (object o in args)
            {
                textWriter.Write(o);
                last = o;
            }
            return last;
        }

        public static string Str(IEnumerable xs)
        {
            StringBuilder s = new StringBuilder();

            foreach (object x in xs)
            {
                if (IsSeq(x))
                    s.Append((Seq(x)).ToString());
                else
                    s.Append(x.ToString());
            }
            return s.ToString();
        }

        public static Pair List(IEnumerable args)
        {
            Pair c = null;

            foreach (object arg in args)
            {
                c = new Pair(arg, c);
            }

            return (Pair)Reverse(c);
        }

        public static Object Prn(IEnumerable args, TextWriter textWriter)
        {
            object o = Pr(args, textWriter);

            textWriter.WriteLine();

            return o;
        }

        public static Object Err(object o)
        {
            if (o is Exception)
                throw ((Exception)o);

            throw new LSharpException((string)o);
        }

        public static Type TypeOf(object arg)
        {
            string typeName;
            if (arg.GetType() == typeof(Symbol))
                typeName = ((Symbol)arg).Name;
            else
                typeName = arg.ToString();

            return ClrGlue.FindType(typeName, namespaces);
        }

        public static Pair FromArray(object[] os, int skip)
        {
            int i = 0;

            Pair x = null;

            foreach (object o in os)
            {
                if (i++ >= skip)
                    x = new Pair(o, x);
            }


            x = (Pair)Reverse(x);

            return x;
        }

        /// <summary>
        /// Returns the value that symbol is bound to in the
        /// given environment
        /// </summary>
        public static object VarRef(Symbol symbol, Environment environment)
        {
            object value = environment.GetValue(symbol);
            return value;
        }

        public static bool HasSyntaxChar(string s)
        {
            foreach (char c in s)
            {
                if ((c == ':') || (c == '~') || (c == '.') || (c == '!'))
                    return true;
            }
            return false;
        }

        public static bool IsSsyntax(object x)
        {
            if (x.GetType() == typeof(Symbol))
            {
                Symbol s = (Symbol)x;

                if ((x == PLUS) || (x == PLUSPLUS))
                    return false;

                return HasSyntaxChar(s.Name);
            }

            return false;

        }

        public static Object ExpandSSyntax(Object x)
        {

            Symbol s = (Symbol)x;

            //if ((s.Name.Contains(':')) || (s.Name.Contains('~')))
            //    return Symbol.FromName("expand-compose");

            //if (s.Name.Contains('!'))
            //    return Symbol.FromName("expand-sexpr");

            if (s.Name.StartsWith("."))
                return Symbol.FromName("call-instance");

            if (s.Name.Contains('.'))
                return Symbol.FromName("call-clr");

            throw new LSharpException("Unknown special syntax " + x);
        }

        public static bool IsLiteral(Object o)
        {
            if (o == null)
                return true;

            if (o == null)
                return true;

            Type t = o.GetType();

            return (t == typeof(bool)) || (t == typeof(char)) || (t == typeof(string)) || (t == typeof(object[])) || Runtime.IsNumber(o);
        }

        public static bool IsNumber(Object o)
        {
            Type t = o.GetType();

            // TODO: Are there other types which are numbers - int64?
            return (t == typeof(int)) || (t == typeof(double)) || (t == typeof(float));
        }

        public static object EachLoop(IEnumerable enumeration, object body, Environment environment)
        {
            object ret = null;

            foreach (object o in enumeration)
            {
                ret = Compiler.FunCall(body, new object[] { o }, environment);
            }
            return ret;
        }


        public static ISequence Map(Function f, object arg, Environment environment)
        {
            if (arg == null)
                return null;

            if (arg is Sequence)
                return ((Sequence)arg).Map(f, environment);

            return Map(f, Seq(arg), environment);

        }

        public static ISequence Range(int start, int finish, int step)
        {
            Pair result = null;
            for (int i = start; i < finish; i += step)
            {
                result = new Pair(i, result);
            }
            return result.Reverse();
        }

        public static ISequence Join(IEnumerable os)
        {

            Pair foo = null;

            foreach (object o in os)
            {
                foo = new Pair(o, foo);
            }

            ISequence bar = null;

            foreach (object o in foo)
            {
                bar = Join(Seq(o), bar);
            }

            return bar;
        }

        public static ISequence Join(ISequence a, ISequence b)
        {

            if (a == null)
                return b;

            if (b == null)
                return a;

            return Sequence.Join(a, b);
        }

        public static object Reduce(Function f, object arg, Environment environment)
        {
            if (arg == null)
                return null;

            if (arg is Sequence)
                return ((Sequence)arg).Reduce(f, environment);

            return Reduce(f, Seq(arg), environment);
        }


        public static object ForLoop(int start, int finish, object body, Environment environment)
        {
            object ret = null;

            for (int i = start; i <= finish; i++)
            {
                ret = Compiler.FunCall(body, new object[] { i }, environment);
            }
            return ret;
        }

        public static object WhileLoop(object test, object body, Environment environment)
        {
            object ret = null;

            while ((bool)Compiler.FunCall(test, new object[0], environment))
            {
                ret = Compiler.FunCall(body, new object[0], environment);
            }
            return ret;
        }

        public static bool Bound(Symbol symbol, Environment environment)
        {
            if (environment.Contains(symbol))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Binds the given symbol to the given value in the given
        /// environment
        /// </summary>
        public static object VarSet(Symbol symbol, Object value, Environment environment)
        {
            environment.Set(symbol, value);
            return value;
        }

        /// <summary>
        /// LSharp has a liberal view of true and false, including T NIL and null.
        /// This method normalises any object to Boolean true or false.
        /// </summary>
        public static bool Boolify(object o)
        {
            return (!((o == null) || (o is bool) && ((bool)o == false)));
        }

        public static Function MakeFunction(object parameters, object body, string documentation, Environment environment)
        {
            Closure closure = new Closure(parameters, body, environment);
            string signature = "";

            if (parameters != null)
                signature = PrintToString(parameters);

            return new Function(new Func<object[], object>(closure.Invoke), signature, documentation, true);
        }

        public static Macro MakeMacro(object parameters, object body, string documentation, Environment environment)
        {
            Closure closure = new Closure(parameters, body, environment);
            string signature = "";

            if (parameters != null)
                signature = PrintToString(parameters);

            return new Macro(new Func<object[], object>(closure.Invoke), signature, documentation, true);
        }

        private static int genSymCount = 0;

        public static Symbol Uniq()
        {
            return new Symbol("g" + genSymCount++);
        }

        /// <summary>
        /// Changes the type of o to be type t
        /// </summary>
        public static object Coerce(object o, object t)
        {
            Type type;

            if (t is String)
                type = TypeOf(t);
            else
                type = (Type)t;

            return Convert.ChangeType(o, type, CultureInfo.InvariantCulture);
        }

        public static object QuasiQuote(Object form, Environment environment)
        {
            if (!(form is Pair))
                return form;

            Pair expression = (Pair)form;

            Pair result = null;
            foreach (object item in expression)
            {
                if (item is Pair)
                {
                    Pair list = (Pair)item;
                    if (list.First() == QUASIQUOTE)
                    {
                        result = new Pair(QuasiQuote(list.Second(), environment), result);
                    }
                    else if (list.First() == UNQUOTE)
                    {
                        result = new Pair(Compiler.Eval(QuasiQuote(list.Second(), environment), environment), result);
                    }
                    else if (list.First() == UNQUOTE_SPLICING)
                    {
                        object l = Compiler.Eval(QuasiQuote(list.Second(), environment), environment);

                        if (l is Sequence)
                        {
                            foreach (object o in (Sequence)l)
                            {
                                result = new Pair(o, result);
                            }
                        }

                    }
                    else
                    {
                        result = new Pair(QuasiQuote(item, environment), result);
                    }

                }
                else
                {
                    result = new Pair(item, result);
                }
            }

            if (result is Pair)
                result = (Pair)Reverse((ISequence)result);

            return result;
        }

        public static object Inspect(object o, TextWriter writer)
        {
            if (o == null)
                writer.WriteLine("null");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(string.Format("// Value={0}\r\n", PrintToString(o)));
            stringBuilder.Append(ClrGlue.TypeInfo(o.GetType()));
            stringBuilder.Append("{\n");
            stringBuilder.Append(ClrGlue.Fields(o));
            stringBuilder.Append("}");

            writer.WriteLine(stringBuilder.ToString());

            return o;
        }

        /// <summary>
        /// A Read Eval Print Loop
        /// </summary>
        public void Repl()
        {
            stdout.WriteLine("L Sharp {0} on {1}", ClrGlue.LSharpVersion(), ClrGlue.EnvironmentVersion());
            stdout.WriteLine("Copyright (c) Rob Blackwell. All rights reserved.");

            // Keep results of recent evaluations using shorthand *1, *2 and *3
            Symbol starOne = Symbol.FromName("*1");
            VarSet(starOne, null, globalEnvironment);

            Symbol starTwo = Symbol.FromName("*2");
            VarSet(starTwo, null, globalEnvironment);

            Symbol starThree = Symbol.FromName("*3");
            VarSet(starThree, null, globalEnvironment);

            // Keep the last exception using the shorthand *e
            Symbol lastException = Symbol.FromName("*e");
            VarSet(lastException, null, globalEnvironment);

            while (true)
            {
                try
                {
                    stdout.Write("> ");

                    // Read
                    object input = Reader.Read(stdin);

                    if (input == EXIT)
                        break;

                    // Eval
                    object output = Eval(input);

                    // Update recent evaluations
                    VarSet(starThree, VarRef(starTwo, globalEnvironment), globalEnvironment);
                    VarSet(starTwo, VarRef(starOne, globalEnvironment), globalEnvironment);
                    VarSet(starOne, output, globalEnvironment);

                    // Print
                    stdout.WriteLine(Runtime.PrintToString(output));
                }

                catch (Exception e)
                {
                    stderr.WriteLine("Exception : {0}", e.Message);

                    // Keep track of the exception in *e
                    VarSet(lastException, e, globalEnvironment);
                }
            }
        }

        public Object Compile(object arg)
        {
            return Compiler.Compile(arg, globalEnvironment);
        }

        public Object Eval(object arg)
        {
            return Compiler.Eval(arg, globalEnvironment);
        }

        public static Object Time(object arg, TextWriter textWriter, Environment environment)
        {
            long start = DateTime.Now.Ticks / 10000;
            object o = Compiler.Eval(arg, environment);
            long stop = DateTime.Now.Ticks / 10000;

            textWriter.WriteLine("time: {0} msec", stop - start);

            return o;
        }


        public TextReader StdIn()
        {
            return stdin;
        }

        public TextWriter StdOut()
        {
            return stdout;
        }

        public TextWriter StdErr()
        {
            return stderr;
        }

        public Object EvalString(string s)
        {
            return Compiler.Eval(Reader.Read(new StringReader(s)), globalEnvironment);
        }

        public Object Load(string filename)
        {
            return Load(filename, globalEnvironment);
        }

        public static Object Load(string filename, Environment environment)
        {
            TextReader textReader = File.OpenText(filename);
            object i;
            object o = null;

            try
            {
                do
                {
                    i = Reader.Read(textReader);
                    if (i != Reader.EOF)
                    {
                        Console.Write(".");

                        if (i == EXIT)
                            break;

                        o = Compiler.Eval(i, environment);

                    }
                } while (i != Reader.EOF);
            }
            finally
            {
                textReader.Close();
            }

            Console.WriteLine();
            return o;
        }
    }
}
