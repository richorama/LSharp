
;; Display a greeting depending on the hour
(if (< (.hour (dateTime.now)) 12) (Console.WriteLine  "Good Morning") 
	(< (.hour (DateTime.Now)) 18) (Console.WriteLine  "Good Afternoon") 
	(Console.WriteLine  "Good Evening"))