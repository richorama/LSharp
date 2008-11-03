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
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Collections.Generic;




namespace LSharp
{
    /// <summary>
    /// An adapter which allows event handlers to be written in L Sharp.
    /// Events are handled by System.EventHandler delegate, but are then passed
    /// onto a generated class that translates the event args into a Cons that
    /// can be consumed by a L Sharp Closure.
    /// </summary>
    public class EventAdapter
    {
        // The binding classes this class will generate will reside within this module that belongs to the
        // assembly.
        
        private static AssemblyBuilder MyAssembly;
        private static ModuleBuilder MyModule;


        // A simple mapping that will keep track of the classes that we generate for different event types.

        private static Dictionary<Type, Type> EventHandlerMap = new Dictionary<Type, Type>();

        
        /// <summary>
        /// Adds clsoure as an event handler for eventName on the target object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="eventName"></param>
        /// <param name="closure"></param>
        /// <returns></returns>
        public static EventInfo AddEventHandler(object target, string eventName, Closure closure)
        {
            // Check for the requried event info.

            EventInfo theEvent = target.GetType().GetEvent(eventName, BindingFlags.IgnoreCase
                | BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic);

            if (theEvent == null)
            {
                throw new LSharpException(string.Format("No event {0} for {1}", eventName, target));
            }


            // Now, check to see if we need to construct a new class to handle this event binding.
            // If we already have an appropriate class just use that.

            Type bindingClass = null;


            if (EventHandlerMap.ContainsKey(theEvent.EventHandlerType) == false)
            {
                bindingClass = CreateNewBindingClass(theEvent);
                EventHandlerMap[theEvent.EventHandlerType] = bindingClass;
            }
            else
            {
                bindingClass = EventHandlerMap[theEvent.EventHandlerType];
            }


            // Now that we have the binding class worked out, create a new object and create a delegate
            // that's bound to that new object.

            Object bindingObject = Activator.CreateInstance(bindingClass,
                                                            new object[] { closure });

            Delegate newDelegate = Delegate.CreateDelegate(theEvent.EventHandlerType,
                                                           bindingObject,
                                                           bindingClass.GetMethod("EventHandler"));


            // Finally add the delegate to the event's list and return the found event info to the caller.

            theEvent.AddEventHandler(target, newDelegate);

            return theEvent;
        }



        /// <summary>
        /// Create a new class that can act as a binding between the specified event and a LSharp closure.
        /// </summary>
        /// <param name="typeModule">Build the new class in this module builder.</param>
        /// <param name="theEvent">Information about the event to bind to.</param>
        /// <returns>The typeinfo for the newly constructed class.</returns>
        private static Type CreateNewBindingClass(EventInfo theEvent)
        {
            // First make sure that we have an assembly to build our binding classes in.
            if (MyAssembly == null)
            {
                MyAssembly = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName("DynamicEvents"),
                                                                      AssemblyBuilderAccess.Run);

                MyModule = MyAssembly.DefineDynamicModule("DynamicModule", true);
            }


            // Define the new class and give it a closure as a private member variable.

            TypeBuilder newClass = MyModule.DefineType(theEvent.Name + "ClosureBindingClass",
                                                       TypeAttributes.Public | TypeAttributes.Sealed);


            FieldBuilder closureField = newClass.DefineField("theClosure",
                                                             typeof(Closure),
                                                             FieldAttributes.Private);


            // Create a constructor for this class that can take a closure in order to init our member variable.

            ConstructorBuilder constructor = newClass.DefineConstructor(MethodAttributes.Public,
                                                                        CallingConventions.Standard,
                                                                        new Type[] { typeof(Closure) });
            ILGenerator conIL = constructor.GetILGenerator();

            conIL.Emit(OpCodes.Ldarg_0);
            conIL.Emit(OpCodes.Ldarg_1);
            conIL.Emit(OpCodes.Stfld, closureField);
            conIL.Emit(OpCodes.Ret);


            // Now create the event handler method.  It's job is to take the event parameters and convert them
            // into a Cons consumable by the Closure.

            // First grab the parameter/return information from the event object information.

            MethodInfo delegateInvoke = theEvent.EventHandlerType.GetMethod("Invoke");
            ParameterInfo[] invokeParams = delegateInvoke.GetParameters();

            Type returnType = delegateInvoke.ReturnType;
            Type[] parameterTypes = new Type[invokeParams.Length];


            // Copy the param info a type array useable by the classbuilder's define method.

            for (int i = 0; i < invokeParams.Length; i++)
            {
                parameterTypes[i] = invokeParams[i].ParameterType;
            }


            // Now, define the method attached to our new class definition.

            MethodBuilder eventHandler = newClass.DefineMethod("EventHandler",
                                                               MethodAttributes.Public,
                                                               returnType,
                                                               parameterTypes);

            ILGenerator evtIL = eventHandler.GetILGenerator();


            // Push the member variable onto the stack.

            evtIL.Emit(OpCodes.Ldarg_0);
            evtIL.Emit(OpCodes.Ldfld, closureField);


            // Now load each event parameter onto the stack for later use.  If the type is a native one,
            // then make sure to box it up.

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                evtIL.Emit(OpCodes.Ldarg, i + 1);

                if (parameterTypes[i].IsClass == false)
                {
                    evtIL.Emit(OpCodes.Box, parameterTypes[i]);
                }
            }


            // Now construct insances of the cons object, in reverse order.  Each one will consume one
            // of the parameters we previously puched on.

            if (parameterTypes.Length > 0)
            {
                evtIL.Emit(OpCodes.Newobj, typeof(Cons).GetConstructor(new Type[] { typeof(object) }));

                for (int i = 1; i < parameterTypes.Length; i++)
                {
                    evtIL.Emit(OpCodes.Newobj,
                               typeof(Cons).GetConstructor(new Type[] { typeof(object), typeof(object) }));
                }
            }
            else
            {
                evtIL.Emit(OpCodes.Ldnull);
            }


            // Now that the list has been constructed, call the closure's invoke method.  We pushed the
            // closure onto the stack earlier.

            evtIL.Emit(OpCodes.Callvirt, typeof(Closure).GetMethod("Invoke",
                                                                   new Type[] { typeof(Cons) }));


            // If the caller doesn't expect a value from the event handler, then just drop it.  Otherwise
            // translate the return value into something the caller will expect.

            if (returnType == typeof(void))
            {
                evtIL.Emit(OpCodes.Pop);
            }
            else if (returnType.IsClass == false)
            {
                evtIL.Emit(OpCodes.Unbox_Any, returnType);
            }
            else if (returnType != typeof(object))
            {
                evtIL.Emit(OpCodes.Castclass, returnType);
            }


            // Finally return to the caller.

            evtIL.Emit(OpCodes.Ret);


            // Turn this class builder into a working type and return the type info to the caller.

            return newClass.CreateType();
        }
    }
}


