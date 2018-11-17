using Microsoft.Practices.Unity;
using Prism.Commands;
using ProductBacklog.Composition;
using ProductBacklog.Interfaces;
using ProductBacklog.Learning.Interfaces;
using ProductBacklog.ProductLoader.Interfaces;
using ProductBacklog.ProductLoader.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProductBacklog
{
    [View("ProductBacklogView")]
    public class ProductBacklogViewModel : BaseViewModel, IProductBacklog, IWaitable, ILoadable
    {
        private BitmapImage image;
        private bool isBusy;
        private string waitMessage = "Wait...";
        private IEnumerable<IMarketData> backlogData = new IMarketData[0];
        private ICommand businessDateCommand;

        [Dependency]
        public IMarketHistoryProvider MarketHistoryProvider { get; set; }

        [Dependency]
        public IContextProvider ContextProvider { get; set; }

        [Dependency]
        public IProductCategory Categories { get; set; }

        public BitmapImage Image
        {
            get
            {
                return image;
            }
            set
            {
                SetField(ref image, value, () => Image);
            }
        }

        public ICommand BusinessDateCommand
        {
            get
            {
                return businessDateCommand;
            }
            private set
            {
                SetField(ref businessDateCommand, value, () => BusinessDateCommand);
            }
        }

        public IEnumerable<IMarketData> BacklogData
        {
            get
            {
                return backlogData;
            }
            private set
            {
                SetField(ref backlogData, value, () => BacklogData);
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                SetField(ref isBusy, value, () => IsBusy);
            }
        }

        public string WaitMessage
        {
            get
            {
                return waitMessage;
            }
            set
            {
                SetField(ref waitMessage, value, () => WaitMessage);
            }
        }

        public void OnLoaded()
        {
            LoadIcons();
            BusinessDateCommand = new DelegateCommand<object>(ExecuteBusinessDateCommand, CanExecuteBusinessDateCommand);
            ContextProvider.OnBusinessDateChanged += OnBusinessDateChanged;
        }

        public void OnUnloaded()
        {
            ContextProvider.OnBusinessDateChanged -= OnBusinessDateChanged;
        }

        private void LoadIcons()
        {
            this.IsBusy = true;
            var task = Task.Factory.StartNew(() =>
            {
                var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                var filePath = Path.Combine(folder, "Images\\calendar.png");
                return filePath;
            });
            Task.Factory.ContinueWhenAny(new[] {task}, result =>
            {
                IsBusy = false;
                this.Image = new BitmapImage(new Uri((result as Task<string>).Result));
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ExecuteBusinessDateCommand(object parameter)
        {
            var selectDates = App.Resolve<ISelectBusinessDate>();
            App.CreateDialog(selectDates);
        }

        private bool CanExecuteBusinessDateCommand(object parameter)
        {
            return true;
        }

        private void OnBusinessDateChanged(IContextProvider provider)
        {
            ReloadProductBacklogData();
        }

        //private async Task<IEnumerable<IProductBacklogItem>> ReloadProductBacklogData_1()
        //{
        //    var dates = this.ContextProvider.Date;
        //    this.WaitMessage = string.Format("AlexVB & Kids Presents.{0}Product Backlog Items for{0}[{1:dd MMM};{2:dd MMM}]",
        //        Environment.NewLine, dates.FromDate, dates.ToDate.AddDays(1));
        //    this.IsBusy = true;
        //    try
        //    {
        //        var result = await Task.Run(() =>
        //        {
        //            var items = this.ProductBacklogService
        //                .GetProductBacklogItems(dates.FromDate, dates.ToDate.AddDays(1)).ToList() as IEnumerable<IProductBacklogItem>;
        //            return items;
        //        });
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        // DO error handling here
        //    }
        //    finally
        //    {
        //        this.IsBusy = false;
        //    }
        //    return null;
        //}

        //private void OnBusinessDateChanged_2(IContextProvider provider)
        //{
        //    ReloadProductBacklogData_2();
        //}

        private void ReloadProductBacklogData()
        {
            var dates = this.ContextProvider.Date;
            this.WaitMessage = string.Format("AlexVB & Kids Presents.{0}Product Backlog Items for{0}[{1:dd MMM};{2:dd MMM}]",
                Environment.NewLine, dates.FromDate, dates.ToDate.AddDays(1));
            this.IsBusy = true;

            Task.Factory.StartNew(() =>
            {
                var marketData = MarketHistoryProvider.GetHostoricalData(this.Categories.SelectedCategory, dates.FromDate, dates.ToDate);
                return marketData.ToList();
            }).ContinueWith((result) =>
            {
                if (result.Exception == null)
                {
                    this.BacklogData = result.Result;
                }
                else
                {
                    // do error handling
                }
                IsBusy = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
