using Microsoft.Practices.Unity;
using ProductBacklog.Composition;
using ProductBacklog.Interfaces;
using ProductBacklog.Learning.Interfaces;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace ProductBacklog.Learning.ViewModels
{
    [View("SelectBusinessDateView")]
    public class SelectBusinessDateViewModel : BaseViewModel, ISelectBusinessDate, IClosable, ILoadable
    {
        public static string ToDateLessOrEqaulFromDate = string.Format("'From Date' must be less than 'To Date'{0}otherwise you will get empty result.", Environment.NewLine);

        private DateTime toDate;
        private DateTime fromDate;
        private string errorCode;
        private Visibility errorCodeVisibility;
        private string relativeDates;
        private ICommand todayCommand;
        private ICommand submitCommand;

        public SelectBusinessDateViewModel()
        {
            this.TodayCommand = new DelegateCommand<object>(ExecuteTodayHandler, (p) => true);
            this.SubmitCommand = new DelegateCommand<object>(ExecuteSubmitHandler, (p) => true);
        }

        [Dependency]
        public IContextProvider ContextProvider { get; set; }

        public ICommand TodayCommand
        {
            get
            {
                return todayCommand;
            }
            set
            {
                SetField(ref todayCommand, value, () => TodayCommand);
            }
        }

        public ICommand SubmitCommand
        {
            get
            {
                return submitCommand;
            }
            set
            {
                SetField(ref submitCommand, value, () => SubmitCommand);
            }
        }

        public DateTime ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                if (SetField(ref toDate, value, () => ToDate))
                    PropertyChangedCallback();
            }
        }

        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                if (SetField(ref fromDate, value, () => FromDate))
                    PropertyChangedCallback();
            }
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                SetField(ref errorCode, value, () => ErrorCode);
            }
        }

        public Visibility ErrorCodeVisibility
        {
            get
            {
                return errorCodeVisibility;
            }
            set
            {
                SetField(ref errorCodeVisibility, value, () => ErrorCodeVisibility);
            }
        }

        public string RelativeDates
        {
            get
            {
                return relativeDates;
            }
            set
            {
                if (SetField(ref relativeDates, value, () => RelativeDates))
                    RelativeDatePropertyChanged();
            }
        }

        public Action<bool> OnClose { get; set; }

        private void PropertyChangedCallback()
        {
            var today = DateTime.Today;
            var ctrl = this;
            if (ctrl.ToDate < ctrl.FromDate)
            {
                ctrl.ErrorCode = ToDateLessOrEqaulFromDate;
                ctrl.ErrorCodeVisibility = Visibility.Visible;
            }
            else if (ctrl.FromDate > today || today > ctrl.ToDate)
            {
                ctrl.ErrorCode = string.Empty;
                ctrl.ErrorCodeVisibility = Visibility.Collapsed;
            }
            else
            {
                ctrl.ErrorCode = string.Empty;
                ctrl.ErrorCodeVisibility = Visibility.Collapsed;
            }

            int d1 = (int)(ctrl.FromDate - today).TotalDays;
            int d0 = (int)(ctrl.ToDate - today).TotalDays;
            ctrl.RelativeDates = string.Format("{0} {1}", d1, d0);
        }

        private void RelativeDatePropertyChanged()
        {
            var relativeDates = this.RelativeDates;
            var dateControl = this;
            int value = 0;
            if (!string.IsNullOrEmpty(relativeDates))
            {
                var values = relativeDates.Split(new char[] { ' ' }, StringSplitOptions.None);
                switch (values.Length)
                {
                    case 0:
                        break;
                    case 1:
                        if (Int32.TryParse(values[0], out value))
                            dateControl.FromDate = DateTime.Today.AddDays(value);
                        break;
                    default:
                        if (Int32.TryParse(values[1], out value))
                            dateControl.ToDate = DateTime.Today.AddDays(value);
                        if (Int32.TryParse(values[0], out value))
                            dateControl.FromDate = DateTime.Today.AddDays(value);
                        break;
                }
            }
        }

        private void ExecuteTodayHandler(object parameter)
        {
            this.ToDate = DateTime.Today;
            this.FromDate = this.ToDate;
        }

        private void ExecuteSubmitHandler(object parameter)
        {
            if (OnClose != null)
            {
                ContextProvider.Date = new BusinessDates(this.ToDate, this.FromDate);
                OnClose(true);
            }
        }

        public void OnLoaded()
        {
            this.ToDate = ContextProvider.Date.ToDate;
            this.FromDate = ContextProvider.Date.FromDate;
        }

        public void OnUnloaded()
        {
        }
    }
}
