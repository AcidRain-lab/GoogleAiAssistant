using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AbsModels
{
    public class OrderResponse
    {
        public RequestInfo Request { get; set; }
        public IEnumerable<OrderDetails> Orders { get; set; }

        public class RequestInfo
        {
            public string BatchId { get; set; }
            public DateTime ReceivedTime { get; set; }
            public int OrdersReceived { get; set; }
            public int OrdersFailed { get; set; }
            public int OrdersSucceeded { get; set; }
        }

        public class OrderDetails
        {
            public string RequestId { get; set; }
            public string ConfirmationNumber { get; set; }
            public string Message { get; set; }
        }
    }

}
