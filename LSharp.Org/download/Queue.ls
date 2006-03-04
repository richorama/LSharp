;;; Queue Sample

(using "System.Collections")

(= my-queue (the Queue '(a b c d e f)))

(Enqueue my-queue 'g)
(Enqueue my-queue 'h)

(prl (the cons my-queue))

(to i (Count my-queue)
	(prl (Dequeue my-queue)))