using Microsoft.Practices.Unity;
using ProductBacklog.Composition;
using ProductBacklog.Learning.Interfaces;

namespace ProductBacklog
{
    [View("ShellView")]
    public class ShellViewModel : BaseViewModel, IShellViewModel
    {
        public ShellViewModel()
        {
                
        }

        [Dependency]
        public IContextProvider ContextProvider { get; set; }

        [Dependency]
        public IProductBacklog Client { get; set; }
    }
}
