(= foo (new LSharp.Cons 5))
(= bar (new LSharp.Cons 5))
(= baz bar)

(and 
	(not (eq foo bar))
	(not (eq 1 1))
	(not (eq '(a b) '(a b)))
	(eq 'a 'a)
	(eql bar baz))