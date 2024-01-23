using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BLL
{
    public class OrderItemBLL
    {
        private OrderItemDAL _orderItemDAL;

        public OrderItemBLL()
        {
            _orderItemDAL = new OrderItemDAL();
        }

        public void addOrderItem(OrderItemDTO orderItem)
        {
            _orderItemDAL.create(orderItem);
        }
        
        public List<OrderItemDTO> getAllOrderItemByOrderId(int orderID)
        {
            var result = _orderItemDAL.getAll(orderID);
            return result;
        }

        public void deleteOrderItemByOrderId(int orderID)
        {
            _orderItemDAL.delete(orderID);
        }
    }
}
