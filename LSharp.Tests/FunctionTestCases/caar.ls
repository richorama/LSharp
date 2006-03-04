;; L Sharp test case for caar
(and
	(eql 'a (caar '((a b c) a (b c))))

	;; (caar null) is an error
	(try
		(caar null)
		(do
			true)))

