using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AbsModels
{
    public class OrderRequest
    {
        public string RequestId { get; set; }
        public string PurchaseOrder { get; set; }
        public string BranchNumber { get; set; }
        public string DeliveryService { get; set; }
        public string TypeCode { get; set; }
      
    }

}
