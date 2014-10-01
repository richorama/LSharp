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
using LSharp;
using NUnit.Framework;
using System.IO;

namespace LSharpTests
{
    /// <summary>
    /// Test suite for LSharp Version 2
    /// </summary>
    [TestFixture]
    public class LanguageTests
    {
        Runtime runtime;

        public LanguageTests()
        {
            runtime = new Runtime(System.Console.In, System.Console.Out, System.Console.Error);
        }

        
        [Test]
        public void Add()
        {
            object actual = runtime.EvalString("(+)");
            Assert.AreEqual(0, actual);

            actual = runtime.EvalString("(+ 1 2 3)");
            Assert.AreEqual(6, actual);

            actual = runtime.EvalString("(+ \"Hello\" \" World\")");
            Assert.AreEqual("Hello World", actual);

            actual = Runtime.PrintToString( runtime.EvalString("(+ '(1 2 3) '(4 5 6))"));
            Assert.AreEqual("(1 2 3 4 5 6)", actual);

            actual = Runtime.PrintToString(runtime.EvalString("(+ [1 2 3] [4 5 6])"));
            Assert.AreEqual("(1 2 3 4 5 6)", actual);
        }

        [Test]
        public void List()
        {
            object actual = Runtime.PrintToString(runtime.EvalString("(list 1 2 3)"));
            Assert.AreEqual("(1 2 3)", actual);
        }


        [Test]
        public void Multiply()
        {
            object actual = runtime.EvalString("(* 4 2)");
            Assert.AreEqual(8, actual);

            actual = runtime.EvalString("(* 4.5 2)");
            Assert.AreEqual(9.0, actual);
        }

        [Test]
        public void GreaterThan()
        {
            object actual = runtime.EvalString("(> 10 9 8 7 6)");
            Assert.AreEqual(true, actual);

            actual = runtime.EvalString("(> 10 9 8 7 9)");
            Assert.AreEqual(false, actual);

        }

        [Test]
        public void Last()
        {
            object actual = runtime.EvalString("(last '(1 2 3))");
            Assert.AreEqual(3, actual);

            actual = runtime.EvalString("(last \"hello\")");
            Assert.AreEqual('o', actual);


        }

        [Test]
        public void Len()
        {
            object actual = runtime.EvalString("(len '(1 2 3 4))");
            Assert.AreEqual(4, actual);

            actual = runtime.EvalString("(len \"hello\")");
            Assert.AreEqual(5, actual);


        }

        [Test]
        public void LessThan()
        {
            object actual = runtime.EvalString("(< 1 2 3 4 5)");
            Assert.AreEqual(true, actual);

            actual = runtime.EvalString("(< 1 2 3 1)");
            Assert.AreEqual(false, actual);

        }

        [Test]
        public void Divide()
        {
            object actual = runtime.EvalString("(/ 4 2)");
            Assert.AreEqual(2, actual);

            actual = runtime.EvalString("(/ 5 2)");
            Assert.AreEqual(2, actual);

            actual = runtime.EvalString("(/ 5.0 2)");
            Assert.AreEqual(2.5, actual);

        }

        [Test]
        public void Mod()
        {
            object actual = runtime.EvalString("(mod 5 2)");
            Assert.AreEqual(1, actual);

            actual = runtime.EvalString("(mod 90 40)");
            Assert.AreEqual(10, actual);

            actual = runtime.EvalString("(mod 90 17 4)");
            Assert.AreEqual(1, actual);

        }

        [Test]
        public void Sub()
        {
            object actual = runtime.EvalString("(- 8 4)");
            Assert.AreEqual(4, actual);

            actual = runtime.EvalString("(-)");
            Assert.AreEqual(0, actual);
        }

    

        [Test]
        public void Isa()
        {
            object actual = runtime.EvalString("(isa (fn (x) (+ x 1)) (typeof \"LSharp.Function\"))");
            Assert.AreEqual(true, actual);
            actual = runtime.EvalString("(isa \"foo\" (typeof \"System.String\"))");
            Assert.AreEqual(true, actual);

            
        }

        [Test]
        public void Apply()
        {

            object actual = runtime.EvalString("(apply + '(1 2 3))");
            Assert.AreEqual(6, actual);
        }

        [Test]
        public void Sleep()
        {
            object actual = runtime.EvalString("(sleep 0.5)");
           

        }

        [Test]
        public void MSec()
        {
            object actual = runtime.EvalString("(msec)");
            Assert.IsInstanceOfType(typeof(Int64), actual);

        }

        [Test]
        public void Macex1()
        {
            object actual = Runtime.PrintToString( runtime.EvalString("(macex1 '(when 1 2))"));
            Assert.AreEqual("(if 1 (do 2))", actual);

        }

        [Test]
        public void New()
        {
            object actual = runtime.EvalString("(new \"Random\")");
            Assert.IsInstanceOfType(typeof(Random), actual);

            actual = runtime.EvalString("(new \"Random\" 10)");
            Assert.IsInstanceOfType(typeof(Random), actual);

            actual = runtime.EvalString("(new (typeof \"Random\") 10)");
            Assert.IsInstanceOfType(typeof(Random), actual);

        }

        [Test]
        public void Let()
        {
            object actual = runtime.EvalString("(let x 100 (prn x))");
            Assert.AreEqual(100, actual);

        }

        [Test]
        public void Not()
        {
            object actual = runtime.EvalString("(not 1)");
            Assert.AreEqual(false, actual);

            actual = runtime.EvalString("(not nil)");
            Assert.AreEqual(true, actual);

        }

        [Test]
        public void Time()
        {
            object actual = runtime.EvalString("(time (+ 1 2))");
            Assert.AreEqual(3, actual);

        }


        [Test]
        public void Nor()
        {
            object actual = runtime.EvalString("(nor 1 2 3)");
            Assert.AreEqual(false, actual);

            actual = runtime.EvalString("(nor nil)");
            Assert.AreEqual(true, actual);

        }

        [Test]
        public void Uniq()
        {
            object actual1 = runtime.EvalString("(uniq)");
            object actual2 = runtime.EvalString("(uniq)");
            Assert.IsFalse(actual1 == actual2);

        }

        [Test]
        public void TypeOf()
        {
            object actual = runtime.EvalString("(typeof \"Int32\")");
            Assert.AreEqual(typeof(Int32), actual);

            actual = runtime.EvalString("(typeof \"Double\")");
            Assert.AreEqual(typeof(Double), actual);

        }

        [Test]
        public void Type()
        {
            object actual = runtime.EvalString("(type 1)");
            Assert.AreEqual(typeof(Int32), actual);

            actual = runtime.EvalString("(type 2.1)");
            Assert.AreEqual(typeof(Double), actual);

        }

        [Test]
        public void IsSeq()
        {
            Assert.AreEqual(false, runtime.EvalString("(seq? 1)"));
            Assert.AreEqual(true, runtime.EvalString("(seq? '( 1 2 3))"));
            Assert.AreEqual(false, runtime.EvalString("(seq? nil)"));
            Assert.AreEqual(true, runtime.EvalString("(seq? \"12\")"));
        }

        [Test]
        public void IsAtom()
        {

            Assert.AreEqual(true, runtime.EvalString("(atom? 1)"));
            Assert.AreEqual(true, runtime.EvalString("(atom? nil)"));
            Assert.AreEqual(false, runtime.EvalString("(atom? '( 1 2 3))"));
        }

        [Test]
        public void IdFn()
        {
            Assert.AreEqual(1, runtime.EvalString("(idfn 1)"));
        }

        [Test]
        public void And()
        {

            Assert.AreEqual(true, runtime.EvalString("(and)"));
            Assert.AreEqual(true, runtime.EvalString("(apply and nil)"));
            Assert.AreEqual(true, runtime.EvalString("(and 1)"));
            Assert.AreEqual(false, runtime.EvalString("(and 1 nil)"));
        }


        [Test]
        public void Or()
        {

            Assert.AreEqual(false, runtime.EvalString("(or)"));
            Assert.AreEqual(false, runtime.EvalString("(apply or nil)"));
            Assert.AreEqual(true, runtime.EvalString("(or 1)"));
            Assert.AreEqual(true, runtime.EvalString("(or 1 nil)"));
        }

   

        [Test]
        public void With()
        {

            Assert.AreEqual(6, runtime.EvalString("(with (a 1 b 2 c 3) (+ a b c))"));

        }





        [Test]
        public void Pr()
        {

            Runtime.PrintToString(runtime.EvalString("(pr 1)"));

        }

        [Test]
        public void Prn()
        {

            Runtime.PrintToString(runtime.EvalString("(prn 1)"));

        }

        [Test]
        public void Help()
        {

            Runtime.PrintToString(runtime.EvalString("(help help)"));

        }

        [Test]
        public void Sqrt()
        {

            Assert.AreEqual(5, runtime.EvalString("(sqrt 25)"));
            Assert.AreEqual(5.0, runtime.EvalString("(sqrt 25.0)"));

        }

        [Test]
        public void Range()
        {

            Assert.AreEqual("(1 2 3 4 5 6 7 8 9)", Runtime.PrintToString(runtime.EvalString("(range 1 10)")));
            Assert.AreEqual("(1 3 5 7 9)", Runtime.PrintToString(runtime.EvalString("(range 1 10 2)")));

        }

        [Test]
        public void Reverse()
        {

            Assert.AreEqual("(1 2 3 4 5 6 7 8 9)", Runtime.PrintToString(runtime.EvalString("(reverse '(9 8 7 6 5 4 3 2 1))")));

        }

        [Test]
        public void Reference()
        {

    
            string s = Runtime.PrintToString(runtime.EvalString("(reference \"System.Xml\")"));

            Assert.IsTrue(s.StartsWith("System.Xml"));

        }

        [Test]
        public void Using()
        {

            Assert.AreEqual("System.Xml", runtime.EvalString("(using \"System.Xml\")"));
        }

        [Test]
        public void Seq()
        {

            Assert.IsInstanceOfType (typeof(ISequence), runtime.EvalString("(seq \"hello\")"));
            Assert.IsInstanceOfType(typeof(ISequence), runtime.EvalString("(seq '(1 2 3))"));
            Assert.IsInstanceOfType(typeof(ISequence), runtime.EvalString("(seq [1 2 3])"));

        }

        [Test]
        public void Set()
        {

            runtime.EvalString("(set aaaaaa 1)");

            Assert.AreEqual(1, runtime.EvalString("aaaaaa"));
           

        }

        [Test]
        public void Map()
        {
            Assert.AreEqual(null, runtime.EvalString("(map idfn nil)"));
            Assert.AreEqual("(1 2 3)", Runtime.PrintToString(runtime.EvalString("(map idfn '(1 2 3))")));
        }

        [Test]
        public void ToArray()
        {

            Assert.IsInstanceOfType(typeof(Array), runtime.EvalString("(toarray \"hello\")"));
            Assert.IsInstanceOfType(typeof(Array), runtime.EvalString("(toarray '(1 2 3))"));
            Assert.IsInstanceOfType(typeof(Array), runtime.EvalString("(toarray [1 2 3])"));

        }

        [Test]
        public void ToList()
        {

            Assert.IsInstanceOfType(typeof(Pair), runtime.EvalString("(tolist \"hello\")"));
            Assert.AreEqual ("(1 2 3)", Runtime.PrintToString( runtime.EvalString("(tolist '(1 2 3))")));
            Assert.IsInstanceOfType(typeof(Pair), runtime.EvalString("(tolist [1 2 3])"));

        }

        [Test]
        public void Cdr()
        {
            Assert.AreEqual(null, runtime.EvalString("(cdr nil)"));
            Assert.AreEqual("(2 3)", Runtime.PrintToString(runtime.EvalString("(cdr '(1 2 3))")));
        }

        [Test]
        public void Some()
        {
            Assert.AreEqual(true, runtime.EvalString("(some? 1 '(1 2 3))"));
            Assert.AreEqual(false, runtime.EvalString("(some? 2 '(1 1 a a))"));
        }

        [Test]
        public void Every()
        {
            Assert.AreEqual(true, runtime.EvalString("(every? 1 '(1 1 1))"));
            Assert.AreEqual(false, runtime.EvalString("(every? 2 '(1 1 a a))"));
        }


        [Test]
        public void CallDotNet()
        {
            Assert.AreEqual(21, runtime.EvalString("(Math.Abs -21)"));
            runtime.EvalString("(Console.writeline \"Hello World\")");
            runtime.EvalString("(.hour (DateTime.Now))");
        }

        [Test]
        public void Coerce()
        {
            Assert.AreEqual(6, runtime.EvalString("(coerce 5.6 \"Int32\")"));
            Assert.AreEqual(5.0, runtime.EvalString("(coerce 5 \"Double\")"));
        }


        [Test]
        public void Compile()
        {
            Assert.IsInstanceOfType(typeof (Delegate), runtime.EvalString("(compile 1)"));

        }

        [Test]
        public void Null()
        {
            Assert.AreEqual(null, runtime.EvalString("null"));
            Assert.AreEqual(null, runtime.EvalString("nil"));

        }

        [Test]
        public void Booleans()
        {
            Assert.AreEqual(false, runtime.EvalString("false"));
            Assert.AreEqual(true, runtime.EvalString("true"));
            Assert.AreEqual(true, runtime.EvalString("t"));

        }

        [Test]
        public void Car()
        {
            Assert.AreEqual(null, runtime.EvalString("(car nil)"));
            Assert.AreEqual('h', runtime.EvalString("(car \"hello\")"));
        }

        [Test]
        public void Caar()
        {
            Assert.AreEqual(1, runtime.EvalString("(caar '((1 2) 3 4))"));
        }

        [Test]
        public void Cadr()
        {
            Assert.AreEqual(2, runtime.EvalString("(cadr '(1 2 3))"));
        }

        [Test]
        public void Cddr()
        {
            Assert.AreEqual("(3)", Runtime.PrintToString( runtime.EvalString("(cddr '(1 2 3))")));
        }

        [Test]
        public void Bound()
        {

            Assert.AreEqual(false, runtime.EvalString("(bound 'a)"));
            Assert.AreEqual(true, runtime.EvalString("(bound 'bound)"));

        }

        [Test]
        public void Def()
        {

            Assert.IsInstanceOfType(typeof(Function), runtime.EvalString("(def foo (x) \"Blah\" (+ x 1))"));

        }

        [Test]
        public void Mac()
        {

            Assert.IsInstanceOfType(typeof(Macro), runtime.EvalString("(mac foo (x) \"Blah\" (+ x 1))"));

        }

        [Test]
        public void Str()
        {

            Assert.AreEqual("123", runtime.EvalString("(str 1 2 3)"));

        }

        [Test]
        public void Streams()
        {

            Assert.AreEqual(System.Console.In, runtime.EvalString("(stdin)"));
            Assert.AreEqual(System.Console.Out, runtime.EvalString("(stdout)"));
            Assert.AreEqual(System.Console.Error, runtime.EvalString("(stderr)"));
        }

        [Test]
        public void Cons()
        {
            Assert.AreEqual("(1 2 3)", Runtime.PrintToString( runtime.EvalString("(cons 1 '( 2 3))")));
            Assert.AreEqual("1hello", runtime.EvalString("(str (cons 1 \"hello\"))"));

        }

        [Test]
        public void Do()
        {
            Assert.AreEqual(3, runtime.EvalString("(do 1 2 3)"));
        }

        [Test]
        public void Do1()
        {
            Assert.AreEqual(1, runtime.EvalString("(do1 1 2 3)"));
        }


        [Test]
        public void IsEmpty()
        {
            Assert.AreEqual(true, runtime.EvalString("(empty? nil)"));
            Assert.AreEqual(true, runtime.EvalString("(empty? \"\")"));
            Assert.AreEqual(false, runtime.EvalString("(empty? \"1\")"));
        
        }

        [Test]
        public void IsMember()
        {
            Assert.AreEqual(false, runtime.EvalString("(member? 1 nil)"));
            Assert.AreEqual(true, runtime.EvalString("(member? 1 '(1 2 3))"));
            Assert.AreEqual(false, runtime.EvalString("(member? 'a '(1 2 3))"));

        }

        [Test]
        public void Is()
        {
            Assert.AreEqual(true, runtime.EvalString("(is 1 1)"));
            Assert.AreEqual(false, runtime.EvalString("(is 1 2)"));
        }

        [Test]
        public void Eval()
        {
            Assert.AreEqual(3, runtime.EvalString("(eval '(+ 1 2))"));

        }

        [Test]
        public void Err()
        {
            try
            {
                runtime.EvalString("(err \"Hello\")");
            } catch (Exception e) 
            {
                Assert.AreEqual("Hello", e.Message);
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void When()
        {
            Assert.AreEqual(2, runtime.EvalString("(when (is 1 1) 2)"));
        }

        

        [Test]
        public void While()
        {
            Assert.AreEqual(null, runtime.EvalString("(while (is 1 2) 2)"));

            Assert.AreEqual(false, runtime.EvalString("(let a true (while a (set a false)))"));
        }

        [Test]
        public void For()
        {
            Assert.AreEqual(10, runtime.EvalString("(for i 1 10 (prn i))"));

        }

        [Test]
        public void Each()
        {
            Assert.AreEqual(3, runtime.EvalString("(each x '(1 2 3) (prn x))"));
        }

        [Test]
        public void Unless()
        {
            Assert.AreEqual(3, runtime.EvalString("(unless (is 1 2) 3)"));
        }

        [Test]
        public void Iso()
        {

            Assert.AreEqual(true, runtime.EvalString("(iso '( 1 2 3) '(1 2 3))"));
            Assert.AreEqual(false, runtime.EvalString("(iso '( 1 2 3) '(1 2 3 4))"));
            Assert.AreEqual(false, runtime.EvalString("(iso 1 2)"));
            Assert.AreEqual(true, runtime.EvalString("(iso 1 1)"));
            Assert.AreEqual(true, runtime.EvalString("(iso \"hello\" \"hello\")"));

        }

        [Test]
        public void Pair()
        {

            Assert.AreEqual("((1 2) (3 4) (5 6) (7 8))",Runtime.PrintToString( runtime.EvalString("(pair '(1 2 3 4 5 6 7 8))")));


        }

        [Test]
        public void Reduce()
        {

            Assert.AreEqual(36, runtime.EvalString("(reduce + '(1 2 3 4 5 6 7 8))"));


        }

        
    }


}
