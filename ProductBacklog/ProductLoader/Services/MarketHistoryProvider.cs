using ProductBacklog.ProductLoader.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using ProductBacklog.ProductLoader.Model;
using System.Globalization;

namespace ProductBacklog.ProductLoader.Services
{
    public class MarketHistoryProvider : IMarketHistoryProvider
    {
        private readonly string urlTemplate = "http://ichart.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv";

        public IEnumerable<IMarketData> GetHostoricalData(string symbol, DateTime from, DateTime to)
        {
            var a = from.Month - 1;
            var b = from.Day;
            var c = from.Year;
            var d = to.Month - 1;
            var e = to.Day;
            var f = to.Year;

            var url = string.Format(urlTemplate, symbol, a, b, c, d, e, f);
            var data = QueryServer(url);

            if (!string.IsNullOrEmpty(data))
            {
                var strings = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var item in strings.Skip(1))
                {
                    var values = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var dt = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    yield return new MarketData
                    {
                        Date = dt,
                        Volume = double.Parse(values[5]),
                        Price = new Price
                        {
                            Open = double.Parse(values[1]),
                            High = double.Parse(values[2]),
                            Low = double.Parse(values[3]),
                            Close = double.Parse(values[4]),
                            AdjClose = double.Parse(values[6])
                        }
                    };
                }
            }
        }


        private string QueryServer(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }
    }
}
