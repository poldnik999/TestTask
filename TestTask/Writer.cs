using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class Writer
    {
        public void WriteFilteredOrdersToFile(List<Order> filteredOrders, string filePath)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                for (int i = 0; i < filteredOrders.Count; i++)
                {
                    var order = filteredOrders[i];
                    streamWriter.WriteLine(order.OrderId + ';' + order.Weight + ';' + order.District + ';' + order.DeliveryTime);
                }
            }
        }
    }
}
