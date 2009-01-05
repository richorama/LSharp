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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace LSharp
{
    /// <summary>
    /// Utilities for interoperating with the Common Language Runtime
    /// </summary>
    public class ClrGlue
    {

        /// <summary>
        /// Searches the list of namespaces for the specified type
        /// </summary>
        public static Type FindType(string typeName, List<string> namespaces)
        {
            foreach (string s in namespaces)
            {
                Type type = FindType(s + "." + typeName);

                if (type != null)
                    return type; // Found it !
            }

            return null;
        }

        /// <summary>
        /// Searches the currently loaded assemblies for a type with the given name
        /// </summary>
        public static Type FindType(string typeName)
        {
            #if SILVERLIGHT
                List<Assembly> assemblies = new List<Assembly>();
			    assemblies.Add(Assembly.Load("mscorlib"));
			    assemblies.Add(Assembly.GetCallingAssembly());
            #else
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            #endif

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(typeName, false);

                if (type != null)
                    return type;

            }

            return null;
        }

        public static object CallStaticMethod(string methodName, Type type, params object[] parameters)
        {
            BindingFlags bindingFlags = BindingFlags.IgnoreCase
                | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;

            Type[] types = new Type[parameters.Length];

            int i = 0;
            foreach (object argument in parameters)
            {
                if (argument == null)
                    types[i] = typeof(System.Object);
                else
                    types[i] = argument.GetType();
                i++;
            }

            // Start by looking for a method call
            MethodInfo m = type.GetMethod(methodName,
                        bindingFlags | BindingFlags.InvokeMethod
                        , null, types, null);
            if (m != null)
                try
                {
                    return m.Invoke(type, parameters);
                }
                catch (Exception e)
                {
                    throw new LSharpException(e.InnerException.ToString());
                }

            // Now loook for a property get
            PropertyInfo p = type.GetProperty(methodName, bindingFlags | BindingFlags.GetProperty,
                null, null, types, null);
            if (p != null)
                return p.GetGetMethod().Invoke(type, parameters);

            // Now look for a field get
            FieldInfo f = type.GetField(methodName, bindingFlags | BindingFlags.GetField);
            if (f != null)
                return f.GetValue(type);

            // TODO: or an event ?

            throw new LSharpException(string.Format("No such method, property or field {0} on {1}", methodName, type));

        }

        /// <summary>
        /// Calls the given method on the given object with given arguments
        /// </summary>
        public static object CallMethod(string method, object o, params object[] parameters) 
        {
            BindingFlags bindingFlags = BindingFlags.IgnoreCase
                | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance;

            Type type = o.GetType();

            Type[] types = new Type[parameters.Length];

            int i = 0;
            foreach (object argument in parameters)
            {
                if (argument == null)
                    types[i] = typeof(System.Object);
                else
                    types[i] = argument.GetType();
                i++;
            }

            // Start by looking for a method call
            MethodInfo m = type.GetMethod(method.ToString(),
                        bindingFlags | BindingFlags.InvokeMethod
                        , null, types, null);
            if (m != null)
                return m.Invoke(o, parameters);

            // Now loook for a property get
            PropertyInfo p = type.GetProperty(method.ToString(), bindingFlags | BindingFlags.GetProperty,
                null, null, types, null);
            if (p != null)
                return p.GetGetMethod().Invoke(o, parameters);

            // Now look for a field get
            FieldInfo f = type.GetField(method.ToString(), bindingFlags | BindingFlags.GetField);
            if (f != null)
                return f.GetValue(o);


            // or an event ?

            throw new LSharpException(string.Format("No such method, property or field {0} on {1}", method.ToString(), type));

        }

        /// <summary>
        /// Looks for a given member on a type. If it can be identified unambiguously
        /// return it, otherwise return null.
        /// N.B. LSharp is a dynamic language, so we don know the types of the arguments
        /// at rntime - just how many there are.
        /// </summary>
        public static MemberInfo FindUnambiguousMember(string methodName, Type type, int argN)
        {
            MemberInfo result = null;
            MemberInfo[] memberInfos = type.GetMembers();

            methodName = methodName.ToLower();

            foreach (MemberInfo m in memberInfos)
            {
                if (m.Name.ToLower() == methodName)
                {
                    if (m is MethodInfo)
                    {
                        // TODO: if virtual then arGN will be 1 too long!
                        if (((MethodInfo)m).GetParameters().Length == argN)
                        {
                            if (result == null)
                                result = m;
                            else
                                return null;
                        }
                    } else if (result == null)
                            result = m;
                        else
                            return null;
                }
            }

            return result;
        }




        private static ConstructorInfo GetConstructor(Type type, Type[] types, object[] values)
        {

            ConstructorInfo constructorInfo = type.GetConstructor(types);

            if (constructorInfo == null)
            {

                ConstructorInfo[] constructorInfos = type.GetConstructors();
                foreach (ConstructorInfo c in constructorInfos)
                {
                    ParameterInfo[] parameterInfos = c.GetParameters();

                    if (parameterInfos.Length == types.Length)
                    {

                        int i = 0;
                        bool congruent = true;
                        foreach (ParameterInfo p in parameterInfos)
                        {
                            congruent = ((p.ParameterType == types[i]) || (values[i] == null));
                            i++;
                        }
                        if (congruent)
                            return c;
                    }
                }

            }

            return constructorInfo;
        }

        public static object MakeInstance(Type type, object[] arguments)
        {
            Type[] types = new Type[arguments.Length];

            int i = 0;
            foreach (object argument in arguments)
            {
                if (argument == null)
                    types[i] = typeof(System.Object);
                else
                    types[i] = argument.GetType();
                i++;
            }

            ConstructorInfo constructorInfo = GetConstructor(type, types, arguments);

            if (constructorInfo == null)
                // TODO: Look for other potential constructors where null can match any type
                throw new LSharpException(string.Format("No such constructor for {0}", type));

            return constructorInfo.Invoke(arguments);
        }

        public static string EnvironmentVersion()
        {
            #if SILVERLIGHT
                return System.Environment.Version + " (Silverlight)";
            #else
                return System.Environment.Version.ToString();
            #endif
        }

        public static string LSharpVersion () 
        {

            return new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();
        }

        /// <summary>
        /// Loads the assembly with the given name
        /// </summary>
        public static Assembly LoadAssembly(string name)
        {
            Assembly assembly;

            if (Path.IsPathRooted(name))
                assembly = Assembly.LoadFrom(name);
            else 
            {

                #if SILVERLIGHT
                assembly = Assembly.LoadFrom(name);
                #else
                // LoadWithPartialName is deprecated, but it's too useful not to use ...
                assembly = Assembly.LoadWithPartialName(name);
                #endif

            }

            return assembly;
        }


        /// <summary>
        /// Reflects over all the fields of an object
        /// </summary>
        public static string Fields(Object o)
        {
            StringBuilder stringBuilder = new StringBuilder();
            FieldInfo[] fieldInfos = o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                stringBuilder.AppendFormat("\t{0} {1} = {2}\n", fieldInfo.FieldType.Name, fieldInfo.Name, Runtime.PrintToString( fieldInfo.GetValue(o)));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Reflects over a type object to create a textual description 
        /// of its declaration
        /// </summary>
        public static string TypeInfo(Type type)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (type.IsPublic) stringBuilder.Append("public ");
            if (type.IsAbstract) stringBuilder.Append("abstract ");
            if (type.IsByRef) stringBuilder.Append("byref ");
            if (type.IsSealed) stringBuilder.Append("sealed ");
            if (type.IsImport) stringBuilder.Append("import ");
            if (type.IsClass) stringBuilder.Append("class ");
            if (type.IsArray) stringBuilder.Append("array ");
            if (type.IsCOMObject) stringBuilder.Append("comobject ");
            if (type.IsEnum) stringBuilder.Append("enum ");
            if (type.IsInterface) stringBuilder.Append("interface ");
            if (type.IsPointer) stringBuilder.Append("pointer ");
            stringBuilder.AppendFormat("{0}\n", type.Name);

            return stringBuilder.ToString();
        }
    }
}
