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
using System.Linq.Expressions;
using System.Reflection;

namespace LSharp
{
    public class Compiler
    {
        private static ParameterExpression environmentParameter = Expression.Parameter(typeof(Environment), "environment");

        // Define some commonly used symbols
        public static Symbol UNQUOTE_SPLICING = Symbol.FromName("unquote-splicing");
        public static Symbol UNQUOTE = Symbol.FromName("unquote");
        public static Symbol QUASIQUOTE = Symbol.FromName("quasiquote");
        public static Symbol FN = Symbol.FromName("fn");
        public static Symbol PROGN = Symbol.FromName("progn");

        // These are methods which a called by the target code
        private static MethodInfo varSetMethodInfo = typeof(Runtime).GetMethod("VarSet");
        private static MethodInfo varRefMethodInfo = typeof(Runtime).GetMethod("VarRef");
        private static MethodInfo callMethodInfo = typeof(ClrGlue).GetMethod("CallMethod");
        private static MethodInfo quasiQuoteMethodInfo = typeof(Runtime).GetMethod("QuasiQuote");
        private static MethodInfo callStaticMethodMethodInfo = typeof(ClrGlue).GetMethod("CallStaticMethod");
        private static MethodInfo boolifyMethodInfo = typeof(Runtime).GetMethod("Boolify");
        private static MethodInfo makeFunctionMethodInfo = typeof(Runtime).GetMethod("MakeFunction");
        private static MethodInfo makeMacroMethodInfo = typeof(Runtime).GetMethod("MakeMacro");
        private static MethodInfo funCallMethodInfo = typeof(Compiler).GetMethod("FunCall", new Type[] { typeof(object), typeof(object[]), typeof(Environment) });
            
        // When optimise is false we work with LINQ Expression trees and compile them at runtime
        // When true we compile them at compile time
        static bool optimise = true;
        
        public static bool Optimise
        {
            get { return optimise; }
            set { optimise = value; }
        }


        public static object Compile(object expr, Environment environment)
        {
            // If expr is already a LINQ Expression tree then compile it
            // fully to executable code
            if (expr.GetType().BaseType == typeof(Expression))
            {
                return LinqCompile((Expression)expr);
            }

            // Otherwise, compile to a LINQ expression tree
            Expression expression = Compile1(expr, environment);

            // If optimisation is on, then compile the expression tree to executable code
            if (optimise)
                return LinqCompile(expression);
            else
                return expression;

        }

        public static Object Eval(object o, Environment environment)
        {
           
            // If it's a delegate, just run it
            if (o is Func<Environment, object>)
            {
                Func<Environment, object> f = (Func<Environment, object>)o;
                // Execute the lambda expression.
                return f(environment);
            }

            // If it's a LINQ Expression, then compile and run it
            if (o is Expression)
            {
                Expression expression = (Expression)o;
                Func<Environment, object> f = Compiler.LinqCompile(expression);
                return f(environment);
            }

            // Otherwise compile it and evaluate it again ..
            object compiledExpression = Compiler.Compile(o, environment);
            return Eval(compiledExpression, environment);

        }

        public static object FunCall(object f, object[] a, Environment environment)
        {
            // Macros are expanded and evaluated ..
            if (f is Macro)
            {
                object expansion = ((Macro)f).Call(a);
                return Eval(expansion, environment);
            }

            // Functions are called ..
            if (f is Function)
            {
                return ((Function)f).Call(a);
            }

            // Delegates are invoked ..
            if (f is Delegate)
            {
                return ((Delegate)f).DynamicInvoke(a);
            }

            // LINQ Expressions are compiled and invoked ..
            if (f is Expression)
            {
                Func<Environment, object> ff = Compiler.LinqCompile((Expression)f);
                return ff(environment);
            }

            throw new LSharpException(string.Format("Cant call {0} with {1}", f, a));
        }

        public static Func<Environment, Object> LinqCompile(Expression expression)
        {
            // If necessary, convert the expression to an object type.
            if (expression.Type != typeof(object))
                expression = Expression.Convert(expression, typeof(object));

            // Wrap the expression in a lambda and compile into executable code
            Expression<Func<Environment, Object>> lambdaExpression = Expression.Lambda<Func<Environment, Object>>(expression, environmentParameter);

            return lambdaExpression.Compile();
        }


        public static Expression CompileVarRef(Symbol symbol)
        {
            return Expression.Call(null, varRefMethodInfo, Expression.Constant(symbol), environmentParameter);      
        }

        public static Expression CompileQuasiQuote(ISequence args, Environment environment)
        {
            object o = CompileQuasiQuote(1, args.First(), environment);

            Expression e = Expression.Constant(o, typeof(object));

            Expression[] es = new Expression[2];
            es[0] = e;
            es[1] = environmentParameter;

            e = Expression.Call(null, quasiQuoteMethodInfo, es);

            return e;
        }

        public static object CompileQuasiQuote(int level, object arg, Environment environment)
        {
            if (level == 0)
                return Compile1(arg, environment);

            if (arg is ISequence)
            {
                ISequence c = (ISequence)arg;
                if (c.First() == UNQUOTE)
                {
                     return new Pair(UNQUOTE,new Pair(CompileQuasiQuote(level - 1, Runtime.Car(Runtime.Cdr(arg)),environment)));
                }

                if ((level == 1) && (c.First() == UNQUOTE_SPLICING))
                {
                    return new Pair(UNQUOTE_SPLICING, new Pair(CompileQuasiQuote(level - 1, Runtime.Car(Runtime.Cdr(arg)), environment)));
                }

                if (c.First() == QUASIQUOTE)
                {
                    return new Pair(QUASIQUOTE, new Pair(CompileQuasiQuote(level + 1, Runtime.Car(Runtime.Cdr(arg)), environment)));
                }

                Pair r = null;
                foreach (object o in c)
                {
                    r = new Pair(CompileQuasiQuote(level, o,environment), r);
                }

                return r.Reverse();
            }

            return arg;
        }




        public static Expression CompileInstanceCall(Sequence expr, Environment environment)
        {
  
            string methodName = ((Symbol)expr.First()).Name.Substring(1);
 
            Expression[] es = CompileArgs1(expr.Cddr(), environment);
            Expression[] ees = new Expression[3];

            ees[0] = Expression.Constant(methodName);
            ees[1] = Compile1(expr.Second(), environment);
            ees[2] = Expression.NewArrayInit(typeof(object), es);

            if (ees[1].Type != typeof(object))
                ees[1] = Expression.Convert(ees[1], typeof(object));

            return Expression.Call(null, callMethodInfo, ees);

        }

        public static Expression CompileCall(Pair c, Environment environment)
        {

            string dotNetExpression = ((Symbol)c.First()).Name;

            int n = dotNetExpression.LastIndexOf('.');

            string typeName = dotNetExpression.Substring(0, n);
            string methodName = dotNetExpression.Substring(n + 1);

            Type t = ClrGlue.FindType(typeName, Runtime.Namespaces);

            MemberInfo m = ClrGlue.FindUnambiguousMember(methodName, t, c.Length() - 1);

            if (m == null)
            {
                // If we cant find an unambiguos MemberInfo then we'll
                // have to compile a call to CallStaticMethod instead.
                // This will look for a method again reflectively at
                // runtime, so this will be a performance hit
                Expression[] es = CompileArgs1(c.Rest(), environment);

                Expression[] ees = new Expression[3];

                ees[0] = Expression.Constant(methodName);
                ees[1] = Expression.Constant(t, typeof(Type));
                ees[2] = Expression.NewArrayInit(typeof(object), es);

                return Expression.Call(null, callStaticMethodMethodInfo, ees);
            }
            else
            {
                // We know what we're doing at compile time, so lets compile
                // up the call now.
                // No need for runtime reflection = better performance !!

                
                if (m is MethodInfo)
                {

                    if (((MethodInfo)m).IsStatic)
                    {
                        Expression[] es = CompileArgsX(c.Rest(), ((MethodInfo)m).GetParameters(), environment);
                        return Expression.Call(null, (MethodInfo)m, es);
                    }
                    else
                    {
                        // Its a virtual call
                        Expression[] es = CompileArgsX(c.Rest().Rest(), ((MethodInfo)m).GetParameters(), environment);
                        Expression o = Compile1(c.Rest().First(),environment);
                        return Expression.Call(o, (MethodInfo)m, es);
                    }
                }

                if (m is PropertyInfo)
                        return Expression.Property(null, (PropertyInfo)m);

                if (m is FieldInfo)
                    return Expression.Field(null, (FieldInfo)m);

                return null;
            }
            
        }


        public static Expression CompileQuote(ISequence args, Environment environment)
        {
            return Expression.Constant(args.First(), typeof(object));
        }

        public static Expression CompileIf(Object args, Environment environment)
        {

            if (args == null)
                return Expression.Constant(null);

            if (((Pair)args).Rest() == null)
                return Compile1(((Pair)args).First(), environment);

            Expression test = Compile1(((Pair)args).First(), environment);

            // If the test isnt already boolean, then we need to make it so ..
            if (test.Type != typeof(bool))
                if (test.Type == typeof(object))
                    // Either at runtime by trying to convert the expression to a bool
                    test = Expression.Call(null, boolifyMethodInfo, test);
                else
                {
                    // Or now at compile time
                    if ((test.Type == typeof(Symbol)) && (((ConstantExpression)test).Value == null))
                    {
                        test = Expression.Constant(false);
                    }
                    else
                        // TODO: Check we wont loose side effects here?
                        test = Expression.Constant(true);
                }

            Expression thenPart = Compile1(((Pair)args).Second(), environment);

            if (thenPart.Type != typeof(object))
                thenPart = Expression.Convert(thenPart, typeof(object));

            Expression elsePart = CompileIf(((Pair)args).Cddr(), environment);

            if (elsePart.Type != typeof(object))
                elsePart = Expression.Convert(elsePart, typeof(object));

            return Expression.Condition(test, thenPart, elsePart);

        }



        public static Expression CompileFn(object stuff, Environment environment)
        {
            object args = ((Pair)stuff).First();
            Pair body = (Pair)((Pair)stuff).Rest();

            string doc = "";
            if ((body.Length() > 1) && (body.First() is string))
            {
                doc = (string)body.First();
                body = (Pair)body.Rest();
            }

            Pair implicitProgn = new Pair(PROGN, body); // Dangerous if it gets redefined

            object v = Compile(implicitProgn, environment);

            return Expression.Call(null, makeFunctionMethodInfo, Expression.Constant(args), Expression.Constant(v), Expression.Constant(doc), environmentParameter);

        }

        public static Expression CompileMac(object stuff, Environment environment)
        {
            object args = ((Pair)stuff).First();
            Pair body = (Pair)((Pair)stuff).Rest();

            string doc = "";

            if ((body.Length() > 1) && (body.First() is string))
            {
                doc = (string)body.First();
                body = (Pair)body.Rest();
            }

            Pair implicitProgn = new Pair(PROGN, body); // Dangerous if it gets redefined

            object v = Compile(implicitProgn, environment);

            return Expression.Call(null, makeMacroMethodInfo, Expression.Constant(args), Expression.Constant(v), Expression.Constant(doc), environmentParameter);

        }

        public static Expression[] CompileArgs1(object args, Environment environment)
        {
            int l = 0;
            if (args is Pair)
                l = Runtime.Len(args);


            Expression[] es = new Expression[l];
            object foo = args;
            for (int i = 0; i < l; i++)
            {
                es[i] = Expression.Convert(Compile1(((Pair)foo).First(), environment), typeof(object));
                foo = ((Pair)foo).Rest();
            }
            return es;
        }

        public static Expression[] CompileArgsX(object args, ParameterInfo[] p,Environment environment)
        {
            int l = 0;
            if (args is Pair)
                l = Runtime.Len(args);


            Expression[] es = new Expression[l];
            object foo = args;
            for (int i = 0; i < l; i++)
            {
                es[i] = Compile1(((Pair)foo).First(), environment);

                if (es[i].Type != p[i].ParameterType)
                    es[i] = Expression.Convert(es[i], p[i].ParameterType);

                foo = ((Pair)foo).Rest();
            }
            return es;
        }

        public static Expression CompileFunctionCall(Object f, object args, Environment environment)
        {
            // Compile all the arguments so that they'll be evaluated
            // before th function is called
            Expression[] es = CompileArgs1(args, environment);

            Expression[] ees = new Expression[3];
            ees[0] = Compile1(f,environment); // Compile the function
            ees[1] = Expression.NewArrayInit(typeof(object), es);
            ees[2] = environmentParameter;

            return Expression.Call(null, funCallMethodInfo, ees);

        }

        



        public static Expression Compile1(Object s, Environment environment)
        {
            // Literals are constants
            if (Runtime.IsLiteral(s)) 
                return Expression.Constant(s);

            // Special Syntax gets expanded and compiled
            if (Runtime.IsSsyntax(s))
                return Compile1(Runtime.ExpandSSyntax(s),environment);

            // Symbols are variable references
            if (s is Symbol)
                return CompileVarRef((Symbol)s);

            // Special Syntax gets expanded and compiled
            if (Runtime.IsSsyntax(Runtime.Car(s)))
            {
                object expansion = Runtime.ExpandSSyntax(((Pair)s).First());
                return Compile1(new Pair(expansion, (Pair)s),environment);
            }

            if (s is Pair)
            {
                object f = ((Pair)s).First();

                // Special Forms

                if (f == Symbol.FromName("if"))
                    return Compiler.CompileIf(((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("quote"))
                    return Compiler.CompileQuote(((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("call-clr"))
                    return Compiler.CompileCall((Pair)((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("call-instance"))
                    return Compiler.CompileInstanceCall((Pair)((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("fn"))
                    return Compiler.CompileFn((Pair)((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("macro"))
                    return Compiler.CompileMac((Pair)((Pair)s).Rest(), environment);

                if (f == Symbol.FromName("quasiquote"))
                    return Compiler.CompileQuasiQuote((Pair)((Pair)s).Rest(), environment);

   

                if ((f is Symbol) && (environment.Contains((Symbol)f)))
                {
                    object value = environment.GetValue((Symbol)f);


                    // Macros get invoked at compile time, then their results are themeselves compiled
                    if (value is Macro)
                    {
                        object expansion = ((Macro)value).Call(Runtime.AsArray(((Pair)s).Rest()));
                        return Compile1(expansion,environment);
                    }

                }

                // It must be a function call
                return CompileFunctionCall(f, ((Pair)s).Rest(),environment);
                

            }

            throw new LSharpException(string.Format("Bad object in expression: ", s),environment);
   
        }
    }
}
