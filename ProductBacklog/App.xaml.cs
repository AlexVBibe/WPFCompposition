using Microsoft.Practices.Unity;
using ProductBacklog.Composition;
using ProductBacklog.Interfaces;
using ProductBacklog.Learning.Interfaces;
using ProductBacklog.Learning.ViewModels;
using ProductBacklog.ProductLoader.Interfaces;
using ProductBacklog.ProductLoader.Services;
using ProductBacklog.ProductLoader.ViewModels;
using System.Windows;

namespace ProductBacklog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer GlobalContainer;

        protected override void OnExit(ExitEventArgs e)
        {
            var registry = GlobalContainer.Resolve<ICompositeViewRegistry>();
            var views = registry.Views;
            foreach (var view in views)
            {
                if ((view as CompositeView).DataContext is ILoadable)
                {
                    var loadable = (view as CompositeView).DataContext as ILoadable;
                    loadable.OnUnloaded();
                }
            }
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            App.GlobalContainer = new UnityContainer();
            GlobalContainer.RegisterType<ICompositeViewRegistry, CompositeViewRegistry>(new ContainerControlledLifetimeManager());

            GlobalContainer.RegisterType<IShellViewModel, ShellViewModel>(new ContainerControlledLifetimeManager());
            GlobalContainer.RegisterType<IProductBacklog, ProductBacklogViewModel>(new ContainerControlledLifetimeManager());
            GlobalContainer.RegisterType<ISelectBusinessDate, SelectBusinessDateViewModel>(new ContainerControlledLifetimeManager());
            GlobalContainer.RegisterType<IHostWindowViewModel, HostWindowViewModel>(new ContainerControlledLifetimeManager());
            GlobalContainer.RegisterType<IContextProvider, ContextProviderViewModel>(new ContainerControlledLifetimeManager());
            GlobalContainer.RegisterType<IProductCategory, ProductCategoryViewModel>(new ContainerControlledLifetimeManager());

            GlobalContainer.RegisterType<IMarketHistoryProvider, MarketHistoryProvider>(new ContainerControlledLifetimeManager());
            base.OnStartup(e);
        }

        public static T Resolve<T>() where T : class
        {
            return GlobalContainer.Resolve<T>();
        }

        public static T CreateDialog<T>() where T : class
        {
            var window = new HostWindow();
            var result = GlobalContainer.Resolve<IHostWindowViewModel>();
            result.Content = GlobalContainer.Resolve<T>();
            window.DataContext = result;
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
            return result.Content as T;
        }

        public static bool? CreateDialog<T>(T viewModel) where T : class
        {
            var window = new HostWindow();
            var hostViewModel = GlobalContainer.Resolve<IHostWindowViewModel>();
            hostViewModel.Content = viewModel;
            window.DataContext = hostViewModel;
            window.Owner = App.Current.MainWindow;

            if (viewModel is IClosable)
                (viewModel as IClosable).OnClose = (result) =>
                {
                    window.DialogResult = result;
                    window.Close();
                };
            return window.ShowDialog();
        }
    }
}
