(= foo 10)
(= bar (fn (foo) (= foo 100)))
(bar 20)
foo