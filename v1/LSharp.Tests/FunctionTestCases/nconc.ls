(setq foo '(a b c))
(setq bar '(d e f))

(setq baz 
      (nconc foo bar))

(and
 (eq (cdddr baz) bar)
 (== baz '(a b c d e f))
 (== (nconc) null)
 (eq (nconc foo) foo))