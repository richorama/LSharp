;;; A web page which allows the user to enter a number
;;; then computes and displays its factorial.

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

(= num-string (item (querystring *request*) "number"))

(when num-string
  (= n (parse int32 num-string))
  (write *response* (factorial n)))
