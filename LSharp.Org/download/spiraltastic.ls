;;; Spiraltastic
;;; More funky .NET graphics

(reference "System" "System.Drawing")

(using 
	"System.Diagnostics"
	"System.Drawing"
	"System.Drawing.Imaging"
	"System.Drawing.Text"
	"System.Drawing.Drawing2D")

(= font (new System.Drawing.Font "Times New Roman" 30))
(= brush (Red Brushes))

;; Create a bitmap of the given size with a spiral pattern
;; based on the specified message
(= spiraltastic (fn (message width height)
	(do
		(= bitmap (new Bitmap width height (Format24bppRgb PixelFormat)))
		(= graphics (FromImage Graphics bitmap))

		(set_TextRenderingHint graphics 
			(AntiAlias TextRenderingHint))

		(TranslateTransform graphics (the Int32 (/ width 2)) (the Int32 (/ height 2)))

		(to i 18
			(do
				(RotateTransform graphics 20)
				(DrawString graphics message font brush 50 0)))
		(Dispose graphics)
		bitmap)))

;; Shell an application
(= shell (fn (app args)
	(do
		(= process (new Process))
		(set_EnableRaisingEvents process false)
		(set_FileName (StartInfo process) app)
		(set_Arguments (StartInfo process) args)
		(Start process))))

;; Create the graphic and save it
(= filename "c:\\junk.png")
(Save (spiraltastic "Spiraltastic" 500 500) filename (Png ImageFormat))

;; Launch Internet Explorer to see the results
(shell "iexplore" filename)

