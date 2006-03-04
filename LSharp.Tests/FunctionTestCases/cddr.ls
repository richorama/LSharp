;; L Sharp test case for cddr
(and
	(eql '(c d) (cddr '(a b c d)))

	(eql "llo" (cddr "hello"))
	
	;; (cddr null) is an error
	(try
		(cddr null)
		(do
			true)))

