using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Aramark.SuiteCatering.SuiteWizardAPI;

namespace Aramark.SuiteCatering
{
    public class OrderController : BaseController
    {        
        public OrderSummary GetOrderSummaryInfo(Guid orderSummaryId, int facilityId)
        {
            using (var service = Instance())
            {
                return service.GetOrderSummaryInfo(orderSummaryId, facilityId);
            }
        }

        public List<OSCPayMethod> GetOSCPaymentMethods(Guid? accountId, int facilityId)
        {
            using (var service = Instance())
            {
                return service.GetOSCPaymentMethods(facilityId, accountId);
            }
        }

        public OrderSummary GetOrderSummaryComplete(Guid orderSummaryId, int facilityId)
        {
            using (var service = Instance())
            {
                return service.GetOrderSummaryComplete(orderSummaryId, facilityId);
            }
        }

        public Guid CreateOrder(OrderSummary oOrderSummary)
        {
            using (var service = Instance())
            {
                oOrderSummary.LastUpdated = DateTime.Now;
                return service.CreateOrder(oOrderSummary);
            }
        }

        public Guid SaveOrderDetail(OrderDetail oOrderDetail)
        {
            using (var service = Instance())
            {
                return service.SaveOrderDetail(oOrderDetail);
            }
        }
        
        public bool UpdateOrderQuantities(OrderSummary oOrderSummary)
        {
            using (var service = Instance())
            {
                return service.UpdateCartQuantities(oOrderSummary);
            }
        }

        public bool SaveOrder(OrderSummary orderSummary)
        {
            using (var service = Instance())
            {
                return service.SaveOrder(orderSummary);
            }
        }

        public bool SaveOrderPayment(OrderPayment orderPayment)
        {
            using (var service = Instance())
            {
                return service.SaveOrderPayment(orderPayment);
            }
        }

    }
}