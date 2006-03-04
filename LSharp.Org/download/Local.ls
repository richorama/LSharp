;;; Demonstrates that environments are local to functions
(= x "hello world")
(= foo (fn (x) (prl x)))
(prl (foo "goodbye london"))
(prl x)