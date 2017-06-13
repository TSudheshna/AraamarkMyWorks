namespace Aramark.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Compat.Web;
    using System.Configuration;
    using System.Linq;
    using System.Web.Script.Services;
    using System.Web.Services;
    using Aramark.SuiteCatering;
    using Aramark.SuiteCatering.Common;
    using Aramark.SuiteCatering.SuiteWizardAPI;
    using Ektron.Cms.Common;
    using Ektron.Cms.Framework;
    using Ektron.Cms.Framework.User;
    using Ektron.Cms.User;
    using System.Web;
    using System.IO;
    using System.Xml;
    using System.Collections;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The calendar events.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class CalendarEvents : WebService
    {

        protected string card_short_description = "";
        //screen #5
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuItemDetails> GetMenuItemDetail(int facilityId, Guid menuItemId, Guid customerId, Guid eventId)
        {
            List<MenuItemDetails> items=new List<MenuItemDetails>();
            for (int i = 0; i < 3; i++) //added 3 more attempts.
            {
                try
                {
                    items = new MenuController().GetMenuItemDetail(facilityId, menuItemId, eventId, customerId);
                    return items.ToList();
                }
                catch (Exception)
                {
                    
                    //todo: logging exception here
                }
                
            }
            
            return items.ToList();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SendOrderEmail(Guid orderSummaryId, int facilityId)
        {
            Utilities.SendOrderEmail(orderSummaryId,facilityId,ConfigurationSettings.AppSettings["SMTPServer"]);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SendOrderEmailMyAccount(Guid orderSummaryId, int facilityId)
        {
            Utilities.SendOrderEmailMyAccount(orderSummaryId, facilityId, ConfigurationSettings.AppSettings["SMTPServer"]);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CalendarEvent> GetCalendarEvents(DateTime startDate, DateTime endDate, int facilityId, int eventDayCutoffHours, int forView)
        {
            var startMonth = startDate.Month;
            var startYear = startDate.Year;
            var endMonth = endDate.Month;
            var endYear = endDate.Year;

            var facilityEvents = new List<EventCalendar>();

            if (startMonth == endMonth)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(startMonth, startYear, facilityId, eventDayCutoffHours, forView));
            }
            else if (startMonth + 1 == endMonth)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(startMonth, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(endMonth, endYear, facilityId, eventDayCutoffHours, forView));
            }
            else if (startMonth + 2 == endMonth)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(startMonth, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(startMonth + 1, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(endMonth, endYear, facilityId, eventDayCutoffHours, forView));
            }
            else if (startYear + 1 == endYear && startMonth == 12 && endMonth == 1)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(12, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(1, endYear, facilityId, eventDayCutoffHours, forView));
            }
            else if (startYear + 1 == endYear && startMonth == 11 && endMonth == 1)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(11, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(12, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(1, endYear, facilityId, eventDayCutoffHours, forView));
            }
            else if (startYear + 1 == endYear && startMonth == 12 && endMonth == 2)
            {
                facilityEvents.AddRange(new EventController().GetEventCalendar(12, startYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(1, endYear, facilityId, eventDayCutoffHours, forView));
                facilityEvents.AddRange(new EventController().GetEventCalendar(2, endYear, facilityId, eventDayCutoffHours, forView));
            }
            
            // var results = facilityEvents.Where(v => v.EventDate > DateTime.Today).Where(v => v.DisplayAsLink == 1).Select(entry => new CalendarEvent
            String currentHost = HttpContext.Current.Request.Url.Host.ToLower();

            var returnList = new List<CalendarEvent>();
            
            if (currentHost.Contains("boothcatering"))
            {
                 var results = facilityEvents.Where(v => v.EventDate >= DateTime.Today).Select(entry => new CalendarEvent
                                                      {
                                                          Id = entry.EventID,
                                                          Title = entry.Description,
                                                          StartDate = ((DateTime)entry.EventDateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                                          EndDate = ((DateTime)entry.CutOffDateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                                          StartTime = ((DateTime)entry.Begintime).ToString("h:mm tt"),
                                                          StartDay = ((DateTime)entry.EventDateTime).ToString("MMMM dd, yyyy"),
                                                          DayofWeek = ((DateTime)entry.EventDateTime).ToString("dddd"),
                                                          Day = ((DateTime)entry.EventDateTime).ToString("dd"),
                                                          EventId = entry.EventID,
                                                          MenuId = entry.MenuID
                                                      }).ToList();

                 foreach (var calendarEvent in results)
                 {
                     if (returnList.All(v => v.EventId != calendarEvent.EventId))
                     {
                         returnList.Add(calendarEvent);
                     }
                 }

            }
            else
            {
                var results = facilityEvents.Where(v => v.EventDate > DateTime.Today).Where(v => v.DisplayAsLink == 1).Select(entry => new CalendarEvent
                {
                    Id = entry.EventID,
                    Title = entry.Description,
                    StartDate = ((DateTime)entry.EventDateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    EndDate = ((DateTime)entry.CutOffDateTime).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    StartTime = ((DateTime)entry.Begintime).ToString("h:mm tt"),
                    StartDay = ((DateTime)entry.EventDateTime).ToString("MMMM dd, yyyy"),
                    DayofWeek = ((DateTime)entry.EventDateTime).ToString("dddd"),
                    Day = ((DateTime)entry.EventDateTime).ToString("dd"),
                    EventId = entry.EventID,
                    MenuId = entry.MenuID
                }).ToList();

                foreach (var calendarEvent in results)
                {
                    if (returnList.All(v => v.EventId != calendarEvent.EventId))
                    {
                        returnList.Add(calendarEvent);
                    }
                }
            }

            return returnList;
        }
 

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Suite> GetSuites(int facilityId, Guid? suiteId)
        {
            return new SuiteController().GetSuites(facilityId, suiteId).ToList();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuPackagesList> GetMenuPackagesList(int facilityId, Guid menuId)
        {
            return new PackageController().GetMenuPackagesList(facilityId, menuId).OrderBy(v => v.SequenceNumber).ToList();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuItemDetails> GetTopSellerItems(int facilityId, Guid menuId, Guid customerId)
        {
            return new PackageController().GetRecommendedItems(facilityId, menuId, customerId).OrderBy(v => v.CategorySort).ToList();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuItemDetails> GetRecommendedItems(int facilityId, Guid menuId, Guid customerId)
        {
            return new PackageController().GetRecommendedItems(facilityId, menuId, customerId).OrderBy(v => v.CategorySort).ToList();
        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AlaCarteGroupList> GetMenuCategoriesList(int facilityId, Guid menuId)
        {
            var items = new MenuController().GetMenuCategoriesList(facilityId, menuId).OrderBy(v => v.SequenceNumber).ToList();

            var completeItems = new List<AlaCarteGroupList>();
            foreach (var item in items)
            {
                completeItems.Add(item);
                if (item.SubCatCount > 0) completeItems.AddRange(new MenuController().GetMenuSubCategoriesList(facilityId, menuId, item.CategoryID).OrderBy(v => v.SequenceNumber));
                //Context.Cache.Insert(String.Format("AlaCarteItem{0}{1}{2}{3}", item.MenuItemID, facilityId, customerId, eventId), item, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return completeItems.ToList();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CrossSellMenuItemListPackage> GetMenuItemCrossSellList(int facilityId, Guid menuDetailId, Guid eventId, Guid customerId)
        {
            var items = new MenuController().GetMenuItemCrossSellList(facilityId, menuDetailId).Select(x => new CrossSellMenuItemListPackage()
            {
                CrossSellID = x.CrossSellID,
                ExtensionData = x.ExtensionData,
                IsPackage = x.IsPackage,
                MenuDetailID = x.MenuDetailID,
                MenuItemID = x.MenuItemID,
                NumberServed = x.NumberServed,
                Price = x.Price,
                ShortDescription = x.ShortDescription,
                SmallGraphicFile = x.SmallGraphicFile,
                Title = x.Title
            }).ToList();

            foreach (var item in items)
            {
                if (item.IsPackage ?? false)
                {
                    var package = GetPackageDetail(facilityId, customerId, item.MenuItemID, eventId);
                    item.PackageTypeID = package.PackageTypeID;
                }
                Context.Cache.Insert(String.Format("CrossSellItem{0}{1}", item.MenuItemID, facilityId), item, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return items.ToList();
        }

        public class CrossSellMenuItemListPackage : CrossSellMenuItemList
        {
            public int? PackageTypeID { get; set; }
        }

        /// <summary>
        /// The get package detail.
        /// </summary>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="packageId">
        /// The package id.
        /// </param>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <returns>
        /// The <see cref="PackageDetail"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public PackageDetail GetPackageDetail(int facilityId, Guid customerId, Guid packageId, Guid eventId)
        {
            
            var item = new PackageController().GetPackageDetail(facilityId, customerId, packageId, eventId);
            if (!String.IsNullOrEmpty(item.ShortDescription)) item.ShortDescription = System.Compat.Web.HttpUtility.HtmlDecode(item.ShortDescription);
            foreach (var menuItem in item.PackageMenuItems)
            {
                menuItem.ShortDescription = System.Compat.Web.HttpUtility.HtmlDecode(menuItem.ShortDescription);
            }
            item.PackageMenuItems = item.PackageMenuItems.OrderBy(v => v.CategorySort ?? 0).ToList();
            Context.Cache.Insert(String.Format("PackageItem{0}{1}{2}{3}", packageId, facilityId, customerId, eventId), item, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            return item;
        }

        /// <summary>
        /// The get sub category items list.
        /// </summary>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <param name="menuId">
        /// The menu id.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <param name="subCategoryId">
        /// The sub category id.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuItemDetails> GetSubCategoryItemsList(int facilityId, Guid menuId, Guid categoryId, Guid subCategoryId, Guid customerId, Guid eventId)
        {
            var items = new MenuController().GetMenuMenuItemsList(facilityId, menuId, categoryId, subCategoryId, customerId, eventId);

            foreach (var item in items)
            {
                Context.Cache.Insert(String.Format("AlaCarteItem{0}{1}{2}{3}", item.MenuItemID, facilityId, customerId, eventId), item, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return items.OrderBy(v => v.CategorySort).ToList();
        }

        /// <summary>
        /// The search menu.
        /// </summary>
        /// <param name="menuId">
        /// The menu id.
        /// </param>
        /// <param name="keywords">
        /// The keywords.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MenuItemDetails> SearchMenu(Guid menuId, string keywords)
        {
            return new MenuController().SearchMenu(menuId, keywords).OrderBy(v => v.CategorySort).ToList();
        }

        /// <summary>
        /// The get menu sub categories list.
        /// </summary>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <param name="menuId">
        /// The menu id.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AlaCarteGroupList> GetMenuSubCategoriesList(int facilityId, Guid menuId, Guid categoryId)
        {
            return new MenuController().GetMenuSubCategoriesList(facilityId, menuId, categoryId).ToList();
        }

        /// <summary>
        /// The get order summary info.
        /// </summary>
        /// <param name="orderSummaryId">
        /// The order summary id.
        /// </param>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <returns>
        /// The <see cref="OrderSummary"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public OrderSummary GetOrderSummaryInfo(Guid orderSummaryId, int facilityId)
        {
            return new OrderController().GetOrderSummaryInfo(orderSummaryId, facilityId);
        }

        /// <summary>
        /// The get pending orders.
        /// </summary>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <param name="accountId">
        /// The account id.
        /// </param>
        /// <param name="eventCalendarId">
        /// The event calendar id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<OrderSummary> GetPendingOrders(int facilityId, Guid accountId, Guid eventCalendarId)
        {
            return new AccountController().GetAccountHistoryFull(facilityId, accountId, eventCalendarId, true).OrderBy(v => v.StatusID).ToList();
        }

        /// <summary>
        /// The get order summary complete.
        /// </summary>
        /// <param name="orderSummaryId">
        /// The order summary id.
        /// </param>
        /// <param name="facilityId">
        /// The facility id.
        /// </param>
        /// <returns>
        /// The <see cref="OrderSummary"/>.
        /// </returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public OrderSummary GetOrderSummaryComplete(Guid orderSummaryId, int facilityId)
        {
            var order = new OrderController().GetOrderSummaryComplete(orderSummaryId, facilityId);
            var packages = order.OrderDetails.Where(v => v.IsPackage && v.ParentOrderDetailID == Guid.Empty).OrderBy(v => v.Title);
            var orderedPackages = new List<OrderDetail>();

            foreach (var package in packages)
            {
                orderedPackages.Add(package);
                orderedPackages.AddRange(order.OrderDetails.Where(v => v.IsPackage && v.ParentOrderDetailID == package.OrderDetailID).OrderBy(v => v.Title));
            }

            orderedPackages.AddRange(order.OrderDetails.Where(v => !v.IsPackage));

            order.OrderDetails = orderedPackages;
            return order;
        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Guid SaveOrderDetail(OrderDetail oOrderDetail)
        {
            return new OrderController().SaveOrderDetail(oOrderDetail);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AccountProfile GetAccountProfile(int facilityId, Guid accountId)
        {
            return new UserController().GetAccountProfile(facilityId, accountId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AccountProfile UpdateAccountProfile(int facilityId, Guid accountId, string firstName, string lastName, string company, string jobTitle, string email, string userId, string password, string reminder)
        {
            var profile = new UserController().GetAccountProfile(facilityId, accountId);

            profile.Account.FirstName = firstName;
            profile.Account.LastName = lastName;
            profile.Account.CompanyName = company;
            profile.Account.Title = jobTitle;
            profile.Account.UserID = userId;
            profile.Account.Password = (password == "********") ? profile.Account.Password : password;
            profile.Account.Reminder = reminder;
            profile.Account.EMailID = email;
            profile.Account.AccountID = profile.Account.AccountID;
            profile.Account.AccountNum = profile.Account.AccountNum;
            profile.Account.AccountPayMethodID = profile.Account.AccountPayMethodID;
            profile.Account.AComments = profile.Account.AComments;
            profile.Account.Active1 = profile.Account.Active1;
            profile.Account.Address1 = profile.Account.Address1;
            profile.Account.Address2 = profile.Account.Address2;
            profile.Account.Address3 = profile.Account.Address3;
            profile.Account.AdminUserId = profile.Account.AdminUserId;
            profile.Account.CardHolderName = profile.Account.CardHolderName;
            profile.Account.CardNumber = profile.Account.CardNumber;
            profile.Account.City = profile.Account.City;
            profile.Account.Country = profile.Account.Country;
            profile.Account.CustomerAccountNumber = profile.Account.CustomerAccountNumber;
            profile.Account.CustomerID = profile.Account.CustomerID;
            profile.Account.CVV2 = profile.Account.CVV2;
            profile.Account.DefaultEmailConfirm = profile.Account.DefaultEmailConfirm;
            profile.Account.DefaultOrder = profile.Account.DefaultOrder;
            profile.Account.DefaultPARBAR = profile.Account.DefaultPARBAR;
            profile.Account.DefaultPaymentMethod = profile.Account.DefaultPaymentMethod;
            profile.Account.DefaultSuiteID = profile.Account.DefaultSuiteID;
            profile.Account.ExpirationDateMM = profile.Account.ExpirationDateMM;
            profile.Account.ExpirationDateYYYY = profile.Account.ExpirationDateYYYY;
            profile.Account.ExtensionData = profile.Account.ExtensionData;
            profile.Account.FacilityID = profile.Account.FacilityID;
            profile.Account.FaxNumber = profile.Account.FaxNumber;
            profile.Account.LastUpdated = profile.Account.LastUpdated;
            profile.Account.MiddleName = profile.Account.MiddleName;
            profile.Account.OSCID = profile.Account.OSCID;
            profile.Account.PARBARAccountID = profile.Account.PARBARAccountID;
            profile.Account.PARBARUpdate = profile.Account.PARBARUpdate;
            profile.Account.PaymentType = profile.Account.PaymentType;
            profile.Account.PaymentTypeDescription = profile.Account.PaymentTypeDescription;
            profile.Account.PaymentTypeID = profile.Account.PaymentTypeID;
            profile.Account.PhoneExt = profile.Account.PhoneExt;
            profile.Account.PhoneNumber = profile.Account.PhoneNumber;
            profile.Account.PostalCode = profile.Account.PostalCode;
            profile.Account.StateProvince = profile.Account.StateProvince;
            profile.Account.SuiteHolderTypeID = profile.Account.SuiteHolderTypeID;
            profile.Account.UseBilling = profile.Account.UseBilling;

            new UserController().EditAccountProfile(profile);

            try
            {
                var userManager = new UserManager(ApiAccessMode.Admin);
                var userCriteria = new UserCriteria();
                userCriteria.AddFilter(UserProperty.UserName, CriteriaFilterOperator.EqualTo, userId);
                var user = userManager.GetList(userCriteria).FirstOrDefault();

                if (user != null)
                {
                    user.Email = email;
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.IsMemberShip = true;
                    userManager.Update(user);
                }
            }
            catch (Exception)
            {
            }

            return profile;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Account CreateHospAccount(int FacilityID, String FirstName, String LastName, String UserID, String Password, String Reminder, String Address1, String Address2, String City, String StateProvince, String PostalCode, String EmailID)
        {
            Account account = new Account();
            // for create, use an empty guid
            Byte[] bytes = new Byte[16];
            Guid emptyGuid = new Guid(bytes);
            account.AccountID = emptyGuid;
            account.FacilityID = FacilityID;
            account.FirstName = FirstName;
            account.LastName = LastName;
            account.UserID = UserID;
            account.Password = Password;
            account.Reminder = Reminder;
            account.Address1 = Address1;
            account.Address2 = Address2;
            account.City = City;
            account.StateProvince = StateProvince;
            account.PostalCode = PostalCode;
            account.EMailID = EmailID;
            account.Title = "NA";
            
            AccountController ac = new AccountController();

            account = ac.CreateEditExHospAccount(account);

            return account;
            
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AccountProfile UpdateAccountBilling(int facilityId, Guid accountId, string paymentTypeDescription, string cardNumber, string cardHolderName, string address1, string address2, string address3, string city, string stateProvince, string postalCode, string phoneNumber, string phoneExt )
        {
            var profile = new UserController().GetAccountProfile(facilityId, accountId);

            profile.Account.FirstName = profile.Account.FirstName;
            profile.Account.LastName = profile.Account.LastName;
            profile.Account.CompanyName = profile.Account.CompanyName;
            profile.Account.Title = profile.Account.Title;
            profile.Account.UserID = profile.Account.UserID;
            profile.Account.Password = profile.Account.Password;
            profile.Account.Reminder = profile.Account.Reminder;
            profile.Account.EMailID = profile.Account.EMailID;
            profile.Account.AccountID = profile.Account.AccountID;
            profile.Account.AccountNum = profile.Account.AccountNum;
            profile.Account.AccountPayMethodID = profile.Account.AccountPayMethodID;
            profile.Account.AComments = profile.Account.AComments;
            profile.Account.Active1 = profile.Account.Active1;
            profile.Account.Address1 = address1;
            profile.Account.Address2 = address2;
            profile.Account.Address3 = address3;
            profile.Account.AdminUserId = profile.Account.AdminUserId;
            profile.Account.CardHolderName = cardHolderName;
            profile.Account.City = city;
            profile.Account.Country = profile.Account.Country;
            profile.Account.CustomerAccountNumber = profile.Account.CustomerAccountNumber;
            profile.Account.CustomerID = profile.Account.CustomerID;
            profile.Account.CVV2 = profile.Account.CVV2;
            profile.Account.DefaultEmailConfirm = profile.Account.DefaultEmailConfirm;
            profile.Account.DefaultOrder = profile.Account.DefaultOrder;
            profile.Account.DefaultPARBAR = profile.Account.DefaultPARBAR;
            profile.Account.DefaultPaymentMethod = profile.Account.DefaultPaymentMethod;
            profile.Account.DefaultSuiteID = profile.Account.DefaultSuiteID;
            profile.Account.ExpirationDateMM = profile.Account.ExpirationDateMM;
            profile.Account.ExpirationDateYYYY = profile.Account.ExpirationDateYYYY;
            profile.Account.ExtensionData = profile.Account.ExtensionData;
            profile.Account.FacilityID = profile.Account.FacilityID;
            profile.Account.FaxNumber = profile.Account.FaxNumber;
            profile.Account.LastUpdated = profile.Account.LastUpdated;
            profile.Account.MiddleName = profile.Account.MiddleName;
            profile.Account.OSCID = profile.Account.OSCID;
            profile.Account.PARBARAccountID = profile.Account.PARBARAccountID;
            profile.Account.PARBARUpdate = profile.Account.PARBARUpdate;
            profile.Account.PaymentType = profile.Account.PaymentType;
            profile.Account.PaymentTypeID = profile.Account.PaymentTypeID;
            profile.Account.PhoneExt = phoneExt;
            profile.Account.PhoneNumber = phoneNumber;
            profile.Account.PostalCode = postalCode;
            profile.Account.StateProvince = stateProvince;
            profile.Account.SuiteHolderTypeID = profile.Account.SuiteHolderTypeID;
            profile.Account.UseBilling = profile.Account.UseBilling;
            new UserController().EditAccountProfile(profile);
            
            return profile;
        }

        [WebMethod]
        public string SaveNewCardDetails(string cardNumber, int CVV, string cardExpMonthMM, string cardExpYYYY, string FirstName, string LastName, Guid cardTypeID, bool setCardasDefault, Guid OrderSummaryId, int FacilityId)
        {
            try
            {
                Debug.WriteLine("In web method");   
                bool saveSuccess = false;
                string ccToken = gettoken(cardNumber, CVV, cardExpMonthMM, cardExpYYYY, FacilityId);
                Regex reNum = new Regex(@"^\d+$");
                bool isNumeric = reNum.Match(ccToken).Success;
                Debug.WriteLine(ccToken);
                Debug.WriteLine(isNumeric);
                if (isNumeric)
                {
                    saveSuccess = SaveOrderPay(ccToken, FirstName, LastName, cardExpMonthMM, cardExpYYYY, cardTypeID, setCardasDefault,OrderSummaryId, FacilityId);
                    if (saveSuccess)
                    {
                        Debug.WriteLine("succcess - ");
                        return "GOTORECEIPT";
                    }
                    else
                    {
                        Debug.WriteLine(ccToken);
                        Debug.WriteLine("suitewizard failed.");
                        return "Internal Server Error. Please try completing this order after a while.";
                    }
                }
                else
                {
                    Debug.WriteLine("ccToken not numeric");
                    return "External Server Error. Please try completing this order after a while.";
                }
            }
           catch(Exception e)
            {
                Debug.WriteLine(e);
                return "Could not complete the order. Please contact administrator for further assistance.";
            }

        }

        private bool SaveOrderPay(string ccToken, string FirstName, string LastName, string cardExpMonthMM, string cardExpYYYY, Guid cardTypeID, bool setCardasDefault, Guid OrderSummaryId, int FacilityId)
        {
            try
            {
                Debug.WriteLine("in save order");
                var order = new OrderController().GetOrderSummaryComplete(OrderSummaryId, FacilityId);
                Debug.WriteLine(order.OrderNum);
                var payment = order.OrderPayments.SingleOrDefault();
                Debug.WriteLine(payment.AccountPayMethodID);
                payment.AccountPayMethodID = Guid.Empty;
                payment.CardHolderName = FirstName + LastName; //ssl_firstname + ssl_lastname;
                payment.CardNumber = "CVTKN" + ccToken;
                payment.ExpirationDateMM = cardExpMonthMM;
                payment.ExpirationDateYYYY = cardExpYYYY;
                //if (card_short_description.Length > 0)
                //{
                //    if (cardAMEX.Checked)
                //        paymentid = cardAMEX.Value;
                //    else if (cardDisc.Checked)
                //        paymentid = cardDisc.Value;
                //    else if (cardMC.Checked)
                //        paymentid = cardMC.Value;
                //    else if (cardVisa.Checked)
                //        paymentid = cardVisa.Value;
                //}
                //else
                //    paymentid = cardVisa.Value;

                payment.PaymentTypeID = cardTypeID;

                payment.SetAsOSCDefaultPayment = setCardasDefault;

                bool paymentSuccess = new OrderController().SaveOrderPayment(payment);
                return paymentSuccess;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        private string gettoken(string cardNumber, int CVV, string cardExpMonthMM, string cardExpYYYY, int FacilityId)
        {
            Debug.WriteLine("in get token");
            Debug.WriteLine(ConfigurationSettings.AppSettings["ConvergeURL"]);
            string returnValue = "";
            try
            {
                string ssl_transaction_type = "ccgettoken";

                var facility = new FacilityController().GetFacility(FacilityId);
                string parmList = "ssl_merchant_id=" + facility.SSLMID.ToString();              // ssl_merchant_id;//742270//008203
                parmList += "&ssl_user_id=" + facility.SSLUser.ToString();              // ssl_user_id; //webpage//assistant
                parmList += "&ssl_pin=" + facility.SSLPin.ToString();                     // ssl_pin;//JYEV2C//D8RMQE
                parmList += "&ssl_transaction_type=" + ssl_transaction_type;
                parmList += "&ssl_show_form=false";
                // parmList += "&ssl_cvv2cvc2_indicator=1";
                // parmList += "&ssl_add_token=Y";
                // parmList += "&ssl_verify=Y";
                // parmList += "&ssl_cvv2cvc2=" + cardCSV.Value;
                parmList += "&ssl_card_number=" + cardNumber;
                parmList += "&ssl_exp_date=" + cardExpMonthMM + cardExpYYYY.ToString().Substring(2, 2);
                parmList += "&ssl_result_format=ASCII";
                Debug.WriteLine(parmList);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.convergepay.com/VirtualMerchant/process.do");
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
                Debug.WriteLine(convergeResponse);

                // parse converge response

                string tokenSuccessResponse = getValueForConvergeField(convergeResponse, "ssl_token_response");
                if (tokenSuccessResponse.ToLower() == "success")
                {
                    string token = getValueForConvergeField(convergeResponse, "ssl_token");
                    card_short_description = getValueForConvergeField(convergeResponse, "card_short_description");
                    Debug.Write("card_short_description = " + card_short_description);
                    returnValue = token;
                }
                else
                {
                    string errorCode = getValueForConvergeField(convergeResponse, "errorCode");
                    string errorMessage = getValueForConvergeField(convergeResponse, "errorMessage");
                    returnValue = "converge error " + errorCode;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                returnValue = "Network error. Please try later.";
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
    }

    

    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string StartDay { get; set; }
        public string DayofWeek { get; set; }
        public string Day { get; set; }
        public Guid EventId { get; set; }
        public Guid MenuId { get; set; }
    }

}
