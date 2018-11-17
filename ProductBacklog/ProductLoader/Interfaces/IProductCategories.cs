using System.Collections.Generic;

namespace ProductBacklog.ProductLoader.Interfaces
{
    public interface IProductCategory
    {
        IEnumerable<string> CategoryList { get; }

        string SelectedCategory { get; }
    }
}
