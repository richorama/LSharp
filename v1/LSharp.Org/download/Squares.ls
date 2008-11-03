;;; List the squares up to n
(= squares (fn (n) 
	(to i (+ n 1)
		(prl (format string "The square of {0} is {1}" i (* i i))))))

(squares 12)