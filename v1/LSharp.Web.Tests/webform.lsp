;;; A web form in L Sharp

(write *response* "<h1>An Example Web Form</h1>")

(write *response* "<p>Whatever you enter will be HTTP POSTed to dump.lsp</p>")

(write *response* 
"
<h2>Please Type your name</h2>
<p> When you press submit you'll be sent to example1.
<form action='dump.lsp?test=yes' method='post'>
<p>Whats your name? <input type='text' name='name'>
<p>Telephone? <input type='text' name='phone'>
<input type='submit' value='Submit'>
</form>
")


	




