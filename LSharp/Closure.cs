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

namespace LSharp
{
    /// <summary>
    /// A closure is a function defined within a captured environment
    /// </summary>
    public class Closure
    {
        private object parameters;
        private object body;
        private Environment environment;
        
        public Closure(object parameters, object body, Environment environment)
        {
            this.body = body;
            this.environment = environment;
            this.parameters = parameters;
            // TODO: Compile optional args here
        }

        public object Invoke(object[] arguments)
        {
            // Create a new lexical environment
            Environment localEnvironment = new Environment(environment);

            // Instantiate parameters with arguments in the new environment
            ProcessArguments(parameters, arguments, localEnvironment);

            return Compiler.Eval(body, localEnvironment);

        }

        public void ProcessArguments(object parameters, object[] arguments, Environment localEnvironment)
        {
            // TODO: Better checking for congruent arguments, better error handling
 
            if (parameters == null)
                return; // No parameters so nothing to do

            if (parameters is Symbol)
            {
                localEnvironment.AssignLocal((Symbol)parameters, Runtime.FromArray(arguments, 0));
                return;
            }

            
            int i = 0;

            ISequence parameterSequence = Runtime.Seq(parameters);

            while (parameterSequence != null)
            {

                object parameter = parameterSequence.First();

                if (parameter is Symbol)
                {
                    if (parameter == Symbol.FromName("&"))
                    {
                        // The next parameter is a rest parameter and receives all the
                        // remaining arguments

                        object rest = Runtime.FromArray(arguments, i);
                        localEnvironment.AssignLocal((Symbol)parameterSequence.Rest().First(), rest);
                        parameterSequence = null;
                    }
                    else
                    {
                        // It's a required parameter

                        localEnvironment.AssignLocal((Symbol)parameter, arguments[i]);
                        parameterSequence = parameterSequence.Rest();
                        i++;
                    }
                }

                else if (parameter is ISequence)
                {
                    // It's an optional parameter

                    Symbol name = (Symbol)((ISequence)parameter).First();
                    object expr = ((ISequence)parameter).Rest().First();

                    if (i < arguments.Length)
                    {
                        // The optional argument is present
                        localEnvironment.AssignLocal(name, arguments[i]);
                        i++;
                    }
                    else
                    {
                        // The optional argument is missing

                        // TODO: Optional argument defaults really should have been compiled by now
                        object o = Compiler.Eval(expr, localEnvironment);
                        
                        localEnvironment.AssignLocal(name, o);

                        
                    }
                    parameterSequence = parameterSequence.Rest();
                }
            }     
        }
    }
}
