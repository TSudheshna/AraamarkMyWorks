using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Collections;
using System.Net;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Web.Mail;
using System.Text.RegularExpressions;
using Aramark.SuiteCatering;
using Aramark.SuiteCatering.Common;
using Aramark.SuiteCatering.SuiteWizardAPI;

namespace Aramark.Widgets
{
    using System.Web.UI.HtmlControls;

    public partial class Checkout : AramarkWidgetBase
    {
       public string ssl_merchant_id, ssl_user_id, ssl_pin,ssl_firstname,ssl_lastname = "";
        public AccountProfile AccountInfo
        {
            get { return new UserController().GetAccountProfile(FacilityId, Guid.Parse(AccountId)); }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var profile = (new UserController()).GetAccountProfile(Convert.ToInt32(FacilityId), Guid.Parse(AccountId));

            if (profile != null && profile.Account != null)
            {
                pCCPlaceholder.InnerHtml = profile.Account.PaymentTypeDescription;
                if (!String.IsNullOrEmpty(profile.Account.CardNumber))  //need to display only last 4 digits
                {
                    pCCPlaceholder.InnerHtml += "<br/>" + profile.Account.CardNumber;
                    if (!String.IsNullOrEmpty(profile.Account.CardHolderName))
                    {
                        pCCPlaceholder.InnerHtml += "<br/>" + profile.Account.CardHolderName;
                    }
                }
            }
            if(!IsPostBack)
                LoadCreditCardYears();
            LoadCardTypeId();
            LoadOrder();
        }

        private void LoadCardTypeId()
        {
            var paymentTypes = new OrderController().GetOSCPaymentMethods(Guid.Parse(AccountId), FacilityId);

            try
            {
                cardAMEX.Value = paymentTypes.FirstOrDefault(v => v.Description.Contains("American Express") && (v.Active ?? false)).ID.ToString();
            }
            catch (Exception)
            {
            }

            try
            {
                cardDisc.Value = paymentTypes.FirstOrDefault(v => v.Description.Contains("Discover") && (v.Active ?? false)).ID.ToString();
            }
            catch (Exception)
            {
            }

            try
            {
                cardMC.Value = paymentTypes.FirstOrDefault(v => v.Description.Contains("Mastercard") && (v.Active ?? false)).ID.ToString();
            }
            catch (Exception)
            {
            }

            try
            {
                cardVisa.Value = paymentTypes.FirstOrDefault(v => v.Description.Contains("Visa") && (v.Active ?? false)).ID.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void LoadCreditCardYears()
        {
            var years = new List<ListItem>
                        {
                            new ListItem { Text = "Year", Value = "0", Selected = true},
                            new ListItem{ Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(1).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(1).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(2).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(2).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(3).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(3).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(4).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(4).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(5).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(5).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(6).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(6).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(7).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(7).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(8).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(8).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(9).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(9).Year.ToString(CultureInfo.InvariantCulture)},
                            new ListItem{ Text = DateTime.Now.AddYears(10).Year.ToString(CultureInfo.InvariantCulture), Value =  DateTime.Now.AddYears(10).Year.ToString(CultureInfo.InvariantCulture)}
                        };
            
            cardYear.DataSource = years;
            cardYear.DataBind();
        }

        private void LoadOrder()
        {
            var order = new OrderController().GetOrderSummaryComplete(OrderSummaryId, FacilityId);
            var facility = new FacilityController().GetFacility(FacilityId);
            ssl_merchant_id = facility.SSLMID.ToString();
            ssl_user_id = facility.SSLUser.ToString();
            ssl_pin = facility.SSLPin.ToString();
            litDisclaimer.Text = facility.ServiceChargeDisclaimer;
            litLocation.Text = FacilityName;
            litEvent.Text = order.EventCalendarDescription;
            litEmail.Text = order.EventContact;
            litVIP.Text = order.VIPGuestName;
            OrderPaymentId = order.OrderPayments.First().OrderPaymentID;
            if (String.IsNullOrEmpty(order.VIPGuestName))
            {
                litVIP.Text = "No VIP Guest";
            }

            litComments.Text = order.Comments;
            litDate.Text = order.EventDate.ToShortDateString();

            if (string.IsNullOrEmpty(order.Comments))
            {
                litComments.Text = "No comments were added to this order";
            }

            litSuite.Text = order.SuiteDescription;

            if (order.OrderOptions != null && order.OrderOptions.Any())
            {
                var options = order.OrderOptions.Where(v => v.OptionValue);

                if (options.Any())
                {
                    litOptions.Text =
                        order.OrderOptions.Where(v => v.OptionValue)
                             .Select(v => v.Description)
                             .Aggregate((current, next) => current + ", " + next);
                }
                else
                {
                    litOptions.Text = "No Account Options Selected";
                }
            }

            var alaCart = order.OrderDetails.Where(v => !v.IsPackage);
            rptAlaCarte.DataSource = alaCart;
            rptAlaCarte.DataBind();

            var packages = order.OrderDetails.Where(v => v.IsPackage && v.ParentOrderDetailID == Guid.Empty).OrderBy(v => v.Title);
            var orderedPackages = new List<OrderDetail>();

            foreach (var package in packages)
            {
                orderedPackages.Add(package);
                orderedPackages.AddRange(order.OrderDetails.Where(v => v.IsPackage && v.ParentOrderDetailID == package.OrderDetailID).OrderBy(v => v.Title));
            }

            rptPackages.DataSource = orderedPackages;
            rptPackages.DataBind();

            phSubTotal.Text = order.SubTotal.ToString(CultureInfo.InvariantCulture);

            rptFeeds.DataSource = order.OrderTotals.Where(v => v.Amount != 0);
            rptFeeds.DataBind();

            phGrandTotal.Text = order.OrderPayments.FirstOrDefault().PaymentAmountTotal.ToString(CultureInfo.InvariantCulture);

            var cartScript = String.Format("$(function(){{ unblockUI();$('.utility ul li:nth-child(3)').html('My Cart: <span>{0} - Packages</span> <span>{1} - A la Carte Items</span> <span>${2} - Subtotal</span>'); }});", packages.Count(), alaCart.Count(), order.SubTotal.ToString(CultureInfo.InvariantCulture));
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "updateTopCart", cartScript, true);
        }

        public Guid OrderPaymentId { get; set; }

        protected void lnkSubmit_OnClick(object sender, EventArgs e)
        {           
             Response.Redirect("/order/receipt/", true);          
        }

        private bool SaveOrderPay(string ccToken)
        {
            try
            {
                var order = new OrderController().GetOrderSummaryComplete(OrderSummaryId, FacilityId);
                var payment = order.OrderPayments.SingleOrDefault();
                payment.CardHolderName = cardFirstName.Value + cardLastName.Value; //ssl_firstname + ssl_lastname;
                payment.CardNumber = "CVTKN" + ccToken;
                payment.ExpirationDateMM = cardMonth.Value;
                payment.ExpirationDateYYYY = cardYear.SelectedValue.ToString();
                string paymentid = "";
                if (cardAMEX.Checked)
                    paymentid = cardAMEX.Value;
                else if (cardDisc.Checked)
                    paymentid = cardDisc.Value;
                else if (cardMC.Checked)
                    paymentid = cardMC.Value;
                else if (cardVisa.Checked)
                    paymentid = cardVisa.Value;
                payment.PaymentTypeID =  new Guid(paymentid); 
                                
                payment.SetAsOSCDefaultPayment = futureOrders.Checked;

                bool paymentSuccess = new OrderController().SaveOrderPayment(payment);
                return paymentSuccess;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        protected void lnkSubmitCredit_Click(object sender, EventArgs e)  //for new cards
        {
            
        }

        

        protected void rptPackageItems_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item == null) return;
            var rpt = e.Item.FindControl("rptPackageItems") as Repeater;
            var mainRow = e.Item.FindControl("mainRow") as HtmlTableRow;

            var detail = e.Item.DataItem as OrderDetail;

            if (detail == null || rpt == null) return;

            if (detail.ParentOrderDetailID != Guid.Empty)
            {
                mainRow.Attributes.Add("class", "package-head addon");
                detail.Title = String.Format("{0} (add-on)", detail.Title);
            }

            rpt.DataSource = detail.OrderDetailPackageItems;
            rpt.DataBind();
        }

        protected void lnkCancel_OnClick(object sender, EventArgs e)
        {
            SelectedEventId = Guid.Empty;
            OrderSummaryId = Guid.Empty;
            Response.Redirect("/", true);
        }

        private string getCvgToken()
        {
            string returnValue = "";
            try
            {
                string ssl_transaction_type = "ccgettoken";


                string parmList = "ssl_merchant_id=" + "008203"; // ssl_merchant_id;
                parmList += "&ssl_user_id=" + "webpage"; // ssl_user_id;
                parmList += "&ssl_pin=" + "D8RMQE"; // ssl_pin;
                parmList += "&ssl_transaction_type=" + ssl_transaction_type;
                parmList += "&ssl_show_form=false";
               // parmList += "&ssl_cvv2cvc2_indicator=1";
               // parmList += "&ssl_add_token=Y";
               // parmList += "&ssl_verify=Y";
                parmList += "&ssl_card_number=" + cardNum.Value;
                parmList += "&ssl_exp_date=" + cardMonth.Value + cardYear.SelectedItem.Value.ToString().Substring(2,2);
               // parmList += "&ssl_cvv2cvc2=" + cardCSV.Value;
                parmList += "&ssl_result_format=ASCII";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.demo.convergepay.com/VirtualMerchantDemo/process.do");
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //httpWebRequest.Headers.Add("X-VPS-Timeout", "30");

                byte[] byteArray = Encoding.ASCII.GetBytes(parmList);
                httpWebRequest.ContentLength = byteArray.Length;
                Stream postStream = httpWebRequest.GetRequestStream();
                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream receivedStream = httpWebResponse.GetResponseStream();

                string convergeResponse;
                StreamReader reader = new StreamReader(receivedStream);

                convergeResponse = reader.ReadToEnd();

                // parse converge response

                string tokenSuccessResponse = getValueForConvergeField(convergeResponse, "ssl_token_response");
                if (tokenSuccessResponse.ToLower() == "success")
                {
                    string token = getValueForConvergeField(convergeResponse, "ssl_token");
                    ssl_lastname = getValueForConvergeField(convergeResponse, "ssl_lastname");
                    ssl_firstname = getValueForConvergeField(convergeResponse, "ssl_firstname");
                    returnValue = token;
                }
                else
                {
                    string errorCode = getValueForConvergeField(convergeResponse, "errorCode");
                    string errorMessage = getValueForConvergeField(convergeResponse, "errorMessage");

                  // Response.Write(convergeResponse);

                    returnValue = "ERROR";
                }
            }
            catch (Exception e)
            {
                returnValue = "ERROR";
                Console.WriteLine(e.StackTrace);
            }
            return returnValue;
        }

        private string getValueForConvergeField(string convergeResponse, string fieldName)
        {
           
			string value = "";

			string[] splits = convergeResponse.Split();

			for (int i = 0; i < splits.Length; i++) 
			{
				int ind = splits[i].IndexOf(fieldName);
				if (ind != -1)
				{
					string s = splits[i];
					s = s.Replace(fieldName + "=", "");
					value = s;
					break;
				}
			}

			return value;
        }


        protected void btnPostNew_Click1(object sender, EventArgs e)
        {
            bool saveSuccess = false;
            string ccToken = getCvgToken();
            if (ccToken != "ERROR")
            {
                saveSuccess = SaveOrderPay(ccToken);
                if(saveSuccess)
                    Response.Redirect("/order/receipt/", true);
            }    
            else
            {
                saveSuccess = false;
                convergeError.Visible = true;
            }
                
           // Response.Write("end of saved card payment");
             
        }
    }
}