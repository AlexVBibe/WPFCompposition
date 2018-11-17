using ProductBacklog.Composition;
using ProductBacklog.Interfaces;
using ProductBacklog.ProductLoader.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProductBacklog.ProductLoader.ViewModels
{
    [View("ProductCategoryView")]
    public class ProductCategoryViewModel : BaseViewModel, IProductCategory, ILoadable
    {
        private string selectedCategory;

        public ProductCategoryViewModel()
        {
            var query = from number in Enumerable.Range(1, 5) select number;
        }

        public IEnumerable<string> CategoryList { get; private set; }

        public void OnLoaded()
        {
            this.CategoryList = new string[] { "VOD.L", ".FTSE", "GOOG", "BT.L", "BARC.L", "HSBC.L", "UBS", "C", "BP.L" };
            this.OnPropertyChanged(() => CategoryList);
        }

        public void OnUnloaded()
        {
        }

        public string SelectedCategory
        {
            get
            {
                return this.selectedCategory;
            }
            set
            {
                this.SetField(ref selectedCategory, value, () => SelectedCategory);
            }
        }
    }
}
