;; L Sharp test case for car
(and
	(eql 'a (car '(a b c)))

	(eql #\h (car "hello"))
	
	;; (car null) is an error
	(try
		(car null)
		(do
			true)))

