;; L Sharp test case for cadr
(and
	(eql 'd (cadr '((a b c) d e)))

	;; (cadr null) is an error
	(try
		(cadr null)
		(do
			true)))

