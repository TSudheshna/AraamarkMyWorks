using System;
using Aramark.SuiteCatering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Aramark.SuiteCatering.SuiteWizardAPI;

namespace APITest
{
    [TestClass]
    public class ServiceTest
    {

        [TestMethod]
        public void ErrorsComment()
        {
            //writing unit test to verify that api works 
            var order = new OrderController().GetOrderSummaryComplete(new Guid("f5183060-4950-4a30-93ec-1f3f69291c35"), 96);
            order.Comments = "test:" + DateTime.Now.ToString();
            Debug.Write(order.Comments);
            var success = new OrderController().SaveOrder(order);

            Debug.Write("Saving result:"+success);


            var order1 = new OrderController().GetOrderSummaryComplete(new Guid("f5183060-4950-4a30-93ec-1f3f69291c35"), 96);
            Debug.Write("Loaded:"+order1.Comments);
        }


        [TestMethod]
        public void ErrorsTest()
        {
            var service = new FacilityController();
            var errors=   service.GetErrorLogByFacilityOrDateRange(59, DateTime.Now.AddDays(-1000), DateTime.Now).ToList();
            //it's always empty
            foreach (var item in errors)
            {
                Debug.Write(item.ErrorMessage);   
            }

        }
        [TestMethod]
        public void CalorieGetMenuItemTest()
        {
            //GetMenuItemDetail(int facilityId, Guid menuItemId, Guid eventId, Guid customerId)
            // this is cross sell calls {facilityId: '98', menuDetailId: '56aee772-5545-4dd2-b257-0bf014bfe555', eventId: '9f1c11c7-772a-4556-ac75-29633d95f37e', customerId: '5748aa39-0f0b-420b-beae-6bc09ff3af26'}
            // item id is package , but I have to use item id as item //White Herb Pizza -- c1e33ce7-1f1d-11d7-8e56-00508bca2b32
            //
            
            var item = new MenuController().GetMenuItemDetail(98, new Guid("c1e33ce7-1f1d-11d7-8e56-00508bca2b32"), new Guid("9f1c11c7-772a-4556-ac75-29633d95f37e"), new Guid("5748aa39-0f0b-420b-beae-6bc09ff3af26"));
            foreach (var menuItem in item)
            {
                //menuItem.ShortDescription = HttpUtility.HtmlDecode(menuItem.ShortDescription);
                Debug.Write(menuItem.ShortDescription);
            }
            //now I have to translate it into json call.


        }

        [TestMethod]
        public void CalorieGetMenuItemCrossSellListTest()
        {

            var item = new PackageController().GetMenuItemCrossSellList(98, new Guid("56aee772-5545-4dd2-b257-0bf014bfe555"));
            foreach (var menuItem in item)
            {
                //menuItem.ShortDescription = HttpUtility.HtmlDecode(menuItem.ShortDescription);
                Debug.Write(menuItem.ShortDescription);

            }
           // var re = item.FindAll(t => t.MenuItemNutritionFacts.Count > 0);

            //{facilityId: '98', menuDetailId: '56aee772-5545-4dd2-b257-0bf014bfe555', eventId: '9f1c11c7-772a-4556-ac75-29633d95f37e', customerId: '5748aa39-0f0b-420b-beae-6bc09ff3af26'}

        }

        [TestMethod]
        public void CalorieTest()
        {

            var customerId = new Guid("5748aa39-0f0b-420b-beae-6bc09ff3af26");

            var eventId = new Guid("3e1be3f1-6402-4500-a36f-c99e85193eaf");

            int facilityId = 98;

            var packageId = new Guid("3efef491-da79-4b38-9822-750d99d7ed9b");

            var item = new PackageController().GetPackageDetail(facilityId, customerId, packageId, eventId);
            if (!String.IsNullOrEmpty(item.ShortDescription)) item.ShortDescription = HttpUtility.HtmlDecode(item.ShortDescription);
            foreach (var menuItem in item.PackageMenuItems)
            {
                menuItem.ShortDescription = HttpUtility.HtmlDecode(menuItem.ShortDescription);

            }
            //item.PackageMenuItems = item.PackageMenuItems.OrderBy(v => v.CategorySort ?? 0).ToList();
            var re = item.PackageMenuItems.FindAll(t => t.MenuItemNutritionFacts.Count > 0);

            Debug.WriteLine("found" + re.Count());
            //Context.Cache.Insert(String.Format("PackageItem{0}{1}{2}{3}", packageId, facilityId, customerId, eventId), item, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            //return item;
        }


        [TestMethod]
        public void AuthenticateUserPass()
        {
            const int facilityId = 98;
            var userController = new UserController();
            var account = userController.AuthenticateUser(facilityId, "Angelo", "Angelo");

            Assert.IsNotNull(account, "Account Authenticated Passed");
        }

        [TestMethod]
        public void AuthenticateUserFail()
        {
            const int facilityId = 98;
            var userController = new UserController();
            var account = userController.AuthenticateUser(facilityId, "Ajib", "10ajib01");

            Assert.IsNull(account);
        }

        [TestMethod]
        public void UserProfilePass()
        {
            const int facilityId = 98;
            var userController = new UserController();
            var account = userController.AuthenticateUser(facilityId, "Angelo", "Angelo");
            var accountProfile = userController.GetAccountProfile(facilityId, account.AccountID);
            Assert.IsNotNull(accountProfile);
        }

        [TestMethod]
        public void UserProfileFail()
        {
            const int facilityId = 98;
            var userController = new UserController();
            var accountProfile = userController.GetAccountProfile(facilityId, Guid.NewGuid());
            Assert.IsNull(accountProfile);
        }

        [TestMethod]
        public void GetFacilityPass()
        {
            const int facilityId = 98;
            var facilityController = new FacilityController();
            var facility = facilityController.GetFacility(facilityId);
            Assert.IsNotNull(facility);
        }

        [TestMethod]
        public void GetFacilityFail()
        {
            const int facilityId = -1;
            var facilityController = new FacilityController();
            var facility = facilityController.GetFacility(facilityId);
            Assert.IsNull(facility);
        }
    }
}
