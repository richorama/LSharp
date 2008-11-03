;; L Sharp test case for cdddr
(and
	(eql '(d) (cdddr '(a b c d)))

	(eql "lo" (cdddr "hello"))
	
	;; (cdddr null) is an error
	(try
		(cdddr null)
		(do
			true)))

