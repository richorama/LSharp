;;; Regular expression example

(reference "System")

(using "System.Text.RegularExpressions")

; Returns true if search appears in buffer
(= find (fn (buffer search)
		(do 
			(= regex (new Regex (Concat System.String ".*" search)))
			(Success (Match regex buffer)))))

; Define a regular expression for matching URLs
(= urlExpression (new Regex "^(?<proto>\\w+)://[^/]+?(?<port>:\\d+)?/"))

; Does it match ?
(prl (Match urlExpression "http://www.aws.net:8080/"))

; Pull out the protocol and port
(prl (Result (Match urlExpression "http://www.aws.net:8080/") "The protocol is ${proto} and the port is ${port}"))

