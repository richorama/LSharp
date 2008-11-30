;;; A minimal Windows Forms application

(reference "System.Windows.Forms")
(reference "System.Drawing")

(using "System.Drawing")
(using "System.Windows.Forms")

;; Create a form
( = form1 (new "Form"))
(.set_text form1 "Hello")

;; Create a label
(= label1 (new "Label"))
(.set_Text label1 "Hello World")
(.set_size label1 (new "Size" 224 23))
(.set_Location label1 (new "Point" 24 16))
(.add (.controls form1) label1)

;; Show the form
(.showdialog form1)
