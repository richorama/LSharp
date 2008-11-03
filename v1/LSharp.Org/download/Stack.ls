;;; Stack Sample

(using "System.Collections")

(= my-stack (the Stack '(a b c d e f)))

(Push my-stack 'g)
(Push my-stack 'h)

(prl (the Cons my-stack))

(to i (Count my-stack)
	(prl (Pop my-stack)))