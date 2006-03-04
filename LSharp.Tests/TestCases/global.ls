(= foo 10)
(= bar (fn (x) (= foo x)))
(bar 20)
foo