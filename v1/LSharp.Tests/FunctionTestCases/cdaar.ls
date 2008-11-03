;; L Sharp test case for cdaar
(and
	(eql '(b) (cdaar '(((a b) c) d e)))

	;; (cdaar null) is an error
	(try
		(cdaar null)
		(do
			true)))

