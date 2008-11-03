;;; Avalon / Windows Presentation Foundation Example
;;; TechEd Barcelona 2006
;;; Requires .NET Framework 3.0 runtime

(reference "presentationcore")
(reference "presentationframework")
(reference "windowsbase")
(reference "system")
(reference "system.xml")

(using "system.windows")
(using "system.io")
(using "system.windows.markup")

;; Load and instantiate objects from a XAML file
(defun load-xaml (filename)
  (with (f (new filestream filename (Open FileMode))
	element (call load xamlreader f))
	(close f)
	element))

(defun main()
  ;; Define a new application
  (= app (new application))

  ;; Define a window based on a XAML Description
  (= win (the window
	   (load-xaml "d:\\code\\lsharp\\lsharp.org\\download\\calc.xaml")))
  
  ;; Run the application with the new window
  (run app win))
 

;; WPF Requires a Single Treaded Apartment state [STAThread]
(spawn 
 (main)   
 (sta system.threading.apartmentstate))

;; TODO - Events for Calculator implementation


    