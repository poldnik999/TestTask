using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class DeliveryService
    {
        public Rider Ride = new Rider();
        public Writer Write = new Writer();

        public string LogPath;
        public string OrderPath;
        public string CityDistrict;
        public DateTime FirstDeliveryDateTime;

        private List<Order> OrderList;
        public void FilterOrders(string district, DateTime firstDeliveryTime)
        {
            try
            {
                List<Order> filteredOrders = this.Ride.ReadOrdersFromFile();
                DateTime endTime = firstDeliveryTime.AddMinutes(30);
                filteredOrders = filteredOrders.Where(o => o.District == district && o.DeliveryTime > firstDeliveryTime && o.DeliveryTime <= endTime).ToList();

                if (filteredOrders.Count == 0)
                    LogOperation.Error("Фильтрация завершена. Записей нет");
                else
                    LogOperation.Success("Фильтрация завершена. Найдено - " + filteredOrders.Count + " записей", filteredOrders);

                this.Write.WriteFilteredOrdersToFile(filteredOrders, OrderPath);
            }
            catch (DeliveryException ex)
            {
                Console.WriteLine($"Ошибка заказа: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }

            
        }
        public void SetArgs(string[] args)
        {
            if (args.Length == 0)
            {
                throw new DeliveryException("No arguments");
            }
            LogPath = args[0];
            LogOperation.LogPath = LogPath;
            LogOperation.Start(args);

            if (args.Length < 4)
            {
                LogOperation.Error("Недостаточно аргументов. |  Not enough arguments");
                throw new DeliveryException("Not enough arguments");
            }
            OrderPath = args[1];
            CityDistrict = args[2];
            if (!DateTime.TryParse(args[3], out FirstDeliveryDateTime))
            {
                LogOperation.Error("Некорректный формат времени. |  Incorrect format FirstDeliveryDateTime");
                throw new DeliveryException("Incorrect format FirstDeliveryDateTime");
            }
        }
    }
    public class DeliveryException : Exception
    {
        public DeliveryException(string message) : base(message)
        {
        }
    }
}
