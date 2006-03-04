(= total 0)
(each a '( 1 2 3)
	(= total (+ total a)))
(== total 6)
