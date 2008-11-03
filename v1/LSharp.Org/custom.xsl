<?xml version='1.0'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                version='1.0'>


<!-- TOC only includes top level items -->
<xsl:param name="toc.section.depth" select="1"/>

<!-- create new page for top-level units -->
<xsl:param name="chunk.first.sections">1</xsl:param>

<!-- show doc titles for next and prev link -->
<xsl:param name="navig.showtitles" select="1"/>


<!-- Use an HTML CSS stylesheet. -->
<xsl:param name="html.stylesheet" select="'docbook.css'"/>


<!-- Use if fields on sections to generate HTML filenames -->
<xsl:param name="use.id.as.filename" select="'1'"></xsl:param>


</xsl:stylesheet>

