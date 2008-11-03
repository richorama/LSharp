;; L Sharp test case for cdr
(and
	(eql '(b c) (cdr '(a b c)))

	(eql "ello" (cdr "hello"))
	
	;; (cdr null) is an error
	(try
		(cdr null)
		(do
			true)))

