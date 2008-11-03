;; I made a change to read. Read needs to take an eof argument that is returned when the eof is reached. 

;; Ensure that it is possible to read null

(= tr (new System.IO.StringReader "null"))
(= eof "")

(while (not (eq (= exp (read tr eof)) eof))
       (prl (= foo exp)))
       
(Close tr)

foo
 
