
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
using System.Text;

namespace LSharp
{
    /// <summary>
    /// A base class for sequences
    /// </summary>
    public abstract class Sequence : ISequence
    {
        public abstract object First();
        public abstract ISequence Rest();
        public abstract ISequence Cons(object o);

        public virtual IEnumerator GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        public virtual object Last()
        {
            ISequence sequence = this;
            while (sequence.Rest() != null)
            {
                sequence = sequence.Rest();
            }

            return sequence.First();
        }

        public virtual int Length() 
        {
            int length = 1;

            for (ISequence sequence = Rest(); sequence != null; sequence = sequence.Rest())
                length++;

            return length;
        }

        public virtual object Second()
        {
            return Rest().First();
        }

        public virtual object Third()
        {
            return Rest().Rest().First();
        }

        public virtual object Fourth()
        {
            return Rest().Rest().Rest().First();
        }

        public virtual ISequence Cddr()
        {
            return Rest().Rest();
        }

        public virtual ISequence Cdddr()
        {
            return Rest().Rest().Rest();
        }

        public virtual object Nth(int n)
        {
            if (n == 0)
                return First();

            ISequence seq = Rest();

            for (int i = 1; i < n; i++)
            {
                seq = seq.Rest();
            }

            return seq.First();
        }

        public virtual bool IsMember(object o)
        {
            ISequence seq = this;

            while (seq != null)
            {
                if (seq.First().Equals(o))
                    return true;

                seq = seq.Rest();
            }

            return false;
        }
        public static ISequence Join(ISequence a, ISequence b)
        {
            ISequence list = b;

            ISequence o = Reverse(a);
            while (o != null)
            {
                list = list.Cons(o.First());
                o = o.Rest();
            }
            return list;
        }

        public virtual ISequence Join(ISequence s)
        {
            return Join(this, s);
        }  

        public static ISequence Reverse(ISequence s)
        {
            Pair list = null;
            ISequence o = s;
            while (o != null)
            {
                list = new Pair(o.First(), list);
                o = o.Rest();
            }
            return list;
        }

        public virtual ISequence Reverse()
        {
            return Reverse(this);
        }

        public virtual bool IsEmpty()
        {
            return (Length() == 0);
        }

        public virtual object[] ToArray()
        {
            object[] result = new Object[Length()];

            int i = 0;
            foreach (object o in this)
            {
                result[i++] = o;
            }
            return result;
        }

        public virtual Pair ToList()
        {
            Pair result = null;

            foreach (object o in this)
            {
                result = new Pair(o, result);
            }
            return (Pair)result.Reverse();
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            foreach (object o in this)
            {
                s.Append(o);
            }

            return s.ToString();

        }

        public virtual ISequence Map(Function f, Environment environment)
        {
            object[] result = new object[Length()];

            int i = 0;
            foreach (object o in this)
            {
                result[i++] = Compiler.FunCall(f, new object[] { o }, environment);
            }
            return new ArraySequence(result);
        }

        public virtual object Reduce(Function f, Environment environment)
        {
            object result = Compiler.FunCall(f, new object[] { First(), Second() }, environment);

            ISequence s = Rest().Rest();

            while (s != null)
            {
                result = Compiler.FunCall(f, new object[] { result, s.First() }, environment);
                s = s.Rest();
            }

            return result;
        }
    }
}
