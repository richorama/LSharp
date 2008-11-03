(try

(try
	(throw (new LSharpException "foo"))
	null
	(prl "finally"))
	
true)