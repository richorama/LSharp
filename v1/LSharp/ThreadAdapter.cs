#region Copyright (C) 2006 Rob Blackwell & Active Web Solutions.
//
// L Sharp .NET, a powerful lisp-based scripting language for .NET.
// Copyright (C) 2006 Rob Blackwell & Active Web Solutions.
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
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LSharp
{
    /// <summary>
    /// ThreadAdapter allows LSharp expressions to be evaluated on
    /// a new thread.
    /// </summary>
    public class ThreadAdapter
    {
        private Environment environment;
        private Object expression;

        /// <summary>
        /// Creates a new ThreadAdapter that will evaluate expression
        /// within the given environment on a new Thread.
        /// TODO: Is environment thread safe ?
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="environment"></param>
        private ThreadAdapter(Object expression, Environment environment)
        {
            this.expression = expression;
            this.environment = environment;
        }

        /// <summary>
        /// Evalauates expression within the given environment on
        /// a new thread - Returns immediately, but the new thread 
        /// continues to execure expression asynchronously.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="environment"></param>
        /// <returns>The new Thread</returns>
        public static Thread Fork(Object expression, Environment environment, ApartmentState apartmentState)
        {
            ThreadAdapter threadAdapter = new ThreadAdapter(expression, environment);

            ThreadStart job = new ThreadStart(threadAdapter.Invoke);
            Thread thread = new Thread(job);
            thread.SetApartmentState(apartmentState);
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Evaluate the expression
        /// </summary>
        private void Invoke()
        {
            Runtime.Eval(expression, environment);
        }
    }
}
