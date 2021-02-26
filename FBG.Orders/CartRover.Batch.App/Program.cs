using CartRover.Batch.App.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App
{
    class Program
    {
        public static string msLogFileName;

        static void Main(string[] args)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            log.Info("Starting batch");
            CartRoverClient cartRover = new CartRoverClient();
            OrderClient orderClient = new OrderClient();
            DateTime dtLOD = orderClient.GetOrderProcessDT();
            DateTime dtfromOPD = dtLOD.AddDays(1); // TODO: Define the date
            DateTime dttoOPD = dtLOD.AddDays(2); // TODO: Define the date*/
            cartRover.GetCartRoverData(DateTime.Now,DateTime.Now.AddDays(2));

        }
        
    }
}
