;;; Hashtable Sample

(using "System.Collections")

;; Convert a list of lists to a Hashtable
(= foo (the Hashtable '(("a" "b") ("c" "d") ("e" "f"))))

;; Access an item
(Item foo "c")

;; Add an item
(set_Item foo "y" "z")

;; Convert the Hashtable back to a list
(prl (the Cons foo))