using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using LSharp;

namespace LSharpSilverlightRepl
{
	public static class RuntimeExtension
	{
		static Symbol starOne;
		static Symbol starTwo;
		static Symbol starThree;
		static Symbol lastException;

		public static void SilverlighReplInit(this Runtime runtime)
		{
			TextWriter stdout = runtime.StdOut();
			LSharp.Environment globalEnvironment = runtime.GlobalEnvironment;

			stdout.WriteLine("L Sharp {0} on {1}", ClrGlue.LSharpVersion(), ClrGlue.EnvironmentVersion());
			stdout.WriteLine("Copyright (c) Rob Blackwell. All rights reserved.");

			// Keep results of recent evaluations using shorthand *1, *2 and *3
			starOne = Symbol.FromName("*1");
			Runtime.VarSet(starOne, null, globalEnvironment);

			starTwo = Symbol.FromName("*2");
			Runtime.VarSet(starTwo, null, globalEnvironment);

			starThree = Symbol.FromName("*3");
			Runtime.VarSet(starThree, null, globalEnvironment);

			// Keep the last exception using the shorthand *e
			lastException = Symbol.FromName("*e");
			Runtime.VarSet(lastException, null, globalEnvironment);
		}

		public static void SilverlighRepl(this Runtime runtime, string input)
		{
			TextWriter stdout = runtime.StdOut();
			TextWriter stderr = runtime.StdErr();
			LSharp.Environment globalEnvironment = runtime.GlobalEnvironment;

			try
			{
				// Eval
				object output = runtime.EvalString(input);

				// Update recent evaluations
				Runtime.VarSet(starThree, Runtime.VarRef(starTwo, globalEnvironment), globalEnvironment);
				Runtime.VarSet(starTwo, Runtime.VarRef(starOne, globalEnvironment), globalEnvironment);
				Runtime.VarSet(starOne, output, globalEnvironment);

				// Print
				stdout.WriteLine(Runtime.PrintToString(output));

			}

			catch (Exception e)
			{
				stderr.WriteLine("Exception : {0}", e.Message);

				// Keep track of the exception in *e
				Runtime.VarSet(lastException, e, globalEnvironment);
			}
		}
	}
}
