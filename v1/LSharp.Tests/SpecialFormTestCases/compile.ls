(= filename (GetTempFileName System.IO.Path))
(compile filename '(+ 1 2 3))
(Delete System.IO.File filename)
true