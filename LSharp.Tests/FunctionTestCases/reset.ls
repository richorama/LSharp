(= foo 1)
(reset)

;; Accessing foo should throw an exception
;;(try
;;	(do
;;		foo
;;		false)
;;	true)

;; language change 6 oct - foo is no longer unbound

(not (== foo 1))
	
