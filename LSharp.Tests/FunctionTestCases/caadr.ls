;; L Sharp test case for caadr
(and
	(eql '(b c) (caadr '(a ((b c) d))))

	;; (caadr null) is an error
	(try
		(caadr null)
		(do
			true)))

