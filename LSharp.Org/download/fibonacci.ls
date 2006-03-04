;;; L Sharp Fibonacci sample

; Fibonacci, tree recursive style
(= fibonacci (fn (n)
		(if (eql n 0) 0
			(if (eql n 1) 1
				(+ (fibonacci (- n 1)) (fibonacci (- n 2)))))))

; Print the first 15 Fibonacci numbers
(to i 15 
	(prl (fibonacci i)))