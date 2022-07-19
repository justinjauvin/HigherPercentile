using HigherPercentile.Data;
using HigherPercentile.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace HigherPercentile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        User? loggedInUser;
        List<Stock> stockList = new List<Stock>();

        
        //main key: D4Rgc8EXP54nsICWqAJqb5rOMqE5EAaj28qTDddR
        //other keys: A3DMs1S8kuaXsrKpT9vYR3UVY9RIy5zF95IdljxE, FfaLPLCLTZ5JzJn3oAVto6T1rr21QPjCLF88WRZf, uTPirwT9wy5eG0jPiCtVb2Oqad2mywOEaDCsaoEj, 9MpHUODA9n1dbWR08IS2j63ct8N62Dmf2mIIJz8m
        String APIkey = "LKAzZanGo12OvTjqV31UXaRwxp4VJL9r15fe4CSB";

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;

            //make a method that sends an API request to check what the
            //status code is if the status code is good then run the subprogram

            int count = db.Stocks.Count();

            if (count==0)
            {
                populateDBWithStocks();
                updateAllStocksDataDB();
            }

            //limit of 100 API calls per day with free YF API account
            updateAllStocksDataDB();
            
        }

        public IActionResult Index()
        {
            String? username = HttpContext.Session.GetString("username");
            String? id = HttpContext.Session.GetString("id");

            if (loggedInUser != null) 
            {
                ViewData["user"] = loggedInUser;
            }
            else if (username != null) {
                loggedInUser = new User(username, Int32.Parse(id));
                ViewData["user"] = loggedInUser;
            }

            List<Stock> stockList = db.Stocks.OrderByDescending(o => o.MomentumTotal).ToList(); 
            
            ViewData["stockList"] = stockList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Watchlist watchlistItem)
        {
            if (watchlistItem.UserId == null || watchlistItem.UserId == 0)
            {
                TempData["error"] = "Need to be Logged in!";
                return RedirectToAction("Index", "Login");
            }
            else if (watchlistItem.StockId == 0)
            {
                return RedirectToAction("Index", "Watchlist");
            }

            IQueryable<Watchlist> watchlistCheck = db.Watchlists.Where(T => T.UserId==watchlistItem.UserId && T.StockId == watchlistItem.StockId);

            if (watchlistCheck.Count()>0) 
            {
                TempData["error"] = "Stock is already in your watchlist!";

                return RedirectToAction("Index", "Home");
            }

            db.Watchlists.Add(watchlistItem);
            db.SaveChanges();
            TempData["success"] = "Stock Added successfully";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Watchlist(int? userId)
        {
            if (userId == 0)
            {
                TempData["error"] = "Need to be Logged in!";
                return RedirectToAction("Index", "Watchlist");
            }

            return RedirectToAction("Index", "Watchlist");
        }

        void populateDBWithStocks()
        {
            StreamReader reader = new StreamReader(@"./ExternalData/stocks.csv");

            List<Stock>? stockList = new List<Stock>();
            //the first line is symbol, name, industry the next line skips that
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                Stock newStock = new Stock(values[0], values[1]);

                stockList.Add(newStock);
            }

            reader.Close();

            db.Stocks.AddRange(stockList);
        }

        void updateAllStocksDataDB()
        {
            stockList = db.Stocks.ToList();

            foreach (Stock stock in stockList)
            {
                
                List<String>? prices = new List<string>();

                bool awaiterResponseCheck = getStockData(stock.Symbol, prices).Result;
                
                if (!awaiterResponseCheck) { 
                    return; 
                }

                getPricePercentChange(prices, stock);
            }
            updateAllStocksDB();
        }

        public async Task<bool> getStockData(String symbol, List<String> prices)
        {
            Console.WriteLine("Entered good");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://yfapi.net/");
            httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                APIkey); //main key: D4Rgc8EXP54nsICWqAJqb5rOMqE5EAaj28qTDddR
            httpClient.DefaultRequestHeaders.Add("accept",
                "application/json");

            var response = await httpClient.GetAsync(
            $"v8/finance/chart/{symbol}?range=1y&region=US&interval=1mo&lang=en");

            if (response.StatusCode.ToString() == "TooManyRequests")
            {
                return false;
            }

            Console.WriteLine("***********************Status Code:" + response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            JObject data = (JObject)JsonConvert.DeserializeObject(responseBody);
            //Console.WriteLine(data);

            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[12]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[10]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[8]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[5]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[0]").Value<string>());

            return true;
        }

        Stock getPricePercentChange(List<String> prices, Stock stock)
        {
            double totalPercent = 0;
            stock.Price = Math.Round(Convert.ToDouble(prices[0]), 2);

            List<double> percentageChanges = new List<double>();

            for (int i = 1; i < prices.Count; i++)
            {
                double monthPercentChange = (1 - Convert.ToDouble(prices[i]) / stock.Price) * 100;
                totalPercent += monthPercentChange;
                percentageChanges.Add(monthPercentChange);
            }

            stock.OneMonthReturn = Math.Round(percentageChanges[0], 2);
            stock.ThreeMonthReturn = Math.Round(percentageChanges[1], 2);
            stock.SixMonthReturn = Math.Round(percentageChanges[2], 2);
            stock.OneYearReturn = Math.Round(percentageChanges[3], 2);

            stock.MomentumTotal = Math.Round(totalPercent / prices.Count - 1, 2);

            return stock;
        }

        void updateAllStocksDB()
        {
            db.Stocks.UpdateRange(stockList);
            db.SaveChanges();
        }

        public async Task<bool> checkIfapiAvailable()
        {
            Console.WriteLine("Entered good");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://yfapi.net/");
            httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                APIkey); //main key: D4Rgc8EXP54nsICWqAJqb5rOMqE5EAaj28qTDddR
            httpClient.DefaultRequestHeaders.Add("accept",
                "application/json");

            var response = await httpClient.GetAsync(
            $"v8/finance/chart/AAPL?range=1y&region=US&interval=1mo&lang=en");

            if (response.StatusCode.ToString() == "TooManyRequests")
            {
                return false;
            }

            return true;
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
