
;; Display a greeting depending on the hour
(if (< (.Hour (DateTime.Now)) 12) (Console.WriteLine  "Good Morning") 
	(< (.Hour (DateTime.Now)) 18) (Console.WriteLine  "Good Afternoon") 
	(Console.WriteLine  "Good Evening"))