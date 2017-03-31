<%@ Page language="c#" Codebehind="editmenuitem_lookup.aspx.cs" AutoEventWireup="True" Inherits="acAdmin.menuitems.editmenuitem_lookup" validateRequest="false" %>
<%@ Register TagPrefix="ac" TagName="navbar" Src="../Controls/navbar.ascx" %>
<%@ Register TagPrefix="ac" TagName="header" Src="../Controls/header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ARAMARK Web Management System</title>
		<LINK href="../styles/ac.css" type="text/css" rel="stylesheet">
			<meta http-equiv="Expires" content="0">
			<meta http-equiv="Cache-Control" content="no-cache">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<ac:header id="uclHeader" runat="server"></ac:header>
			<table height="556" width="744" align="left">
				<tr>
					<td vAlign="top" align="left" width="200"><ac:navbar id="uclNavBar" runat="server"></ac:navbar></td>
					<td vAlign="top" align="left"><span class="header1">Edit Menu Item</span>
						<hr>
						<table>
							<TBODY>
								<tr>
									<td colSpan="2"><span class="header7"><asp:label id="lblTitle" Runat="server" Font-Bold="True">Service Date = </asp:label>
										</span></td>
								</tr>
								<asp:Panel id="pnlLookup" runat="server">
									<TR>
										<TD class="menutopcolor" colSpan="2" align="center"><SPAN class="xxsmalltext">Use a Recipe Number: </SPAN>
											<asp:textbox id="txtFmsId" Runat="server" Width="100px"></asp:textbox>
											<asp:button id="btnLookUp" Runat="server" Text="Get It!" onclick="btnLookUp_Click"></asp:button><SPAN class="xxsmalltext"><A href="https://mtoolkit.aramark.net/mtoolkit/fpslh/fpslogon.shtml" target="_blank"><IMG border="0" src="/img/recipebox.gif">
													Go To E-Recipes </A>
											</SPAN></TD>
									</TR>
								</asp:Panel>
								<tr>
									<td align="center" class="menutopcolor" colSpan="2"><asp:checkbox id="chkClosed" Font-Names="arial" Font-Size="X-Small" Runat="server" Text="Hide this Menu item and display the following message in its place:"></asp:checkbox><br>
										<asp:textbox id="txtClosedMessage" Runat="server" Width="325px"></asp:textbox>
										<asp:TextBox id="hidServiceDate" runat="server" Visible="False"></asp:TextBox></td>
								</tr>
								<tr class="header2">
									<td width="100%" colSpan="2">Menu Item Name
									</td>
								</tr>
								<!-- Menu Description -->
								<tr>
									<td><asp:image id="imgStationLogo" Runat="server"></asp:image></td>
									<td><asp:textbox id="txtMenuDesc" Runat="server" Rows="6" Columns="40" TextMode="MultiLine"></asp:textbox><br>
										<!--<font face="arial" color="maroon" size="-2"><b>New!</b></font><font face="arial" size="-2">
											&lt; br &gt; tag no longer needed! <A href="javascript:openHelpWindow('helpguide.cfm?helpid=6','slideWindow',true,345,500)">
												click here for more info!</A></font>-->
									</td>
								</tr>
								<!-- full description --><asp:panel id="pnlFullDesc" Runat="server">
									<TR class="header7">
										<TD width="100%" colSpan="2">Full Description</TD>
									</TR>
									<TR>
										<TD></TD>
										<TD id="nutritionalData">
											<asp:textbox id="txtFullDesc" Runat="server" TextMode="MultiLine" Columns="40" Rows="6"></asp:textbox></TD>
									</TR>
								</asp:panel>
								<!-- prices --><asp:panel id="pnlPrices" Runat="server">
									<TR class="header2">
										<TD width="100%" colSpan="2"><SPAN class="class=header7">Price</SPAN>
										</TD>
									</TR>
									<TR class="bodytext">
										<TD colSpan="2">Price1:
											<asp:TextBox id="txtPrice1" Runat="server" Width="100px"></asp:TextBox>Price1 
											Description:
											<asp:TextBox id="txtPrice1Desc" Runat="server" Width="100px"></asp:TextBox><BR>
											Price2:
											<asp:TextBox id="txtPrice2" Runat="server" Width="100px"></asp:TextBox>Price2 
											Description:
											<asp:TextBox id="txtPrice2Desc" Runat="server" Width="100px"></asp:TextBox></TD>
									</TR>
								</asp:panel>
                                <!-- calories field for non-prima recipies -->
                                <asp:panel id="pnlCaloriesForNonPrima" Runat="server">
									<TR class="header2">
										<TD width="100%" colSpan="2"><SPAN class="class=header7">Calories</SPAN>
										</TD>
									</TR>
									<TR class="bodytext">
										<TD colSpan="2">Calories:
											<asp:TextBox id="txtCaloriesForNonPrima" Runat="server" Width="100px" AutoPostBack="true" ToolTip="Enter Calories in integer format"></asp:TextBox></TD>
									</TR>
								</asp:panel>
								<!-- nutritional data -->
								<asp:panel id="pnlNutritionalData" Runat="server">
									<TR class="header2">
										<TD width="100%" colSpan="2">Nutritional Data -&nbsp;Recipe Number:
											<asp:Label id="lbl8Steps" runat="server"></asp:Label></TD>
									</TR>
									<TR>
										<TD colSpan="2">
											<TABLE border="0" cellSpacing="5" cellPadding="0" width="99%">
												<TR>
													<TD colSpan="4">
														<asp:CheckBox id="chkPublishNutrition" runat="server"></asp:CheckBox><SPAN class="bodytext5">Publish Nutritional Data for this Item.</SPAN>
														<BR>
														&nbsp;
														<asp:CheckBox id="chkVerify" runat="server"></asp:CheckBox><SPAN class="xxsmalltext">I verify that I have printed and followed 
                  the recipe for this item. Recipe <BR>ingredients have not been 
                  substituted and the procedures outlined in the recipe <BR>have 
                  been followed. No ingredients have been added that are not 
                  called for in <BR>the recipe.<BR><A 
                  href="https://mtoolkit.aramark.net/mtoolkit/fpspl/fps_recipe_card.wparse?p_param=wcdrom,,<%=lbl8Steps.Text.ToString().Replace("M","")%>," 
                  target=_blank>Click here to view this recipe on E-Recipes</A>*you must have a STAR PC to view <BR>this 
                  recipe</SPAN>
													</TD>
												</TR>
												<TR>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblServingSize" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblCalories" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblFat" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblProtein" runat="server"></asp:Label>
														</SPAN></TD>
												</TR>
												<TR>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblCarbohydrate" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblCholesterol" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblSodium" runat="server"></asp:Label>
														</SPAN></TD>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblFiber" runat="server"></asp:Label>
														</SPAN></TD>
												</TR>
												<TR>
													<TD align="center"><SPAN class="xxsmalltext">
															<asp:Label id="lblSaturatedFat" runat="server"></asp:Label>
														</SPAN></TD>
													<TD></TD>
													<TD></TD>
													<TD></TD>
												</TR>
											</TABLE>
										</TD>
									</TR>
								</asp:panel>
								<!-- end of nutritional data -->
								<TR>
									<td colspan="2" align="center">
										<asp:Button id="btnSubmit" runat="server" Text="Update It!" onclick="btnSubmit_Click"></asp:Button>
									</td>
								</TR>
							</TBODY>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
