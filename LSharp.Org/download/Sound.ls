;;; Sound Sample

(reference "Lsharp.Libraries")

(= play-sound (fn (filename)
	(Play LSharp.Libraries.Sound filename)))

(play-sound "c:\\windows\\media\\tada.wav")