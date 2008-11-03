;;; Recursively print substrings

;; Print succesive cdrs of a string 
(= funny-print (fn (n)
	(when n
		(prl (the String n))
		(funny-print (rest n)))))
						


;; Concatenate two strings
(= my-string (Concat String "Hello " "there"))

(funny-print my-string)