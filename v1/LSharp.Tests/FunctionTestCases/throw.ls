
(try 
	(do
		(throw (new LSharpException "boo"))
		false)
	(eql (Message it) "boo"))