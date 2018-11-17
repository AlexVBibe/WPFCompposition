using System;

namespace ProductBacklog.ProductLoader.Model
{
    public class MarketData : IMarketData
    {
        public DateTime Date { get; set; }

        public double Volume { get; set; }

        public IPrice Price { get; set; }
    }
}
