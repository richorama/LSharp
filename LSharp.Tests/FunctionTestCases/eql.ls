(= foo (new LSharp.Cons 5))
(= bar (new LSharp.Cons 5))
(= baz bar)

(and 
	(eql foo bar)
	(eql 1 1)
	(not (eql 1 1.0))
	(eql '(a b) '(a b))
	(eql 'a 'a)
	(eql bar baz))