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
using System.Reflection;
using System.Collections;

namespace LSharp
{
    /// <summary>
    /// An adapter which allows event handlers to be written in L Sharp.
    /// Events are handled by System.EventHandler delegate, but then passed 
    /// onto an LSharp closure through an instance of this adapter which wraps
    /// that closure.
    /// </summary>
    public class EventAdapter
    {
        // Keep a cache of event adapters so that we can reuse them 
        // on re definition of event handlers
        private const int CAPACITY = 500;
        private static Hashtable eventAdapterTable = new Hashtable(CAPACITY);

        // The closure that will be called to handle the event that this
        // adapter is defined for.
        private Closure closure;

        /// <summary>
        /// Creates a new EventAdapter with the specified closure as the 
        /// underlying handler.
        /// </summary>
        /// <param name="closure"></param>
        private EventAdapter(Closure closure)
        {
            this.closure = closure;
        }

        /// <summary>
        /// The underlying closure that gets called to handle the event.
        /// </summary>
        public Closure Closure {
            get { return closure; }
            set { closure = value;}
        }

        /// <summary>
        /// Handles the event by invoking the closure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleEvent(object sender, EventArgs e)
        {
            closure.Invoke(new Cons(sender, new Cons(e)));
        }

        /// <summary>
        /// Adds closure as an event handler for eventName on the target object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="eventName"></param>
        /// <param name="closure"></param>
        /// <returns></returns>
        public static EventAdapter AddEventHandler(object target, string eventName, Closure closure)
        {
            // Create a unique key for this target and event combination
            string key = target.GetHashCode() + eventName.ToLower();

            // If there is an existing adapter ..
            EventAdapter eventAdapter = (EventAdapter)eventAdapterTable[key];
            if (eventAdapter != null)
            {
                // The handler is being redefined. All the plumbing is already in place, 
                // so just update the closure which will handle the event.
                eventAdapter.Closure = closure;
                return eventAdapter;
            }
            else
            {
                // Create a new EventAdapter for the supplied closure
                eventAdapter = new EventAdapter(closure);
                eventAdapterTable.Add(key, eventAdapter);

                // Create an EventHandler which will call this adapter
                EventHandler eventHandler = new EventHandler(eventAdapter.HandleEvent);
                
                // Get a list of all available events for target
                EventInfo[] eventInfos = target.GetType().GetEvents();

                // Search for eventName
                foreach (EventInfo eventInfo in eventInfos)
                {
                    // When we find eventName, wire up a new handler through this adapter
                    if (eventInfo.Name.ToLower() == eventName.ToLower())
                    {
                        eventInfo.AddEventHandler(target, eventHandler);
                        return eventAdapter;
                    }
                }

                throw new LSharpException(string.Format("No such event {0} for {1}", eventName, target));
            }

        }
    }
}

