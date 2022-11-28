using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.Utility
{
    public static class SD
    {
        public const string Proc_CoverType_create = "SP_CreateCoverType";
        public const string Proc_CoverType_Update = "SP_UpdateCoverType";
        public const string Proc_CoverType_Delete = "SP_DeleteCoverTypes";
        public const string Proc_GetCoverTypes = "SP_GetCoverTypes";
        public const string Proc_GetCoverType = "SP_GetCoverType";

        //Roles
        public const string Role_Admin = "Admin_User";
        public const string Role_Employee = "Employee_User";
        public const string Role_Company = "Company_User";
        public const string Role_Individual = "Individual_User";


        //session
        public const string Ss_session = "CartCountSession";

        //ORDER STATUS
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProgress = "InProgress";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";

        //Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "Delay Payment";
        public const string PaymentStatusRejected = "Rejected";
        


        public static double GetPriceBasedQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;

            else if (quantity < 100)
                return price50;

            else
                return price100;
        }
        public static string ConvertToRawHtml(string source)
        {
             char[] array = new char[source.Length];
             int arrayIndex = 0;
             bool inside = false;
             for(int i=0;i<source.Length;i++)
             {
                 char let = source[i];
                 if (let =='<')
                 {
                     inside = true;
                     continue;
                 }
                 if(let=='>')
                 {
                     inside = false;
                     continue;
                 }
                 if(!inside)
                 {
                     array[arrayIndex] = let;
                     arrayIndex++;
                 }
             }
             return new string(array, 0, arrayIndex);
     
        }
    }
}