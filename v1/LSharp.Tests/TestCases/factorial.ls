;; Factorial
(= factorial (fn (n)
		(if (eql n 1) 
			1
			(* n (factorial (- n 1))))))