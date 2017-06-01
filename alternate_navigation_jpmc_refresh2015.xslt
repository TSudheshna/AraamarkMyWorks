<?xml version="1.0" encoding="ISO-8859-1"?>
<!-- Edited by XMLSpy® -->
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:variable name="locMenu" select="@LocationName"/> 	
<xsl:template match="/">  
<ul id="topnav">
	<xsl:apply-templates/>
 </ul>
</xsl:template>
	<xsl:template match="Group">
		<xsl:variable name="rows" select="numModules" />
		<xsl:variable name="locationId" select ="Module/LocationId" />
		<li>
			<a id="menunav">
				<xsl:attribute name="href">
					<xsl:choose>
						<xsl:when test="$rows =1">
							<!--<xsl:value-of select="Module/NavigateUrl"/>-->
							locationhome.aspx?locationid=<xsl:value-of select="$locationId"/>&amp;pageid=<xsl:value-of select="Module/pageId"/>
							<xsl:if test="Module/pageId=30">&amp;custompageid=<xsl:value-of select="Module/CustomPageId"/></xsl:if>
						</xsl:when>
						<xsl:otherwise>#</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="Group_Name"/>
			</a>
			<xsl:if test="$rows > 1">
				<span>
				<xsl:for-each select ="Module">
					<xsl:apply-templates select =".">
						<xsl:with-param name="locId" select="$locationId" />
					</xsl:apply-templates>
				</xsl:for-each>
				</span>
			</xsl:if>
		</li>
	</xsl:template>
	<xsl:template match="Module">
		<xsl:param name="locId" />
			<a>
				<xsl:attribute name="href" >
					<!--<xsl:value-of select="NavigateUrl"/>-->
					locationhome.aspx?locationid=<xsl:value-of select="$locId"/>&amp;pageid=<xsl:value-of select="pageId"/>
					<xsl:if test="pageId=30">
						&amp;custompageid=<xsl:value-of select="CustomPageId"/>
					</xsl:if>          
          <xsl:if test="MenuId">&amp;menuid=<xsl:value-of select="MenuId"/></xsl:if>          
				</xsl:attribute>
				<xsl:value-of select ="LinkText_Customer"/>
			</a>
	</xsl:template>
</xsl:stylesheet>
