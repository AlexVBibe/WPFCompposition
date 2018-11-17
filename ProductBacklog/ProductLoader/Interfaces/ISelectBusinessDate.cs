using System;

namespace ProductBacklog.Learning.Interfaces
{
    public interface ISelectBusinessDate
    {
        DateTime ToDate { get; set; }

        DateTime FromDate { get; set; }

        string RelativeDates { get; set; }
    }
}
