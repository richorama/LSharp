;; L Sharp test case for apply
(and 
	;; Apply an L Sharp bult in function
	(== 10 (apply + '( 1 2 3 4)))

	;; Apply a closure
	(let x (fn (x) (+ x 2))
		(== 4 (apply x '(2))))

	;; Apply a .NET method
	(eql (apply 'Parse (list System.Double "1.07")) 1.07))