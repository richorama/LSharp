;;; Demonstrate some L Sharp iteration expressions

(for (= i 0) (< i 10) (++ i)
	(pr i))
	
(let i 0
	(while (< (++ i) 10) (pr i)))
	
(to i 10 (pr i))

(each x '(a b c) (pr x))