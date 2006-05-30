;;; Hello World in LSharp

(write *response* "<h1>Retrieve data passed to the page</h1>")

(write *response* "<h2>These values are from the querystring</h2>")

(each q (querystring *request*)
	(write *response* (format string "{0}: {1}<br>" q (call item (querystring *request*) q) )))
	
(write *response* "<h2>These values were posted</h2>")

(each q (form *request*)
	(write *response* (format string "{0}: {1}<br>" q (call item (form *request*) q) )))
	




