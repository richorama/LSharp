;; Define a closure and make sure that the environments are correct
(= foo 10)
(= wibble (let foo 5
	(fn (x) (+ x foo))))
(= foo 20)
(wibble 10)

