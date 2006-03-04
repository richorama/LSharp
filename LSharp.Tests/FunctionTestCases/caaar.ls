;; L Sharp test case for caaar
(and
	(eql 'a (caaar '(((a b c) a (b c)))))

	;; (caaar null) is an error
	(try
		(caaar null)
		(do
			true)))

