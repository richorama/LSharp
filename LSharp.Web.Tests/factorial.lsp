;; Web Apps in L Sharp

(= factorial (fn (n)
		(if (eql n 1) 
			1
			(* n (factorial (- n 1))))))
			
(write *response* 
"<h1> Factorial in L Sharp </h1>
<form  method='get'>
<p>Please enter a number <input type='text' name='number'>
<input type='submit' value='Submit'>
</form>")

(= n (item (querystring *request*) "number"))


(write *response* (factorial n))
