;;; SortedList Sample

(using "System.Collections")

;; Convert a list of lists to a Hashtable
(= foo (the SortedList '(("c" "d") ("e" "f") ("a" "b"))))

;; Access an item
(Item foo "c")

;; Add an item
(Add foo "y" "z")

;; Convert the Hashtable back to a list
(prl (the Cons foo))