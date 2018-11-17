using ProductBacklog.Composition;
using ProductBacklog.Interfaces;
using ProductBacklog.Learning.Interfaces;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace ProductBacklog.Learning.ViewModels
{
    [View("ContextProviderView")]
    public class ContextProviderViewModel : BaseViewModel, IContextProvider, ILoadable
    {
        private DateTime toDate;
        private DateTime fromDate;
        private ICommand applyDatesCommand;

        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                SetField(ref toDate, value, () => ToDate);
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
                SetField(ref fromDate, value, () => fromDate);
            }
        }

        public ICommand ApplyDatesCommand
        {
            get
            {
                return applyDatesCommand;
            }
            set
            {
                SetField(ref applyDatesCommand, value, () => ApplyDatesCommand);
            }
        }

        public BusinessDates Date
        {
            get
            {
                return new BusinessDates(toDate, fromDate);
            }
            set
            {
                ToDate = value.ToDate;
                FromDate = value.FromDate;

                ApplyDatesCommand.Execute(null);
            }
        }

        public event BusinessDateChanged OnBusinessDateChanged;

        public void OnLoaded()
        {
            this.ApplyDatesCommand = new DelegateCommand<object>((p) =>
            {
                if (OnBusinessDateChanged != null)
                    OnBusinessDateChanged(this);
            }, (p) => true);

            this.ToDate = DateTime.Today;
            this.FromDate = this.ToDate.AddDays(-1);

            if (OnBusinessDateChanged != null)
                OnBusinessDateChanged(this);
        }

        public void OnUnloaded()
        {
        }
    }
}
