using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal static class ValidationOrder
    {
        private static string exDuplID;

        // Доступный метод для проверки данных заказа
        public static List<string> Validate(Order order)
        {
            var errors = new List<string>();

            ValidateOrderId(order.OrderId, errors);
            ValidateWeight(order.Weight, errors);
            ValidateDistrict(order.District, errors);
            ValidateDeliveryTime(order.DeliveryTime, errors);
            return errors;
        }
        // Доступный метод, возвращающий заказы с дублирующим ID
        public static List<Order> Validate(List<Order> orders)
        {
            var duplicateOrders = orders
            .GroupBy(o => o.OrderId)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .ToList();
            return duplicateOrders;
        }
        //4 метода на проверку полей
        private static void ValidateOrderId(string orderId, List<string> errors)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                errors.Add("Пустое поле ID в записи");
            }
        }

        private static void ValidateWeight(double weight, List<string> errors)
        {
            if (weight <= 0)
            {
                errors.Add("Вес должен быть больше 0");
            }
        }

        private static void ValidateDistrict(string district, List<string> errors)
        {
            if (string.IsNullOrEmpty(district))
            {
                errors.Add("Район не может быть пустым");
            }
        }

        private static void ValidateDeliveryTime(DateTime deliveryTime, List<string> errors)
        {
            if (deliveryTime == default)
            {
                errors.Add("Некорректная дата доставки");
            }
        }
    }
}
