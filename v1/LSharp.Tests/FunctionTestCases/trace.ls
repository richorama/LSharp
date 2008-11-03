(= filename (GetTempFileName System.IO.Path))
(trace filename '(+ 1 2 3))
(Delete System.IO.File filename)
true