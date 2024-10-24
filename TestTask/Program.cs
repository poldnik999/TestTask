using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class Order
    {
        public string OrderId { get; set; }
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
    internal class Program
    {
        private static string OrderPath = "orders.csv";
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Нет аргументов.");
                return;
            }
            string deliveryLogPath = args[0];
            LogOperation.LogPath = deliveryLogPath;
            LogOperation.Start(args);

            if (args.Length < 4)
            {
                LogOperation.Error("Недостаточно аргументов.");
                return;
            }
            string deliveryOrderPath = args[1];
            string cityDistrict = args[2];
            DateTime firstDeliveryDateTime;

            
            
            if (!DateTime.TryParse(args[3], out firstDeliveryDateTime))
            {
                LogOperation.Error("Некорректный формат времени.");
                return;
            }
            
            List<Order> orders = ReadOrdersFromFile(OrderPath);

            List<Order> filteredOrders = FilterOrders(orders, cityDistrict, firstDeliveryDateTime);
            WriteFilteredOrdersToFile(filteredOrders, deliveryOrderPath);
            if(filteredOrders.Count ==0)
                LogOperation.Error("Фильтрация завершена. Записей нет");
            else
                LogOperation.Success("Фильтрация завершена. Найдено - " + filteredOrders.Count + " записей", filteredOrders);
        }

        // Чтение таблицы из файла
        private static List<Order> ReadOrdersFromFile(string filePath)
        {
            List<Order> ordersList = new List<Order>();
            List<Order> exOrderList = new List<Order>();
            
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var valuesOrder = line.Split(';');

                    Order order = new Order();
                    order.OrderId = valuesOrder[0];
                    order.Weight = Convert.ToDouble(valuesOrder[1]);
                    order.District = valuesOrder[2];
                    order.DeliveryTime = Convert.ToDateTime(valuesOrder[3]);

                    // Валидация значений в таблице orders
                    if(ValidationOrder.Validate(order).Count > 0)
                    {
                        LogOperation.OrderError(ValidationOrder.Validate(order), line);
                    }
                    else
                    {
                        ordersList.Add(order);
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

        // Фильтрация заказов по времени и месту
        private static List<Order> FilterOrders(List<Order> orders, string district, DateTime firstDeliveryTime)
        {
            DateTime endTime = firstDeliveryTime.AddMinutes(30);
            return orders.Where(o => o.District == district && o.DeliveryTime > firstDeliveryTime && o.DeliveryTime <= endTime).ToList();
        }

        // Запись результата фильтрации в файл
        private static void WriteFilteredOrdersToFile(List<Order> filteredOrders, string filePath)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                for(int i=0;i< filteredOrders.Count; i++)
                {
                    var order = filteredOrders[i];
                    streamWriter.WriteLine(order.OrderId + ';' + order.Weight + ';' + order.District + ';' + order.DeliveryTime);
                }
            }
        }

    }
}
