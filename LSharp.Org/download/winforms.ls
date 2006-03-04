;;; Creating a Windows Forms application

(reference "System.Windows.Forms")
(reference "System.Drawing")

(using "System.Drawing")
(using "System.Windows.Forms")

;; Create a form
(= form (new System.Windows.Forms.Form))
(set_text form "Hello")

;; Create a label
(= label1 (new System.Windows.Forms.Label))
(set_Text label1 "Hello World")
(set_Size label1 (new System.Drawing.Size 224 23))
(set_Location label1 (new System.Drawing.Point 24 16))


(add (Controls form) label1)

(showdialog form)