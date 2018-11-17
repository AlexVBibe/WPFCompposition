using ProductBacklog.Composition;

namespace ProductBacklog
{
    [View("HostWindow")]
    public class HostWindowViewModel : BaseViewModel, IHostWindowViewModel
    {
        private object content;

        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                this.OnPropertyChanged(() => Content);
            }
        }
    }
}
