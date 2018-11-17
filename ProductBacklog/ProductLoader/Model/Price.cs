namespace ProductBacklog.ProductLoader.Model
{
    public class Price : IPrice
    {
        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double AdjClose { get; set; }
    }
}
