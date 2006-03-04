;;; Sends an SMTP email message

(reference "System.Web")

(using "System.Web.Mail")

; Use this SMTP Mail server
(= *mailserver* "localhost")

(= send-mail (fn (to-address from-address subject body)
	(do	
		(set_SmtpServer SmtpMail *mailserver* )

		(= mailMessage (new MailMessage))

		(set_To mailMessage to-address)
		(set_From mailMessage from-address)
		(set_Subject mailMessage subject)
		(set_Body mailMessage body)

		(Send SmtpMail mailMessage))))


(send-mail "fred.bloggs@hotmail.com" "rob.blackwell@aws.net" "Test Subject" "Test Body")