namespace ProductBacklog.ProductLoader.Model
{
    public interface IPrice
    {
        double Open { get; }

        double Close { get; }

        double High { get; }

        double Low { get; }

        double AdjClose { get; }
    }
}
