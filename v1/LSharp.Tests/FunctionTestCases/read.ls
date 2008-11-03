(and
	(eql '(a b c) (read (new System.io.StringReader "(a b c)" )))
	(eql 1 (read (new System.io.StringReader "1" )))
	)