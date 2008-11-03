;;; A simple WHOIS Client
;;; Rob Blackwell 2004

(reference "System")

(using 
	"System.Net.Sockets"
	"System.Text"
	"System.Collections"
	"System.IO")

;; A lookup table of Top Level Domains and their associated WHOIS servers
(= whois-servers (the Hashtable 
	(quote ( 
		("net" "whois.networksolutions.com")
		("coop" "whois.nic.coop")
		("uk" "whois.nic.uk")
		("com" "whois.verisign-grs.com")
		("org" "whois.pir.org")
		("info" "whois.afilias.net")))))

;; Returns the WHOIS server responsible for a given domain
(= find-whois-server (fn (domain)
	(do
		(= tld (first (last (the Cons (Split domain (ToCharArray "."))))))
		(get_Item whois-servers tld))))

(= whois (fn (server query)
	(do
		(= buffer (GetBytes (ASCII Encoding) (Concat String query "\n")))
		(= stream (GetStream (new TcpClient server 43)))
		(Write stream buffer 0 (GetLength buffer 0))
		(= result (ReadToEnd (new StreamReader stream)))
		(Close stream)
		result)))

;; Usage is Lsharp.exe whois.ls domain, so domain is the third command line arg
(= domain (caddr (GetCommandLineArgs System.Environment)))

;; Find the correct whois server
(= nic (find-whois-server domain))

;; Print the whois information
(prl (whois nic domain))
