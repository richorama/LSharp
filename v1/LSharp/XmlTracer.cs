#region Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
//
// L Sharp .NET, a powerful lisp-based scripting language for .NET.
// Copyright (C) 2005 Rob Blackwell & Active Web Solutions.
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
using System.IO;

namespace LSharp
{
	/// <summary>
	/// A profiler which traces LSharp programs and writes the trace to an XML file
	/// </summary>
	public class XmlTracer : IProfiler
	{
		private string filename;
		private static TextWriter traceWriter;

		public XmlTracer(string filename)
		{
			this.filename = filename;
			traceWriter = new StreamWriter(filename);
			traceWriter.WriteLine("<?xml version=\"1.0\"?>");
			traceWriter.WriteLine("<trace>");
		}

		public void Comment (string comment) 
		{
			traceWriter.WriteLine(String.Format("<comment>{0}</comment>",comment));
		}

		public void Close() 
		{
			traceWriter.WriteLine("</trace>");
			traceWriter.Close();
		}

		public void TraceCall (object form) 
		{
			traceWriter.WriteLine("<eval>");
			traceWriter.WriteLine("<call>{0}</call>",Printer.WriteToString(form));
		}

		public object TraceReturn (object form) 
		{
			traceWriter.WriteLine("<return>{0}</return>",Printer.WriteToString(form));
			traceWriter.WriteLine("</eval>");
				
			return form;
		}

		
	}
}
