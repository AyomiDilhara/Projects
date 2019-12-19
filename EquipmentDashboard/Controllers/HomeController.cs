using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EquipmentDashboard.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace EquipmentDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        
        {
            string reqUrl = "http://ivivaanywhere.ivivacloud.com/api/Asset/Asset/All?apikey=SC:ivivademo:8d756202d6159375&max=100&last=0";
            var httpWebRequestQR = (HttpWebRequest)WebRequest.Create(reqUrl);
            httpWebRequestQR.ContentType = "application/json";
            httpWebRequestQR.Method = "GET";


            var httpResponseQR = (HttpWebResponse)httpWebRequestQR.GetResponse();
            using (var streamReader = new StreamReader(httpResponseQR.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //string jsonStringsign = resultQR;
                //var json = new JavaScriptSerializer().Serialize(jsonStringsign);
                //JsonData json = JsonMapper.ToObject(jsonStringsign);
                //Console.WriteLine(json);
                Console.WriteLine(result);
                var jsonResult=JsonConvert.DeserializeObject<Result[]>(result);

                int operationalCount = 0;
                int nonOperatinalCount = 0;

                List<string> asset = new List<string>();

                foreach (Result x in jsonResult)
                {
                    if(x.OperationalStatus.Equals("Operational"))
                    {
                        operationalCount++;
                    } else
                    {
                        nonOperatinalCount++;
                    }
                    asset.Add(x.AssetCategoryID);
                }

                var g = asset.GroupBy(i => i);
                List<string> keyList = new List<string>();

                Dictionary<string, int> hash = new Dictionary<string, int>();
                foreach (var grp in g)
                {
                    hash.Add(grp.Key, grp.Count());
                    keyList.Add(grp.Key);
                }

                Console.WriteLine("===========================================");

                Console.WriteLine(hash[keyList[0]]);

                var processedResult = new ProcessedResult();
                processedResult.operatinalCount = operationalCount;
                processedResult.nonOperatinalCount = nonOperatinalCount;
                processedResult.keyValues = hash;
                processedResult.keys = keyList;

                return View(processedResult);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
