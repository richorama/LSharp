;;; Hello World in LSharp

(write *response* "<h1>An Example Form</h1>")

(write *response* 
"
<h2>Please Type your name</h2>
<p> When you press submit you'll be sent to example1.
<form action='example1.lsp?test=yes' method='post'>
<p>Whats your name? <input type='text' name='name'>
<p>Telephone? <input type='text' name='phone'>
<input type='submit' value='Submit'>
</form>
")


	




