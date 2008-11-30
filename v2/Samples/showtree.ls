;; Hook up the LINQ Expression Tree Visualizer

;; Reference the necessary assemblies
(reference "Microsoft.VisualStudio.DebuggerVisualizers")
(reference "c:\\code\\lsharp\\ExpressionTreeVisualizer\\bin\\Debug\\ExpressionTreeVisualizer.dll")

(using "ExpressionVisualizer")
(using "Microsoft.VisualStudio.DebuggerVisualizers")

(def showtree (expr)
     "Shows the Expression tree visualizer for the given LINQ Expression"
     (Console.WriteLine "The Visualizer may be hidden behind a window. Try the GuiHost.")

     (.ShowVisualizer  (new "VisualizerDevelopmentHost" expr
			    (typeof "ExpressionTreeVisualizer")
			    (typeof "ExpressionTreeVisualizerObjectSource"))))


;; If you set optimise to false in LSharp Compiler, then you
;; can inspect compiler outut as LINQ expressions, like this:
;; (showtree (compile '(+ 1 2)))