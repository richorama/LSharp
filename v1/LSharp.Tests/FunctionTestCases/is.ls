;; The is operator is used to check whether the run-time type 
;; of an object is compatible with a given type.
(and
	(is String "hello" )
	(is Int32 5)
	(is Double 5.3))