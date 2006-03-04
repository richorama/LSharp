;;; L Sharp Factorial sample

;; Factorial
(= factorial (fn (n)
		(if (eql n 1) 
			1
			(* n (factorial (- n 1))))))
			
(prl (factorial 5))