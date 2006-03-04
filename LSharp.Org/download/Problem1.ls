;; The problem: Write a function foo that takes a number n and returns a 
;; function that takes a number i, and returns n incremented by i. 

(= foo (fn (n) (fn (i) (+ n i))))
