(= foo (fn (x) ( * 2 x)))

(eql (map foo '(2 4 6)) '(4 8 12))