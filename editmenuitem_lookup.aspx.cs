using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using acAdmin.Data;

namespace acAdmin.menuitems
{
	/// <summary>
	/// Summary description for editmenuitem_lookup.
	/// </summary>
	public partial class editmenuitem_lookup : System.Web.UI.Page
	{
		private int menuItemId;
		private int locId;
		private string strDSN;
		bool isNewItem;
		private CultureInfo locCulture;
		protected string timestamp;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			strDSN = Application["dsn"].ToString();
			// get the QueryString values
			locId = Convert.ToInt32(Session["locationid"].ToString());
			//menuItemId = Convert.ToInt32(Request.Params["menuid"].ToString());

			//append to end of url (&tmp=) to fool proxy server
			timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

			LocationData locData = new LocationData(strDSN);
			DataView dvLocInfo = locData.GetLocationInfo(locId);
			pnlLookup.Visible = Convert.ToBoolean(dvLocInfo[0]["bmasteritemlookup"].ToString());

			// if not a page refresh
			if(!Page.IsPostBack)
			{
				try
				{
					// populate the page data
					PopulateData(locId, Convert.ToInt32(Request.Params["menuitemid"].ToString()));
					PopulateServiceDate(locId, int.Parse(Request.Params["menuid"].ToString()), int.Parse(Request.Params["dayid"].ToString()));

				}
				catch
				{
					//menuid is "empty". only get the service date
					PopulateServiceDate(locId, int.Parse(Request.Params["menuid"].ToString()), int.Parse(Request.Params["dayid"].ToString()));
					
					//still need to get the station icon
					getStationIcon(Convert.ToInt32(Request.Params["menustation"].ToString()));
				}			
			}

			// check location configurations
			CheckLocationConfigs();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void getStationIcon(int stationid)
		{
			//getStationIcon
			
			MenuData menuStation = new MenuData(strDSN);
			DataView dvStationIcon = menuStation.getStationIcon(stationid);

			if(dvStationIcon.Count > 0)
			{
				if(dvStationIcon[0]["StationLogoPath_65x65"].ToString().Length > 0)
				{
					imgStationLogo.ImageUrl = "/" + dvStationIcon[0]["StationLogoPath_65x65"].ToString();
				}
				else
				{
					imgStationLogo.ImageUrl = "/img/spacer.gif";
				}
			}
			else
			{
				imgStationLogo.ImageUrl = "/img/spacer.gif";
			}
		}

		protected void btnLookUp_Click(object sender, System.EventArgs e)
		{
			//populate the nutritional data
			try
			{
				// get the data
				//MenuData menuData = new MenuData();
				//DataView dvNutritionalData = menuData.GetNutritionalData(int.Parse(txtFmsId.Text));
				LocationData locData = new LocationData(strDSN);
				DataView dvNutritionalData = locData.GetNutritionalData(locId, txtFmsId.Text);

				if(dvNutritionalData.Count > 0)
				{
					lbl8Steps.Text = dvNutritionalData[0]["fmsid"].ToString();
		
					// populate the screen
					TextInfo TI = new CultureInfo("en-US",false).TextInfo;
					//txtMenuDesc.Text = TI.ToTitleCase(dvNutritionalData[0]["NSNAME"].ToString().ToLower());
					txtMenuDesc.Text = TI.ToTitleCase(dvNutritionalData[0]["marketingname"].ToString().ToLower());
					txtFullDesc.Text = TI.ToTitleCase(dvNutritionalData[0]["marketingdescription"].ToString().ToLower());
					//populate txtFullDesc when new DB table is built.  Note: use TRY CATCH blocks
					//txtFullDesc.Text = dvNutritionalData[0]["nsname"].ToString();
					/*
					lblServingSize.Text = "Serving Size<br>" + dvNutritionalData[0]["nspsizen"].ToString() + " " + dvNutritionalData[0]["nspsizeu"].ToString();
					lblCalories.Text = "Calories<br>" + dvNutritionalData[0]["nutrval05"].ToString();
					lblFat.Text = "Total Fat (g)<br>" + dvNutritionalData[0]["nutrval03"].ToString();
					lblProtein.Text = "Protein (g)<br>" + dvNutritionalData[0]["nutrval02"].ToString();
					lblCarbohydrate.Text = "Carbohydrate (g)<br>" + dvNutritionalData[0]["nutrval04"].ToString();
					lblCholesterol.Text = "Cholesterol (mg)<br>" + dvNutritionalData[0]["nutrval30"].ToString();
					lblSodium.Text = "Sodium (mg)<br>" + dvNutritionalData[0]["nutrval07"].ToString();
					lblFiber.Text = "Dietary Fiber (g)<br>" + dvNutritionalData[0]["nutrval31"].ToString();
					lblSaturatedFat.Text = "Saturated Fat(g)<br>" + dvNutritionalData[0]["nutrval28"].ToString();
					*/
					if((dvNutritionalData[0]["nspsizen"].ToString()).Equals("999999.998"))
					{
						lblServingSize.Text = "Serving Size<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nspsizen"].ToString()).Equals("999999.999"))
					{
						lblServingSize.Text = "Serving Size<br>" + "null";
					}
					else
					{
						lblServingSize.Text = "Serving Size<br>" + dvNutritionalData[0]["nspsizen"].ToString() + " " + dvNutritionalData[0]["nspsizef"].ToString() + " " + dvNutritionalData[0]["nspsizeu"].ToString();
					}
					if((dvNutritionalData[0]["nutrval05"].ToString()).Equals("999999.998"))
					{
						lblCalories.Text = "Calories<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval05"].ToString()).Equals("999999.999"))
					{
						lblCalories.Text = "Calories<br>" + "null";
					}
					else
					{
						lblCalories.Text = "Calories<br>" + dvNutritionalData[0]["nutrval05"].ToString();
					}
					if((dvNutritionalData[0]["nutrval03"].ToString()).Equals("999999.998"))
					{
						lblFat.Text = "Total Fat (g)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval03"].ToString()).Equals("999999.999"))
					{
						lblFat.Text = "Total Fat (g)<br>" + "null";
					}
					else
					{
						lblFat.Text = "Total Fat (g)<br>" + dvNutritionalData[0]["nutrval03"].ToString();
					}
					if((dvNutritionalData[0]["nutrval02"].ToString()).Equals("999999.998"))
					{
						lblProtein.Text = "Protein (g)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval02"].ToString()).Equals("999999.999"))
					{
						lblProtein.Text = "Protein (g)<br>" + "null";
					}
					else
					{
						lblProtein.Text = "Protein (g)<br>" + dvNutritionalData[0]["nutrval02"].ToString();
					}
					if((dvNutritionalData[0]["nutrval04"].ToString()).Equals("999999.998"))
					{
						lblCarbohydrate.Text = "Carbohydrate (g)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval04"].ToString()).Equals("999999.999"))
					{
						lblCarbohydrate.Text = "Carbohydrate (g)<br>" + "null";
					}
					else
					{
						lblCarbohydrate.Text = "Carbohydrate (g)<br>" + dvNutritionalData[0]["nutrval04"].ToString();
					}
					if((dvNutritionalData[0]["nutrval30"].ToString()).Equals("999999.998"))
					{
						lblCholesterol.Text = "Cholesterol (mg)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval30"].ToString()).Equals("999999.999"))
					{
						lblCholesterol.Text = "Cholesterol (mg)<br>" + "null";
					}
					else
					{
						lblCholesterol.Text = "Cholesterol (mg)<br>" + dvNutritionalData[0]["nutrval30"].ToString();
					}
					if((dvNutritionalData[0]["nutrval07"].ToString()).Equals("999999.998"))
					{
						lblSodium.Text = "Sodium (mg)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval07"].ToString()).Equals("999999.999"))
					{
						lblSodium.Text = "Sodium (mg)<br>" + "null";
					}
					else
					{
						lblSodium.Text = "Sodium (mg)<br>" + dvNutritionalData[0]["nutrval07"].ToString();
					}
					if((dvNutritionalData[0]["nutrval31"].ToString()).Equals("999999.998"))
					{
						lblFiber.Text = "Dietary Fiber (g)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval31"].ToString()).Equals("999999.999"))
					{
						lblFiber.Text = "Dietary Fiber (g)<br>" + "null";
					}
					else
					{
						lblFiber.Text = "Dietary Fiber (g)<br>" + dvNutritionalData[0]["nutrval31"].ToString();
					}
					if((dvNutritionalData[0]["nutrval28"].ToString()).Equals("999999.998"))
					{
						lblSaturatedFat.Text = "Saturated Fat(g)<br>" + "trace";
					}
					else if((dvNutritionalData[0]["nutrval28"].ToString()).Equals("999999.999"))
					{
						lblSaturatedFat.Text = "Saturated Fat(g)<br>" + "null";
					}
					else
					{
						lblSaturatedFat.Text = "Saturated Fat(g)<br>" + dvNutritionalData[0]["nutrval28"].ToString();
					}	
				}
				else
				{
					ShowAlert(int.Parse(txtFmsId.Text));
				}
			}
			catch(Exception exc)
			{
				string strErr = exc.Message;
				//user did not enter a filed in the 8Steps textbox
			}
		}


		private void CheckLocationConfigs()
		{
			LocationData locData = new LocationData(strDSN);
			DataView dvLocInfo = locData.GetLocationInfo(locId);
			locCulture = new CultureInfo(dvLocInfo[0]["localename"].ToString(),false);
			// full Description
			if(Convert.ToBoolean(dvLocInfo[0]["bmenufulldesc"].ToString()))
				pnlFullDesc.Visible = true;
			else
				pnlFullDesc.Visible = false;
			// price
			if(Convert.ToBoolean(dvLocInfo[0]["bmenuprices"].ToString()))
				pnlPrices.Visible = true;
			else
				pnlPrices.Visible = false;
			// Nutritional Data:  check if it should be displayed
			if(Convert.ToBoolean(dvLocInfo[0]["bshownutritionaldata"].ToString()))
			{
				//check if there data to be displayed
				if(locData.fmsidExist(locId, lbl8Steps.Text) || locData.fmsidExist(locId, txtFmsId.Text)) //check the nutrition table to see if the fmsid exists
				{
					pnlNutritionalData.Visible = true;
                    pnlCaloriesForNonPrima.Visible = false;
				}
				else
                {
                    pnlNutritionalData.Visible = false;
                    pnlCaloriesForNonPrima.Visible = true;
                }
					
			}
			else
            {
                pnlNutritionalData.Visible = false;
                pnlCaloriesForNonPrima.Visible = true;
            }
		}

		
		private void PopulateData(int locationId, int menuItemId)
		{
			// get the data
			MenuData menuData = new MenuData(strDSN);
			DataView dvMenuData = menuData.GetMenuItemData(locationId, menuItemId);
			
			/*
			DateTime dtStartCycleWeek = DateTime.Parse(dvMenuData[0]["publishmenudate"].ToString());
			int intDayOfWeek = int.Parse(dvMenuData[0]["dayofweek"].ToString());
			int intDateDiff = -1;

			switch(dtStartCycleWeek.DayOfWeek.ToString())
			{
				case "Sunday":
					intDateDiff = intDayOfWeek - 1;
					break;
				case "Monday":
					intDateDiff =  intDayOfWeek - 2;
					break;
				case "Tuesday":
					intDateDiff =  intDayOfWeek - 3;
					break;
				case "Wednesday":
					intDateDiff =  intDayOfWeek - 4;
					break;
				case "Thursday":
					intDateDiff =  intDayOfWeek - 5;
					break;
				case "Friday":
					intDateDiff =  intDayOfWeek - 6;
					break;
				case "Saturday":
					intDateDiff =  intDayOfWeek - 7;
					break;
			}

			DateTime dtServiceDate = dtStartCycleWeek.AddDays(intDateDiff);
			// populate the screen
			lblTitle.Text = "Service Date = " + dtServiceDate.ToLongDateString();
			hidServiceDate.Text = dtServiceDate.ToString();
			*/

			if(dvMenuData[0]["stationlogo"].ToString().Length > 0)
			{
				imgStationLogo.ImageUrl = "/" + dvMenuData[0]["stationlogo"].ToString();
			}
			else
			{
				imgStationLogo.ImageUrl = "/img/spacer.gif";
			}

			//imgStationLogo.ImageUrl = "/" + dvMenuData[0]["stationlogo"].ToString();

			try
			{
				chkClosed.Checked = Convert.ToBoolean(dvMenuData[0]["boverride"].ToString());
			}
			catch
			{
				chkClosed.Checked = false;
			}

			//TextInfo TI = new CultureInfo("en-US",false).TextInfo;

			txtClosedMessage.Text = dvMenuData[0]["overridetext"].ToString();
			txtMenuDesc.Text = dvMenuData[0]["ItemDesc"].ToString().Replace("<br>", "\n");
			txtFullDesc.Text = dvMenuData[0]["FullDesc"].ToString().Replace("<br>", "\n");
			/*
			try
			{
				txtPrice1.Text = Convert.ToDecimal(dvMenuData[0]["Price"].ToString()).ToString("0.00");
			}
			catch
			{
				txtPrice1.Text = "0.00";
			}
			txtPrice1Desc.Text = dvMenuData[0]["Price"].ToString();
			try
			{
				txtPrice2.Text = Convert.ToDecimal(dvMenuData[0]["Price2"].ToString()).ToString("0.00");
			}
			catch
			{
				txtPrice2.Text = "0.00";
			}
			txtPrice2Desc.Text = dvMenuData[0]["Price2Desc"].ToString();
			*/
			lbl8Steps.Text = dvMenuData[0]["fmsid"].ToString();
			try
			{
				chkPublishNutrition.Checked = Convert.ToBoolean(dvMenuData[0]["bverifynutrition"].ToString());
			}
			catch
			{
				chkPublishNutrition.Checked = false;
			}

			DataView dvPrice = menuData.GetMenuItemPrice(locationId, menuItemId);
			try
			{
				txtPrice1.Text = Convert.ToDecimal(dvPrice[0]["price"].ToString()).ToString("0.00");
			}
			catch(Exception exct)
			{
				string str = exct.Message;
				txtPrice1.Text = "";
			}
			try
			{
				txtPrice1Desc.Text = dvPrice[0]["pricedescription"].ToString();
			}
			catch
			{
				txtPrice1Desc.Text = "";
			}
			try
			{
				txtPrice2.Text = Convert.ToDecimal(dvPrice[1]["price"].ToString()).ToString("0.00");
			}
			catch
			{
				txtPrice2.Text = "";
			}
			try
			{
				txtPrice2Desc.Text = dvPrice[1]["pricedescription"].ToString();
			}
			catch
			{
				txtPrice2Desc.Text = "";
			}
            try
            {
                txtCaloriesForNonPrima.Text = dvMenuData[0]["calories"].ToString();
            }
            catch
            {
                txtCaloriesForNonPrima.Text = "";
            }

				
		//get nutritional data from nutrition table
		try
			{
				// get the data
				LocationData locData = new LocationData(strDSN);
				DataView dvNutritionalData = locData.GetNutritionalData(locationId, lbl8Steps.Text.ToString());
		
				// populate the screen
				if((dvNutritionalData[0]["nspsizen"].ToString()).Equals("999999.998"))
				{
					lblServingSize.Text = "Serving Size<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nspsizen"].ToString()).Equals("999999.999"))
				{
					lblServingSize.Text = "Serving Size<br>" + "null";
				}
				else
				{
					lblServingSize.Text = "Serving Size<br>" + dvNutritionalData[0]["nspsizen"].ToString() + " " + dvNutritionalData[0]["nspsizef"].ToString() + " " + dvNutritionalData[0]["nspsizeu"].ToString();
				}
				if((dvNutritionalData[0]["nutrval05"].ToString()).Equals("999999.998"))
				{
					lblCalories.Text = "Calories<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval05"].ToString()).Equals("999999.999"))
				{
					lblCalories.Text = "Calories<br>" + "null";
				}
				else
				{
					lblCalories.Text = "Calories<br>" + dvNutritionalData[0]["nutrval05"].ToString();
				}
				if((dvNutritionalData[0]["nutrval03"].ToString()).Equals("999999.998"))
				{
					lblFat.Text = "Total Fat (g)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval03"].ToString()).Equals("999999.999"))
				{
					lblFat.Text = "Total Fat (g)<br>" + "null";
				}
				else
				{
					lblFat.Text = "Total Fat (g)<br>" + dvNutritionalData[0]["nutrval03"].ToString();
				}
				if((dvNutritionalData[0]["nutrval02"].ToString()).Equals("999999.998"))
				{
					lblProtein.Text = "Protein (g)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval02"].ToString()).Equals("999999.999"))
				{
					lblProtein.Text = "Protein (g)<br>" + "null";
				}
				else
				{
					lblProtein.Text = "Protein (g)<br>" + dvNutritionalData[0]["nutrval02"].ToString();
				}
				if((dvNutritionalData[0]["nutrval04"].ToString()).Equals("999999.998"))
				{
					lblCarbohydrate.Text = "Carbohydrate (g)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval04"].ToString()).Equals("999999.999"))
				{
					lblCarbohydrate.Text = "Carbohydrate (g)<br>" + "null";
				}
				else
				{
					lblCarbohydrate.Text = "Carbohydrate (g)<br>" + dvNutritionalData[0]["nutrval04"].ToString();
				}
				if((dvNutritionalData[0]["nutrval30"].ToString()).Equals("999999.998"))
				{
					lblCholesterol.Text = "Cholesterol (mg)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval30"].ToString()).Equals("999999.999"))
				{
					lblCholesterol.Text = "Cholesterol (mg)<br>" + "null";
				}
				else
				{
					lblCholesterol.Text = "Cholesterol (mg)<br>" + dvNutritionalData[0]["nutrval30"].ToString();
				}
				if((dvNutritionalData[0]["nutrval07"].ToString()).Equals("999999.998"))
				{
					lblSodium.Text = "Sodium (mg)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval07"].ToString()).Equals("999999.999"))
				{
					lblSodium.Text = "Sodium (mg)<br>" + "null";
				}
				else
				{
					lblSodium.Text = "Sodium (mg)<br>" + dvNutritionalData[0]["nutrval07"].ToString();
				}
				if((dvNutritionalData[0]["nutrval31"].ToString()).Equals("999999.998"))
				{
					lblFiber.Text = "Dietary Fiber (g)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval31"].ToString()).Equals("999999.999"))
				{
					lblFiber.Text = "Dietary Fiber (g)<br>" + "null";
				}
				else
				{
					lblFiber.Text = "Dietary Fiber (g)<br>" + dvNutritionalData[0]["nutrval31"].ToString();
				}
				if((dvNutritionalData[0]["nutrval28"].ToString()).Equals("999999.998"))
				{
					lblSaturatedFat.Text = "Saturated Fat(g)<br>" + "trace";
				}
				else if((dvNutritionalData[0]["nutrval28"].ToString()).Equals("999999.999"))
				{
					lblSaturatedFat.Text = "Saturated Fat(g)<br>" + "null";
				}
				else
				{
					lblSaturatedFat.Text = "Saturated Fat(g)<br>" + dvNutritionalData[0]["nutrval28"].ToString();
				}	
			}
			catch
			{
				//no nutrition data
			}
		}

		private void PopulateServiceDate(int locationId, int menuId, int dayId)
		{
			MenuData menuData = new MenuData(strDSN);
			DataView dvCycleWeek = menuData.GetCycleweek(locationId, menuId);

			DateTime dtStartCycleWeek = DateTime.Parse(dvCycleWeek[0]["publishmenudate"].ToString());

			if(dayId == 0)
			{
				lblTitle.Text = "Service Date = " + dtStartCycleWeek.ToLongDateString();
				hidServiceDate.Text = dtStartCycleWeek.ToString();
			}
			else
			{
				int intDayOfWeek = dayId;
				int intDateDiff = -1;

				switch(dtStartCycleWeek.DayOfWeek.ToString())
				{
					case "Sunday":
						intDateDiff = intDayOfWeek - 1;
						break;
					case "Monday":
						intDateDiff =  intDayOfWeek - 2;
						break;
					case "Tuesday":
						intDateDiff =  intDayOfWeek - 3;
						break;
					case "Wednesday":
						intDateDiff =  intDayOfWeek - 4;
						break;
					case "Thursday":
						intDateDiff =  intDayOfWeek - 5;
						break;
					case "Friday":
						intDateDiff =  intDayOfWeek - 6;
						break;
					case "Saturday":
						intDateDiff =  intDayOfWeek - 7;
						break;
				}

				DateTime dtServiceDate = dtStartCycleWeek.AddDays(intDateDiff);
				lblTitle.Text = "Service Date = " + dtServiceDate.ToLongDateString();
				//lblTitle.Text = "Service Date = " + dtServiceDate.ToString(locCulture.DateTimeFormat.LongDatePattern,locCulture) + " - " + Convert.ToDateTime(dvMenuDates[0]["enddate"].ToString()).ToString(locCulture.DateTimeFormat.LongDatePattern,locCulture);
				hidServiceDate.Text = dtServiceDate.ToString();
			}
		}
		
		
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{

			if(txtPrice1.Text.ToString().Length > 0)
			{
                if (IsNumeric(txtPrice1.Text.ToString(), "Only numbers and decimals are allowed in the Price field. Please re-type your price."))
				{
					//its good
				}
				else
				{
					return;
				}
			}
			if(txtPrice2.Text.ToString().Length > 0)
			{
				if(IsNumeric(txtPrice2.Text.ToString(),"Only numbers and decimals are allowed in the Price field. Please re-type your price."))
				{
					//its good
				}
				else
				{
					return;
				}
			}
            if (txtCaloriesForNonPrima.Visible == true) //&& txtCaloriesForNonPrima.Text.ToString().Length > 0
            {
                if (IsNumeric(txtCaloriesForNonPrima.Text.ToString(),"Only numbers are allowed in Calories."))
                {
                    //check for rounded number
                    if (isCaloriesRounded(txtCaloriesForNonPrima.Text.ToString()))
                    {

                    }
                    else
                        return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                    txtCaloriesForNonPrima.Text = "0";
            }

			bool isNewItem = false;
			//TextInfo TI = new CultureInfo("en-US",false).TextInfo;
			MenuData menuData = new MenuData(strDSN);

			try
			{
				Convert.ToInt32(Request.Params["menuitemid"].ToString());
				isNewItem = false;
			}
			catch
			{
				isNewItem = true;
			}

			string fmsid;
			try
			{
				fmsid = lbl8Steps.Text.ToString();
			}
			catch
			{
				fmsid = "0"; //no fmsid has value of 0.  fmsid will become NULL in stored procedure.
			}

			// both boxes must be checked
			if(chkPublishNutrition.Checked && !(chkVerify.Checked))
			{
				NutritionVerifyAlert();
				return;
			}
			//if they don't verify nutrition then we don't record the fmsid
			if(!(chkVerify.Checked))
			{
				fmsid="0"; //will become NULL in the stored procedure
			}

			if(!chkClosed.Checked)
			{
				txtClosedMessage.Text = "";
			}

			if(isNewItem) //Insert
			{
				menuData.InsertMenuItem(
					locId,
					Convert.ToInt32(Request.Params["menustation"].ToString()),
					Convert.ToInt32(Request.Params["menuid"].ToString()),
					Convert.ToInt32(Request.Params["dayid"].ToString()),
					chkClosed.Checked,
					txtClosedMessage.Text.ToString(),
					txtMenuDesc.Text.ToString().Replace("\n", "<br>"),
					txtFullDesc.Text.ToString().Replace("\n", "<br>"),
					"", //txtServingSize.Text,
                    txtCaloriesForNonPrima.Text, //"", 
					"", //txtFat.Text,
					"", //txtProtein.Text,
					"", //txtCarbohydrate.Text,
					"", //txtCholesterol.Text,
					"", //txtSodium.Text,
					"", //txtFiber.Text,
					"", //txtSaturatedFat.Text,
					fmsid, //fmsid
					chkPublishNutrition.Checked, //verify publish nutrition box is checked -- never TRUE on this page because user will enter nutritional facts in manually
					txtPrice1.Text.ToString(),
					txtPrice1Desc.Text.ToString(),
					txtPrice2.Text.ToString(),
					txtPrice2Desc.Text.ToString(),
					Convert.ToInt32(Request.Params["itemorderid"].ToString())
					);

			}
			else
			{
				menuData.UpdateMenuItem(
					locId,
					Convert.ToInt32(Request.Params["menuitemid"].ToString()),
					Convert.ToInt32(Request.Params["menustation"].ToString()),
					Convert.ToInt32(Request.Params["menuid"].ToString()),
					Convert.ToInt32(Request.Params["dayid"].ToString()),
					chkClosed.Checked,
					txtClosedMessage.Text.ToString(),
					txtMenuDesc.Text.ToString().Replace("\n", "<br>"),
					txtFullDesc.Text.ToString().Replace("\n", "<br>"),
					"", //txtServingSize.Text,
                    txtCaloriesForNonPrima.Text, //"", 
					"", //txtFat.Text,
					"", //txtProtein.Text,
					"", //txtCarbohydrate.Text,
					"", //txtCholesterol.Text,
					"", //txtSodium.Text,
					"", //txtFiber.Text,
					"", //txtSaturatedFat.Text,
					fmsid, //fmsid
					chkPublishNutrition.Checked, //verify publish nutrition box is checked -- never TRUE on this page because user will enter nutritional facts in manually
					txtPrice1.Text.ToString(),
					txtPrice1Desc.Text.ToString(),
					txtPrice2.Text.ToString(),
					txtPrice2Desc.Text.ToString()
					);
			}
			Response.Redirect("editmenu.aspx?menuid=" + Request.QueryString["menuid"].ToString() + "&tmp=" + timestamp);

			/*
			//check if user wants to publish the nutritional data
			//and that he/she has verifed the recipe info
			if(chkPublishNutrition.Checked && !(chkVerify.Checked))
			{
				NutritionVerifyAlert();
				return;
			}
								
			// update the database
			// this function is also used in editmenuitem.aspx: that is why there are empty string values
			// do not update nutritional data. only the fmsid value
			MenuData menuData = new MenuData();
			menuData.UpdateMenuItem(
				menuItemId,
				chkClosed.Checked,
				txtClosedMessage.Text,
				txtMenuDesc.Text,
				txtFullDesc.Text,
				txtPrice1.Text,
				txtPrice1Desc.Text,
				txtPrice2.Text,
				txtPrice2Desc.Text,
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				lbl8Steps.Text,
				chkPublishNutrition.Checked
				);

			// redirect the user back to the menu
			Response.Redirect("/rssbui/menuitems/editmenu.aspx?date=" + DateTime.Parse(hidServiceDate.Text).ToShortDateString());
			*/
		}

        private bool isCaloriesRounded(string strCalories)
        {
            int intCalories = int.Parse(strCalories);
            if(intCalories > 0)
            {
                if(intCalories < 50)
                {
                    if (intCalories % 5 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ShowAlert("Round the calories to nearest 5 if its <50 or to its nearest 10 if its >=50.");
                        return false;
                    }

                }
                else// (intCalories >= 50)
                {
                    if (intCalories % 10 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ShowAlert("Round the calories to nearest 5 if its <50 or to its nearest 10 if its >=50.");
                        return false;
                    }
                }  
            }
            else
            {
                ShowAlert("Calories should a positive interger value.");
                return false;
            }
                   
  
        }
		
		private void NutritionVerifyAlert()
		{
			string scriptString = "<script language=JavaScript>";
			scriptString += @"alert('You have selected the option to include nutritional information, but have not verified that the recipe has been printed and followed.  Please verify this information by clicking the appropriate checkbox.');";
			scriptString += "</script>";

			if(!Page.IsClientScriptBlockRegistered("clientScript"))
				Page.RegisterClientScriptBlock("clientScript", scriptString);
		}

		private bool IsNumeric(string price, string alertMessage)
		{
			try
			{
				decimal.Parse(price);
				return true;
			}
			catch
			{
				//ShowAlert("not numeric");
                ShowAlert(alertMessage);
				return false;
			}
			return false;
		}


		private void ShowAlert(int fmsid)
		{
			string scriptString = "<script language=JavaScript>";
			scriptString += @"alert('Recipe number " + fmsid + " not found in the system. Please click OK and try again!');";
			scriptString += "</script>";

			if(!Page.IsClientScriptBlockRegistered("clientScript"))
				Page.RegisterClientScriptBlock("clientScript", scriptString);
		}

		private void ShowAlert(string str)
		{
			string scriptString = "<script language=JavaScript>";
			if(!String.IsNullOrEmpty(str))
			{
				//scriptString += @"alert('Only numbers and decimals are allowed in the Price field. Please re-type your price.');";
                scriptString += @"alert('"+ str +"');";
			}
			scriptString += "</script>";

			if(!Page.IsClientScriptBlockRegistered("clientScript"))
				Page.RegisterClientScriptBlock("clientScript", scriptString);
		}

        //protected void txtCaloriesForNonPrima_TextChanged(object sender, EventArgs e)
        //{
        //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtCaloriesForNonPrima.Text, "^[0-9]*$"))
        //    {
        //        txtCaloriesForNonPrima.Text = string.Empty;
        //        string scriptString = "<script language=JavaScript>";
        //        scriptString += @"alert('Please enter calories in interger format only.');";
        //        scriptString += "</script>";
        //        Page.RegisterClientScriptBlock("clientScript", scriptString);
        //        txtCaloriesForNonPrima.Focus();
        //        //if (!Page.IsClientScriptBlockRegistered("clientScript"))
        //        //    ClientScript.RegisterClientScriptBlock("clientScript", scriptString);
        //    }
        //}
	}
}
