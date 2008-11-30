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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LSharp
{
    /// <summary>
    /// Encapsulates any IEnumerable obejct and treats it as a sequence.
    /// This really serves as a default implementation - more specialised
    /// implemetations of IEnumerable can easily defined better performant
    /// sequence wrappers depending upon their particular features.
    /// </summary>
    public class EnumerableSequence : Sequence
    {
        private IEnumerable enumerable;
        private IEnumerator enumerator;
        int offset;

        public EnumerableSequence(IEnumerable enumerable) 
        {
            this.enumerable = enumerable;
            this.enumerator = enumerable.GetEnumerator();
            offset = 0;
            enumerator.MoveNext();
        }

        private EnumerableSequence(IEnumerable enumerable, IEnumerator emumerator, int offset)
        {
            this.enumerable = enumerable;
            this.enumerator = emumerator;
            this.offset = offset;
            enumerator.MoveNext();

            for (int i = 0; i < offset; i++)
            {
                enumerator.MoveNext();
            }
        }

        public override object First()
        {
            return enumerator.Current;
        }

        public override ISequence Rest()
        {
            if (enumerator.MoveNext())
                return new EnumerableSequence(enumerable, enumerable.GetEnumerator(), offset + 1);
            else
                return null;
        }

        public override ISequence Cons(object o)
        {
            throw new NotImplementedException();
        }


        public override System.Collections.IEnumerator GetEnumerator()
        {
            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i < offset; i++)
            {
                enumerator.MoveNext();
            }

            return enumerator;
        }

      
    }
}
