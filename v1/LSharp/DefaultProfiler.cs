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

namespace LSharp
{
	/// <summary>
	/// The DefaultProfiler does nothing except provide
	/// a placeholder implementation of IProfiler
	/// </summary>
	public class DefaultProfiler : IProfiler
	{

		public void TraceCall (object form) 
		{
		}

		public void Comment (string comment) 
		{
		}

		public object TraceReturn (object form) 
		{
			return form;
		}
		public void Close() 
		{
		}


	}
}
