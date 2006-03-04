# Builds the L Sharp documentation

xhtml: lsharp.xml custom.xsl
	xmlto xhtml -m custom.xsl lsharp.xml

pdf: lsharp.xml
	xmlto pdf lsharp.xml
	
txt: lsharp.xml
	xmlto txt lsharp.xml

clean: 
	rm *.html
	rm *.pdf

deploy:
	scp -r . blackwre@lsharp.sourceforge.net:/home/groups/l/ls/lsharp/htdocs/
	