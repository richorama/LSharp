(using "System.Threading")
(let foo 0
	(spawn '(= foo 1))
	(sleep thread 1000)
	(eql foo 1))