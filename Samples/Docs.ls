;; Iterates over an LSharp environment 
;; to print documentation for functions and
;; macros.

(def show-doc (x)
     "Shows doc for an environment entry"
     (pr (.key x) " ")
     (help (.value x)))

(map show-doc environment)