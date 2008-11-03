(= foo (macro (a) `( ,a b)))
(== (macroexpand foo 'boo) '(boo b))