(= a 10)
(= b '(a b c))

(== `( foo ,a bar ,@b c) '(foo 10 bar a b c c))