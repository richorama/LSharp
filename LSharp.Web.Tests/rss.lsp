;;; Lists all the items from an RSS Feed

(reference "System.Xml")

(= feed "http://planet.lisp.org/rss20.xml")
(= news (new System.Xml.XmlDocument))
(call load news feed)
(write *response* (format string "<h1> RSS Feed : {0}</h1>" feed))
(foreach node (selectnodes news "/rss/channel/item/title") 
	(write *response* "<p>")
	(write *response* (innertext node)))
