;;; Example exception handling

; try executes its first form (the body) and if an exception is thrown
; executes its second form (the catch) with the variable it bound to
; the System.Exception object.
(try
	(throw (new System.NotImplementedException "foobar"))
	(prl (Format System.String "It failed, here's why: {0}" it)))