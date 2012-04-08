;;; An RSS Reader in 4 lines of code?

(reference "System.Xml")

(= news (new "System.Xml.XmlDocument"))
(.load news "http://www.theregister.co.uk/headlines.rss")

(map (fn (x) (.innertext x)) (.selectnodes news  "/rss/channel/item/title"))



