;;; L Sharp File Sample

(using "System.IO")

;; Copy a file
(= copy-file (fn (from to)
	(do
		(= fileInfo (new FileInfo from))
		(CopyTo fileInfo to true))))

;; Move a file		
(= move-file (fn (from to)
	(do
		(= fileInfo (new FileInfo from))
		(MoveTo fileInfo to))))

;; Delete a file		
(= delete-file (fn (filename)
	(Delete File filename)))

;; Read a file, returning its contents as a string
(= read-file (fn (filename)
	(do
		(= textReader (new StreamReader filename))
		(= buffer (ReadToEnd textReader))
		(Close textReader)
		buffer)))

