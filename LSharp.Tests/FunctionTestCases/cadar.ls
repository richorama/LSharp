;; L Sharp test case for cadar
(and
	(eql 'b (cadar '((a b c) d)))

	;; (cadar null) is an error
	(try
		(cadar null)
		(do
			true)))

