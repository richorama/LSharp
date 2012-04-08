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

using LSharp;

namespace LSharpConsole
{
    class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// [STAThread]
        static void Main(string[] args)
        {

            Runtime runtime = new Runtime(System.Console.In, System.Console.Out, System.Console.Error);
           
            if (args.Length < 1)
            {
                // Interactive Read, Eval, Print Loop
                runtime.Repl();
            }
            else
            {
                // Batch mode
                string filename = args[0];

                // Windows uses backslash as directory separator, so
                // we must escape it
                filename = filename.Replace("\\", "\\\\");

                runtime.Load(filename);

            }
        }
    }
}
