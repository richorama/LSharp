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
using System.Web;
using LSharp;
using System.Web.SessionState;

namespace LSharp.Web
{
    /// <summary>
    /// A custom HttpHandler which allows LSharp to be used for web application
    /// development.
    /// </summary>
    public class LSharpHttpHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// This handler is reusable
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Processes an HTTP Web request using the LSharp runtime. 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            // Create a new environment with access to the HttpContext
            Environment environment = new Environment();
            environment.Assign(Symbol.FromName("*context*"), context);
            environment.Assign(Symbol.FromName("*response*"), context.Response);
            environment.Assign(Symbol.FromName("*request*"), context.Request);
            environment.Assign(Symbol.FromName("*application*"), context.Application);
            environment.Assign(Symbol.FromName("*session*"), context.Session);

            // The url requested maps to a physical file which contains the
            // L Sharp code to be interpreted.
            string filename = context.Request.PhysicalPath;
            filename = filename.Replace("\\", "\\\\");

            // Load the code in the newly created context
            string expression = string.Format("(load \"{0}\")", filename);

            try
            {
                Runtime.EvalString(expression, environment);
            }
            catch (Exception e)
            {
                context.Response.Write("<br> <font color='red'>");
                context.Response.Write(e);
                context.Response.Write("</font>");
            }
        }  
    }
}
