;; Test changes to evaluator suggested by Patrick
;; ... how do you translate this code in LSharp?
;;
;; Object o = new DateTime(2005,8,10);
;; Object p = o.GetType();
;; Console.WriteLine(p.ToString());

(= o (new DateTime 2005 8 10))
(= p (GetType o))
(prl (ToString p))
