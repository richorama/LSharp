;;; Display a greeting depending on the hour

(if (< (Hour (Now DateTime)) 12) 
	(WriteLine Console "Good Morning") 
	(WriteLine Console "Good Afternoon") )