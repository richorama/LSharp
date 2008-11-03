(= foo (cons 'a 'b))

(and
	(eq (car foo) 'a)
	(eq (cdr foo) 'b))