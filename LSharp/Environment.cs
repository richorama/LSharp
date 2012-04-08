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

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LSharp
{
    public class Environment : IEnumerable
    {
        private const int CAPACITY = 10;
        private Dictionary<Symbol, object> hashtable = new Dictionary<Symbol, object>(CAPACITY);

        // Maintain a reference to a previous environment to allow nesting
        // of environments, thus supporting local variables in a lexical
        // scope
        private Environment previousEnvironment;

        public void GlobalReset()
        {
            hashtable = new Dictionary<Symbol, object>(CAPACITY);
        }

        public Environment()
        {
            AssignLocal(Symbol.FromName("environment"), this);
        }

        /// <summary>
        /// Creates a new environment which has access to a previous environment
        /// </summary>
        public Environment(Environment environment)
        {
            this.previousEnvironment = environment;
            AssignLocal(Symbol.FromName("environment"), this);
        }


        public object GetValue(Symbol symbol)
        {
            if (hashtable.ContainsKey(symbol))
            {
                return hashtable[symbol];
            }
            
            if (previousEnvironment != null)
            {
                return previousEnvironment.GetValue(symbol);
            }
            
            throw new LSharpException("Reference to undefined identifier: " + symbol.Name,this);
        }


        /// <summary>
        /// Determines whether the environment contains a definition for
        /// a variable with the given symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>True or false</returns>
        public bool Contains(Symbol symbol)
        {
            if (hashtable.ContainsKey(symbol))
                return true;

            if (previousEnvironment != null)
                return previousEnvironment.Contains(symbol);

            return false;

        }

        /// <summary>
        /// Returns the environment in which a given variable is defined, or null
        /// </summary>
        private Environment GetEnvironment(Symbol symbol)
        {
            if (hashtable.ContainsKey(symbol))
                return this;

            if (previousEnvironment == null)
                return null;

            return previousEnvironment.GetEnvironment(symbol);

        }

        /// <summary>
        /// Sets a variable with given symbol to a given value
        /// </summary>
        public object Set(Symbol symbol, object value)
        {
            if ((hashtable.ContainsKey(symbol)) || (previousEnvironment == null))
                return this.AssignLocal(symbol, value);

            
            return previousEnvironment.Set(symbol, value);
        }

        /// <summary>
        /// Asssigns value to a local variable symbol in this
        /// local environment (irrespective of whether symbol
        /// is defined in any parent environments).
        /// </summary>
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
            foreach (Symbol key in hashtable.Keys)
            {
                stringBuilder.AppendFormat("{0}:{1}\r\n", key.Name, hashtable[key]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Make environment enumerable so that they are convenient to use
        /// from within LSharp itself
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return hashtable.GetEnumerator();
        }

    }
}
