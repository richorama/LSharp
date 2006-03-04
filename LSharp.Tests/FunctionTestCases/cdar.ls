;; L Sharp test case for cdar
(and
	(eql '(c) (cdar '(((a b) c) d e)))

	;; (cdar null) is an error
	(try
		(cdar null)
		(do
			true)))

