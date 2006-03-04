;;; L Sharp Directory sample
(using "System.IO")

;; Rename / move a directory
(= move-directory (fn (from to)
	(do
		(= directoryInfo (new DirectoryInfo from))
		(MoveTo directoryInfo to))))

;; Recurse the given directory applying the the function 
;; or method fun to each file found
(= recurse-directory (fn (dir pattern fun)
	(do
		(= directoryInfo (new DirectoryInfo dir))
		(= files (GetFileSystemInfos directoryInfo))
		(each file files 
			(if (eql (GetType file)	(typeof DirectoryInfo))
					(recurse-directory (FullName file) pattern fun)))

		(= files (GetFileSystemInfos directoryInfo pattern))
		(each file files 
			(if (eql (GetType file)	(typeof FileInfo))
					(fun file)))
	)))

;; Define a function to be called on each file
(= myfun (fn (n)
	(prl (format string "Filename is {0}" (FullName n)))))

;; Recurse the directory calling fun on each dll file
(recurse-directory "c:\\windows\\system32" "*.dll" myfun)

;; Recurse the directory calling the method FullName on each file 
(recurse-directory "c:\\windows\\system32" "*.dll" 'FullName)
