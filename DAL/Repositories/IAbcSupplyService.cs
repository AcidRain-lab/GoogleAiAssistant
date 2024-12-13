using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.AbsModels;

namespace DAL.Repositories
{
    public interface IAbcSupplyService
    {
        Task<OrderResponse> PlaceOrderAsync(OrderRequest orderRequest);
        Task<IEnumerable<Order>> GetOrdersAsync(string confirmationNumber);
    }
}
