(= a 1)

(and
	(eql 6 (eval '(+ 1 2 3)))
	(eql 1 (eval 1))
	(eql 2.08 (eval 2.08))
	(eql #\c (eval #\c))
	(eq System.Console (eval 'System.Console))
	(eql 1 (eval 'a))
	(eql "string" (eval "string")))