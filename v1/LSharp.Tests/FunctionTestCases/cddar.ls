;; L Sharp test case for cddar
(and
	(eql '(d) (cddar '(((a b) c d) d e)))

	;; (cddar null) is an error
	(try
		(cddar null)
		(do
			true)))

