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

namespace LSharp
{
    /// <summary>
    /// Encapsulates an array and treats it as a sequence
    /// </summary>
    public class ArraySequence : Sequence
    {
        Array array;
        int index = 0;

        public ArraySequence(Array array)
        {
            this.array = array;
        }

        private ArraySequence(Array array, int index)
        {
            this.array = array;
            this.index = index;
        }

        public override object First()
        {
            if (index < array.Length)
                return array.GetValue(index);

            return null;
        }

        public override ISequence Rest()
        {
            if (index + 1 < array.Length)
                return new ArraySequence(array, index + 1);

            return null;

        }

        public override ISequence Cons(object item)
        {
            // Sequences are immutable, so create a new array
            Array newArray = Array.CreateInstance(array.GetValue(0).GetType(), array.Length + 1);

            // Copy the existing items
            array.CopyTo(newArray, 1);

            // Add the new item
            newArray.SetValue(item, 0);

            // Return the new copy
            return new ArraySequence(newArray);
        }

        public override int Length()
        {
            return (array.Length - index);
        }

        public override object Nth(int n)
        {
            return array.GetValue(index + n);
        }

    }
}
