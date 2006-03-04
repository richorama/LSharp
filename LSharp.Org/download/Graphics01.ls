
(reference "System.Windows.Forms"
           "System.Drawing")

(using "System.Windows.Forms"
       "System.Drawing"
       "System.Drawing.Drawing2D")


(defun gen-points (min max)
    (= base-size (- max min))
    (= count 20)
    (= increment (/ base-size count))

    (= new-list (list min))

    (for (= i 1) (<= i count) (++ i)
        (= new-list (append new-list (list (* i increment)))))

    new-list)



(defun draw-lines (graphics pen widths heights)
    (if (or (== widths null) (== heights null))
        null
        (list
            (= start (new point 10 (car heights)))
            (= end (new point (car widths) 10))

            (DrawLine graphics pen start end)
            (draw-lines graphics pen (cdr widths) (cdr heights)))))



(= my-form (new Form))
(set_Text my-form "Graphics Test 01")




(defevent Resize my-form (sender args) (invalidate sender))

(defevent Paint my-form (sender args)
    (= size (get_ClientSize my-form))
    (= graphics (get_Graphics args))
    (= pen (new Pen (get_Black Color)))

    (draw-lines graphics pen
            (gen-points 10 (- (get_width size) 10))
            (reverse (gen-points 10 (- (get_height size) 10)))))


(EnableVisualStyles Application)
(spawn (Run Application my-form))
