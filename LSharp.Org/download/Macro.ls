;;; A macro which sets a variable to null
(= nil! (macro (x) `(= ,x null)))

(nil! x)

x