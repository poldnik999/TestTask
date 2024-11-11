using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal static class LogOperation
    {
        public static string LogPath = "logDefault.txt";
        public static int OrdersBound = 5;

        // Граница использования внутри файла логирования
        public static void Start(string[] args)
        {
            string argsString = "";
            foreach(string arg in args)
                argsString += " "+arg;
            LogWriter("\n++++++++++++++++++++++++++++\n" +
                "Использование: " + DateTime.Now + " Аргументы:" + argsString);
        }

        public static void Error(string exMessage)
        {
            LogWriter("Ошибка: " + exMessage);
        }
        // В случае успешной фильтрации выводит в лог первые 5 фильтрованных заказов
        public static void Success(string message, List<Order> filteredOrders)
        {
            LogWriter("Успех: " + message);
            for(int i = 0;i < filteredOrders.Count && i < OrdersBound; i++)
            {
                if (i < OrdersBound)
                {
                    LogWriter(" ***  "+
                    filteredOrders[i].OrderId + "; " +
                    filteredOrders[i].Weight + "; " +
                    filteredOrders[i].District + "; " +
                    filteredOrders[i].DeliveryTime
                    );
                }
                else LogWriter("... еще " + (filteredOrders.Count - OrdersBound) + " записей");


            }

        }

        // Выводит ошибки, связанные с файлом orders
        public static void OrderError(string exMessages, string exOrder)
        {
            LogWriter("\t * Обнаружены ошибки в файле orders: ");
            LogWriter("\t\t Некорректная строка : " + exOrder);
        }
        // Выводит ошибки, связанные с файлом orders
        public static void OrderError(List<Order> exOrders)
        {
            LogWriter("\t * Обнаружены дубликаты ID в файле orders: ");
            foreach (Order o in exOrders)
            {
                LogWriter("\t\t Некорректная строка : " + 
                    o.OrderId + "; " +
                    o.Weight + "; " +
                    o.District + "; " +
                    o.DeliveryTime);
            }
            
        }

        // Внутренний метод для записи в файл логов
        private static void LogWriter(string logMessage)
        {
            Console.WriteLine(logMessage);
            using (StreamWriter sw = new StreamWriter(LogPath, true))
            {
                sw.WriteLine(logMessage);
            }
        }
    }
}
