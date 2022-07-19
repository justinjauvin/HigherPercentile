using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HigherPercentile.Data;
using HigherPercentile.Models;

namespace HigherPercentile.Processing
{
    public class ProcessStocks
    {
        /*
        List<string> names = new List<string>();
        List<string> symbols = new List<string>();

        private readonly ApplicationDbContext db;

        public ProcessStocks()
        {
            //this.db = db;

            getDataFromFile();
            addDataToDB();

            //next need to add stock names and symbols to the DB
            //check the create method from the other project for reference
        }

        void getDataFromFile() {
            StreamReader reader = new StreamReader(@"../ExternalData/constituents.csv");

            //the first line is symbol, name, industry the next line skips that
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                symbols.Add(values[0]);
                names.Add(values[1]);
            }

            reader.Close();

            //debug
            foreach (String s in names) 
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(symbols.ElementAt(0));
            Console.WriteLine(names.ElementAt(0));
            Console.WriteLine(symbols.Count);
            Console.WriteLine(names.Count);
            
        }

        void addDataToDB() 
        {
            int count = symbols.Count;

            for (int i = 0; i <count; i++)
            {
                Stock newStock = new Stock(symbols.ElementAt(i), names.ElementAt(i));
                db.Stocks.Add(newStock);
                db.SaveChanges();
            }

            Console.WriteLine("add to db time");
        }

        static void Main(string[] args)
        {
            ProcessStocks test = new ProcessStocks();
        }
    */
    }



}
/*
 
public async Task<List<String>> getStockData(Stock stock)
        {
            Console.WriteLine("Entered good");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://yfapi.net/");
            httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                "D4Rgc8EXP54nsICWqAJqb5rOMqE5EAaj28qTDddR");
            httpClient.DefaultRequestHeaders.Add("accept",
                "application/json");

            var response = await httpClient.GetAsync(
            "v8/finance/chart/AAPL?range=1y&region=US&interval=1mo&lang=en");

            Console.WriteLine("***********************Status Code:" + response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            JObject data = (JObject)JsonConvert.DeserializeObject(responseBody);
            Console.WriteLine(data);


            String currentPrice = data.SelectToken("chart.result[0].indicators.quote[0].close[12]").Value<string>();



            //Console.WriteLine(data);
            Console.WriteLine("**********Current Price: " + currentPrice);

            List<String> prices = new List<String>();

            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[12]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[10]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[8]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[5]").Value<string>());
            prices.Add(data.SelectToken("chart.result[0].indicators.quote[0].close[0]").Value<string>());
            
            //adding all the prices to the list
            /*
            for (int i = 0; i <13; i++) 
            {
                String price = data.SelectToken($"chart.result[0].indicators.quote[0].close[{i}]").Value<string>();

                prices.Add(price);
            }
            


    return prices;
        }

 */