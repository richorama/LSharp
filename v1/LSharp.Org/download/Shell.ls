;;; Demonstrates how to shell another process
(reference "System")
(using "System.Diagnostics")

(= shell (fn (app args)
	(do
		(= process (new Process))
		(set_EnableRaisingEvents process false)
		(set_FileName (StartInfo process) app)
		(set_Arguments (StartInfo process) args)
		(Start process))))

(shell "calc" "")
(shell "iexplore" "http://www.aws.net")
(shell "notepad" "c:\\junk.txt")