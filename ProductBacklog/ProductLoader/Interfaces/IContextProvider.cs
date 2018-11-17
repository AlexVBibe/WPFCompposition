using System;

namespace ProductBacklog.Learning.Interfaces
{
    public struct BusinessDates
    {
        public DateTime ToDate { get; }
        public DateTime FromDate { get; }

        public BusinessDates(DateTime to, DateTime from)
        {
            ToDate = to;
            FromDate = from;
        }
    }

    public delegate void BusinessDateChanged(IContextProvider provider);

    public interface IContextProvider
    {
        BusinessDates Date { get; set; }

        event BusinessDateChanged OnBusinessDateChanged;
    }
}
