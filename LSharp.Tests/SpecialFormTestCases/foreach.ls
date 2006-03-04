(= total 0)
(foreach a '( 1 2 3)
	(= total (+ total a)))
(== total 6)
