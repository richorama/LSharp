;;; Compute Fibonacci numbers

(def fib (n)
     (if (is n 0) 0
       (is n 1) 1
       (+ (fib (- n 1)) (fib (- n 2)))))

; Print the first 15 Fibonacci numbers
(for i 0 15
     (prn (fib i)))

;;(time (fib 15))