;; A Windows Forms sample by Kelly Murphy
;; A simple script to test the eventing and graphics of LSharp.  
;; This script will draw two curves entirely with straight lines.

(reference "System.Windows.Forms"
           "System.Drawing")

(using "System.Windows.Forms"
       "System.Drawing"
       "System.Drawing.Drawing2D")



; Generate a list of points between the specified min and max.  
; The list will hold the number of
; entries specified by count.

(defun gen-points (count min max)
    (= base-size (- max min))
    (= increment (/ base-size count))
    (= new-list (list min))

    (for (= i 1) (<= i count) (++ i)
        (= new-list (append new-list (list (* i increment)))))

    new-list)

; A recursive function that handles the drawing of the graph lines.  
; All the lines are drawn using the specified graphics object, and 
; drawn in the given pen.  The lines are drawn against the
; specified horizontal (x) and vertical (y) planes.  All of the 
; variable points are specified by the widths and heights lists.

(defun draw-lines (graphics pen x y widths heights)

    (if (or (== widths null) (== heights null))

        null

        (list (DrawLine graphics pen (new point x (car heights)) (new point (car widths) y))
              (draw-lines graphics pen x y (cdr widths) (cdr heights)))))

; Construct a new windows form and give it a name.
; Note that the form's styles are also updated.  
; This is to make the form double buffered and to
; avoid a lot of screen flicker as the form is resized.

(= my-form (new Form))

(set_Text my-form "Graphics Test 01")

(SetStyle my-form (DoubleBuffer ControlStyles) true)
(SetStyle my-form (AllPaintingInWmPaint ControlStyles) true)
(SetStyle my-form (UserPaint ControlStyles) true)


; Attach a function to the new form's paint event.  
; This function will handle generating the points
; needed by the draw lines function.  That and call 
; the draw lines function.


(defevent paint my-form (sender args)

    (= size (get_ClientSize my-form))

    (= graphics (get_Graphics args))
    (= pen (get_Black Pens))

    (set_SmoothingMode graphics (AntiAlias SmoothingMode))

    (FillRectangle graphics (get_White Brushes)
                   10 10 (- (get_width size) 20) (- (get_height size) 20))

    (DrawRectangle graphics pen 10 10 (- (get_width size) 20) (- (get_height size) 20))

    (= point-count 20)
    (= widths (gen-points point-count 10 (- (get_width size) 10)))
    (= heights (reverse (gen-points point-count 10 (- (get_height size) 10))))

    (draw-lines graphics pen 10 10 widths heights)
    (draw-lines graphics pen (- (get_width size) 10) (- (get_height size) 10) widths heights))


; Hook up to the resize event.  This is needed to make sure 
; that the window's contents get properly redrawn when the 
; window gets resized.

(defevent resize my-form (sender args)
    (invalidate sender))

; Finally, enable the XP look and get things rolling using 
; my-form as the application's "main form".

(EnableVisualStyles Application)
(spawn (run application my-form))
