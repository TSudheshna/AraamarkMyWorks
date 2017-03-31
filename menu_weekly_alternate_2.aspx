<%@ Page language="c#" Codebehind="menu_weekly_alternate_2.aspx.cs" AutoEventWireup="True" Inherits="acCustomer.components.menu_weekly_alternate_2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML lang="en-US" dir="ltr">
   <HEAD>
      <title><%=_strLocationName%> | <%=_strLocationCity%></title>
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
      <meta content="noindex,nofollow" name="robots">
      <link href="../layouts/<%=_directoryName%>/css/<%=_cssClass%>" type=text/css rel=stylesheet >
      <script type="text/javascript" src="../layouts/<%=_directoryName%>/javascript/jquery/jquery.js"></script>
      <style>
         .container {
         margin: 0 auto;
         border-right: 1px solid #CCCCCC;
         }
         .post {
         margin: 0;
         }
         .box {
         float: right;
         width: 217px;
         height: 103px;
         padding: 15px;
         font-size: 12px;
         background-color: #FFFFFF;
         }
         .topBanner {
         height: 135px;
         line-height: 1.2em;
         border-bottom: 1px solid #CCCCCC;
         border-top: 1px solid #CCCCCC;
         width : 500px;
         }
         .bottomBanner {
         height: 45px;
         width: 750px;
         overflow: hidden;
         }
         .column {
         padding: 10px;
         width: 150px;
         height: auto;
         }
         .column .header {
         text-transform: uppercase;
         text-align: center;
         font-size: 18px;
         font-weight: bold;
         margin: 15px 0 15px 0;
         }
         table .container {
         }
         table .inner td {
         border-left: 1px solid #CCCCCC;
         }
         .subhead {
         color: #000000;
         }
      </style>
      <script>
          jQuery(document).ready(function () {
              //adjust column sizes
              var columnPadding = 20;
              var columnCount = jQuery("#column_container td").length;
              var columnBorders = 1;
              var columnWidth = (parseInt(jQuery(".container").attr("width")) / columnCount) - columnPadding - columnBorders;

              jQuery(".column").each(function (index) {
                  var c = columnWidth;
                  jQuery(this).css("width", c + "px");
              });
          });
          //Hides print button and the displays button again after print window
          function printMenu() {
              hidePrintDiv();
              window.print();
              window.setTimeout('hidePrintDiv()', 1000);
          }
          function hidePrintDiv() {
              var e = document.getElementById('printPage');
              if (e.style.display == 'none')
                  e.style.display = 'block';
              else
                  e.style.display = 'none';
          }
      </script> 
   </HEAD>
   <body>
      <!-- *** begin main content ***-->
      <div class="foodMenuLeftDiv">
         <asp:Panel Runat="server" ID="pnlHoursOfOperations" Visible="False">
            <div>
               <h1>Restaurant Hours</h1>
            </div>
            <div class="box clearfix"><%=_strHoursOfOperations%></div>
         </asp:Panel>
         <div style="float: left; width: 100%; font-weight: 500; font-size: .8em;">
            <div id="printerDiv" onclick="openWin();" style="cursor:pointer";><i>Weekly Menu</i> 
               <img alt="print" src="images/Printer_icon.png" style="padding-top:5px;"/>
            </div>
         </div>
      </div>
<%--      <div class ="jumpMenuSectionDown">
         <img src="/layouts/canary_2015/images/down_arrow.png"></img>
      </div>--%>
      <div class="foodMenuRightdiv" id="foodMenuRightdiv" style="background-image: url(<%=_BackgroundImagePath%>);" >
         <div class="foodMenuRightDiv_horizTop">
            <%if(_dtMenuDate == null) { %> 
             <h2></h2>
             <% } else {%>
            <h2>Week of <%=_dtMenuDate%></h2>
             <% } %>
         </div>
         <div id="scrollTop" style="float:left; display:inline-block;">
            <div class="foodMenuDayColumn" >
               <div class="title">
                  <span class="foodMenuStationName">
                  <%=_StationName%>
                  </span>
                  <!-- <div class="quickNavbar">
                       <!-- <asp:Literal ID="quickNavbarLinks" runat="server" />-->
                      <%--<img src="/layouts/canary_2015/images/back_arrow.png"></img>--%>
                     <!-- <asp:Literal ID="navbarBack" runat="server" />
                    </div> -->
               </div>
               <div id="hoursForPrint" class="box clearfix" style="display:none;"><%=_strHoursOfOperations%></div>
            </div>
            <div id="mondayColumn" class="foodMenuDayColumn">
               <h1>Monday</h1>
               <asp:Xml ID="xmlMenuMonday" Runat="server"></asp:Xml>
            </div>
            <div id="tuesdayColumn"  class="foodMenuDayColumn">
               <h1>Tuesday</h1>
               <asp:Xml ID="xmlMenuTuesday" Runat="server"></asp:Xml>
            </div>
         </div>
 <%--       <div class="foodMenuDayColumn" runat="server" id="spacerDiv">
                <asp:Xml ID="xml1" Runat="server"></asp:Xml>
            </div>--%>
<%--         <div class ="jumpMenuSectionUpMiddle">
             <img src="/layouts/canary_2015/images/up_arrow.png"></img>
          </div>--%>
         <div id="scrollDown" style="float:left; display:inline-block;">
            <div id="wednesdayColumn" class="foodMenuDayColumn">
               <h1>Wednesday</h1>
               <asp:Xml ID="xmlMenuWednesday" Runat="server"></asp:Xml>
            </div>
            <div id="thursdayColumn" class="foodMenuDayColumn">
               <h1>Thursday</h1>
               <asp:Xml ID="xmlMenuThursday" Runat="server"></asp:Xml>
            </div>
            <div id="fridayColumn" class="foodMenuDayColumn">
               <h1>Friday</h1>
               <asp:Xml ID="xmlMenuFriday" Runat="server"></asp:Xml>
            </div>
         </div>
<%--          <div runat="server" id="Div1">
               <asp:Xml ID="xml1" Runat="server"></asp:Xml>
               </div>--%>
         <div style="float:left; display:inline-block;">
            <div  class="foodMenuDayColumn" runat="server" id="divEveryDay">
               <h1>Every Day</h1>
               <asp:Xml ID="xmlMenuEveryday" Runat="server"></asp:Xml>
            </div>
             <div class="foodMenuDayColumn"></div>
             <div class="foodMenuDayColumn"></div>
         </div>
          
          <div style="float:left; display:inline-block;">
              <div runat="server" id="divDisclaimer" class="disclaimerStyle">
                  <%=ConfigurationManager.AppSettings.Get("NMLDisclaimerText")%>
              </div>
         </div>
      </div>

       
<%--      <div class ="jumpMenuSectionUp">
         <img src="/layouts/canary_2015/images/up_arrow.png"></img>
      </div>--%>
      <!-- *** end main content ***-->
   </body>
</html>