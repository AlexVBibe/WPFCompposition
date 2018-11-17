using System;

namespace ProductBacklog.ProductLoader.Model
{
    public interface IMarketData
    {
        DateTime Date { get; }

        double Volume { get; }

        IPrice Price { get; }
    }
}
