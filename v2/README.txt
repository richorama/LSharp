L Sharp Version 2
-----------------

L Sharp .NET is a Lisp-like scripting language for .NET. It uses a 
modern Lisp dialect and integrates with the .NET  Framework which 
provides a rich set of libraries. 

It has a small, simple, extensible core that's coded in C# - The source 
code is easy to follow and you can easily add your own functions in C# 
or L#.

The language design is influenced by Paul Graham's Arc and Rich Hickey's
Clojure.

For more information see www.lsharp.org

Goals and Approach
------------------

Build a modern Lisp implementation for the .NET CLR which is good enough for
production work.

A small, clean, readable implementation that is a framework for experimentation
and can easily be understood and extended by other people.

A language that is fun to hack on.

Provide a scripting engine for .NET applications.

Dont be afraid to pinch good ideas from other languages.

An Open Source licence that wont stop Microsoft people using it (i.e. not GPL!)


Warnings
--------

I reserve the right to change the language at any time without maintaining
backwards compatibility.

Beware - This is alpha qality code which has not yet undergone significant
testing.

Getting Started
---------------

Sorry, there isn't much documentation at he moment!

The best way to get started is to compile the code yourself using Visual
Studio 2008.

Delving into the code, you'll find all the definitions for LSharp functions
and Macros in Runtime.cs

To get a feel for what the various functions and macros do, see the NUnit
test cases.

Evaluating (help command) Will bring up some basic documentation and info
about the arguments.

There are few simple sample apps in the Samples subdirectory.

Notes
-----

Symbols are case sensitive, so Foo is not the same as foo.

Lists are comprised of pairs (also know as Cons Cells). I've decided not 
to allow dotted pairs. The rest or cdr of a pair must be either another pair
or null. null signifies the end of the list.

All aggregate data structures - lists, arrays, strings can be treated as
sequences so first, rest and cons and a variety of sequence oriented functions
all work with all these data types.

At the REPL *1 is the result of the last evaluation and *2 the evaluation before that.
The last exception is *e.

Rob Blackwell
November 2008