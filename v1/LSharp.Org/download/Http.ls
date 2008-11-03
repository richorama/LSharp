;;; HTTP GET and POST

(reference "System" "System.Web")

(using "System.IO" "System.Net" "System.Web" "System.Text")

;; ASCII encodes a Byte Array
(= ascii-encode (fn (byteArray)
	(do
		(GetString (ASCII Encoding) byteArray))))


;; HTTP POST to a url
(= http-post
	(fn (url postdata)
		(do
			(= uri (new Uri url))
			(= request (Create WebRequest uri))
			(set_Method request "POST")
			(set_ContentType request "text/xml")
			(set_ContentLength request (Length postdata))
			(= writer (new StreamWriter (GetRequestStream request)))
			(Write writer postdata)
			(Flush writer)
			(Close writer)
			(= response (GetResponse request))
			(= reader (new StreamReader (GetResponseStream response)))
			(= result (ReadToEnd reader))
			result)))

;; Retrieve a web resource using HTTP GET
(= http-get
	(fn (url)
		
		(do
			(= uri (new Uri url))
			(= request (Create WebRequest uri))
			(set_Timeout request 120000)
			(set_Accept request "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*")
			(set_KeepAlive request false)
			(set_UserAgent request "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)")
			(set_Method request "GET")
			(= response (GetResponse request))
			(= reader (new StreamReader (GetResponseStream response)))
			(= result (ReadToEnd reader))
			(Close reader)
			(Close response)
			result)))


;; Retrieve and print the Slashdot home page
(prl (http-get "http://slashdot.org/"))



