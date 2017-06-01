<?xml version='1.0'?>
<!--<?xml version="1.0" encoding="ISO-8859-1"?>-->
<!-- Edited by XMLSpy® -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="dayNameFromCode"></xsl:param>
  <xsl:param name="dayNameFromCode2"></xsl:param>
  <xsl:param name="stationID"></xsl:param>
  <xsl:param name="MenuId"></xsl:param>
  <xsl:template match="Menu">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="Station">
    <xsl:if test="count(Item[normalize-space(dayname)=$dayNameFromCode]) &gt; 0 or count(Item[normalize-space(dayname)=$dayNameFromCode2]) &gt; 0" ><!-- if the number of children items for the day isn't at least 1, hide the UL -->
      <xsl:if test="$stationID &lt; 0">
       <!--<span class="stationUL" ><xsl:value-of select="StationName" disable-output-escaping="yes"/></span> -->
         <span class="stationUL" >
        <xsl:element name="a">
          <xsl:attribute name="href">
            ./locationhome.aspx?locationid=<xsl:value-of select="locationid"/>&amp;pageid=20<xsl:if test="$MenuId &gt; 0">&amp;menuid=<xsl:value-of select="$MenuId"/></xsl:if>&amp;stationID=<xsl:value-of select="StationId"/>
          </xsl:attribute>
          <xsl:value-of select="StationName" disable-output-escaping="yes"/>
        </xsl:element>
           </span>
      </xsl:if>
      <ul>
        <xsl:apply-templates select="Item" />
      </ul>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Item">
	<xsl:variable name="dayname" select ="dayname"/>
  <xsl:variable name="bVerifyNutrition" select ="ShowNutrition"/>
  <xsl:variable name ="bOverride" select="bOverride"/>
    <xsl:if test="$dayNameFromCode = $dayname or $dayNameFromCode2 = $dayname">
      <xsl:if test="bOverride = 1">
        <li>
          <xsl:element name="div">
            <xsl:attribute name="class">noNutritionalLink</xsl:attribute>
            <xsl:value-of select="OverrideText" disable-output-escaping="yes"/>
          </xsl:element>
        </li>
      </xsl:if>
    <xsl:if test="bOverride = 0">
      <li>
        <xsl:if test="$bVerifyNutrition = 1">
        
          <xsl:element name="a">
            <xsl:attribute name="href">
              <xsl:choose>
                <xsl:when test="DayId=-1">
                  javascript:openNutritionalValues('../../displaynutrition.aspx?dailyitemid=<xsl:value-of select="MenuItemId" disable-output-escaping="yes"/>','slideWindow',false,345,600)
                </xsl:when>
                <xsl:otherwise>
                  javascript:openNutritionalValues('../../displaynutrition.aspx?menuitemid=<xsl:value-of select="MenuItemId" disable-output-escaping="yes"/>','slideWindow',false,345,600)
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="ItemDesc" disable-output-escaping="yes"/>
          </xsl:element>
        </xsl:if>
        <xsl:if test="$bVerifyNutrition = 0">
          <xsl:element name="div">
            <xsl:attribute name="class">noNutritionalLink</xsl:attribute>
            <xsl:value-of select="ItemDesc" disable-output-escaping="yes"/>
           <!--<xsl:if test="Calories != 0">
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>
                <xsl:value-of select="Calories" disable-output-escaping="yes"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]>cal</xsl:text>
            </xsl:if>-->
          </xsl:element>
        </xsl:if>
        <span class="menuRightDiv_li_p">
          <xsl:value-of select="fulldesc" disable-output-escaping="yes"/>
        </span>
        <div class ="noNutritionalLink">
          <xsl:if test="Calories != 0">
            <xsl:value-of select="Calories" disable-output-escaping="yes"/>
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]>CAL</xsl:text>
          </xsl:if>
        </div>
        <span class="item-price">
          <xsl:value-of select="PriceDesc" disable-output-escaping="yes"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Price" disable-output-escaping="yes"/>
        </span>
        <span class="item-price">
          <xsl:value-of select="PriceDesc2" disable-output-escaping="yes"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Price2" disable-output-escaping="yes"/>
        </span>
      </li>
     </xsl:if>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
