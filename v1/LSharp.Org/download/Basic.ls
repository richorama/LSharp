; basics.ls -- implements some universally used functions in
;              the L# language
;
; Copyright (c) 2006, Andrew Norris
;
; This file doesn't contains anything especially remarkable, 
; and anyone is free to reuse this in whatever fashion you 
; like, and include it in a product licensed under any terms.
; Please retain the copyright attribution. Thanks.


; reasonably efficient list concatenation function (= concat (fn (&rest items)
    (let sb (new system.text.stringbuilder)
      (foreach elem items
        (call append sb elem))
      (tostring sb ))))

; recovers the string value of an object for use in code (= str (fn (obj)
    (writetostring printer obj)))

; applies function f to a list of items and returns the ; result (= map (fn (f lst)
    (if (== (length lst) 1)
      (list (f (car lst)))
      (append (list (f (car lst))) (map f (cdr lst))))))

; returns the items in the list for which f(item) is true (= filter (fn (f lst)
    (let results nil
      (each item lst
        (when (f item)
          (= results
            (if (== results nil)
              (list item)
              (cons item results)))))
      (if (!= results nil)
        (= results (reverse results)))
      results)))

; returns the result of applying a group function over ; the list lst ; ; for example, (reduce (fn (a b) (* a b)) '(2 3 4 5) 1) ; will initialize with the value 1, and return the ; product of all of the values of the list, i.e.
; 1 * 2 * 3 * 4 * 5 = 120
(= reduce (fn (f lst init)
    (each item lst
      (= init (f init item)))
    init))

; pushes item on to the front of list lst, altering the ; actual list passed in (defmacro push (lst item)
  `(= ,lst (cons ,item ,lst)))

; pushes item on to list lst, altering the actual list ; passed in ; note: uses a goofy variable, because 1.2.1 doesn't have ; gensym (defmacro pop (lst)
  `(let obscure-item-name (car ,lst)
    (= ,lst (cdr ,lst))
    obscure-item-name)))
