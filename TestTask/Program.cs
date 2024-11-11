using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            DeliveryService deliveryService = new DeliveryService();
            deliveryService.SetArgs(args);

            string deliveryLogPath = deliveryService.LogPath;
            string deliveryOrderPath = deliveryService.OrderPath;
            string cityDistrict = deliveryService.CityDistrict;
            DateTime firstDeliveryDateTime = deliveryService.FirstDeliveryDateTime;

            deliveryService.FilterOrders(cityDistrict, firstDeliveryDateTime);

        }

    }
}
