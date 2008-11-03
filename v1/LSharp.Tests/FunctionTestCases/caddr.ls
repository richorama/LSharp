;; L Sharp test case for caddr
(and
	(eql 'e (caddr '((a b c) d e)))

	;; (caddr null) is an error
	(try
		(caddr null)
		(do
			true)))

