using ProductBacklog.ProductLoader.Model;
using System;
using System.Collections.Generic;

namespace ProductBacklog.ProductLoader.Interfaces
{
    public interface IMarketHistoryProvider
    {
        IEnumerable<IMarketData> GetHostoricalData(string symbol, DateTime from, DateTime to);
    }
}
