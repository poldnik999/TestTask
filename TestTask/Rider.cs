using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class Rider
    {
        public List<Order> ReadOrdersFromFile()
        {
            Order order = new Order();
            List<Order> ordersList = new List<Order>();
            //List<Order> exOrderList = new List<Order>();

            using (StreamReader streamReader = new StreamReader(order.Path))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var valuesOrder = line.Split(';');

                    order = new Order();
                    order.OrderId = valuesOrder[0];
                    order.Weight = Convert.ToDouble(valuesOrder[1]);
                    order.District = valuesOrder[2];
                    order.DeliveryTime = Convert.ToDateTime(valuesOrder[3]);

                    // Валидация значений в таблице orders
                    try
                    {
                        ValidationOrder.Validate(order);
                        ordersList.Add(order);
                        //LogOperation.OrderError(ValidationOrder.Validate(order), line);
                    }
                    catch(DeliveryException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        LogOperation.OrderError(ex.Message, line);
                    }

                }
            }

            // Проверка на дублирующие ID, исключение дублирующих записей из выборки
            if (ValidationOrder.Validate(ordersList).Count > 1)
            {
                LogOperation.OrderError(ValidationOrder.Validate(ordersList));
                List<Order> difference = ordersList.Where(o1 => !ValidationOrder.Validate(ordersList).Any(o2 => o2.OrderId == o1.OrderId)).ToList();
                return difference;
            }
            return ordersList;
        }
    }
}
