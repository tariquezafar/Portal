using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public enum Roles { SuperAdmin=1,Admin=2,Other=3}
    public enum StatusMessage { Success, Fail };
    public enum FolderCreationMode { Year, Month, Day, Hour, Individual }
    public enum ProviderType { AT }
    public enum ErrorMode { Critical, Normal }
    public enum ErrorType { Error, Exception }
    public enum ProductMode { Common, Portal }
    public enum AccessMode { AddAccess=1, EditAccess=2,ViewAccess=3 , CancelAccess=4, ReviseAccess = 5}
    public enum RequestMode { GetPost=1,Ajax=2}

    public enum MailSendingMode { SaleInvoice=1, SO=2, Quotation=3, Challan=4, PO=5, BirthDay=6, PurchaseInvoice=7 }

    public enum Month
    {
        NotSet = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
