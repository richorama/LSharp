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
    /// An LSharp function is really just a wrapper for a delegate, but this
    /// allows us to tag it with documentation and information about the 
    /// calling convention.
    /// </summary>
    public class Function
    {
        private Delegate implementation;
        private string signature;
        private string documentation;
        bool aggregateArgs; // Whether the delegate gets all its args together or individually
             
        public Function(Delegate implementation)
        {
            this.implementation = implementation;
        }

        public Function(Delegate implementation,
            string signature,
            string documentation,
            bool aggregateArgs)
        {
            this.implementation = implementation;
            this.signature = signature;
            this.documentation = documentation;
            this.aggregateArgs = aggregateArgs;
        }


        public string Signature
        {
            get { return signature; }
        }

        public string Documentation
        {
            get { return documentation; }
        }

        public virtual object Call(object[] args)
        {
            object result;

            try
            {
                if (aggregateArgs)
                    // Pass all the arguments together as an array
                    result = ((Delegate)implementation).DynamicInvoke(new object[] { args });
                else
                    // Pass the arguments individually
                    result = ((Delegate)implementation).DynamicInvoke(args);
            } 
            catch (System.Reflection.TargetInvocationException e) 
            {
                // The inner exception is the real problem ..
                throw e.InnerException;
            }

            return result;
        }
    }
}
