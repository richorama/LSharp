;; Repeat Macro

(defmacro repeat (n &rest body) `(to ,(gensym) ,n ,@body))

(repeat 5 (prl "Hello") (prl "there"))