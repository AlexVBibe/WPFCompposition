using System.Windows.Controls;
using System.Windows.Input;

namespace ProductBacklog.Learning.View
{
    /// <summary>
    /// Interaction logic for SelectBusinessDateView.xaml
    /// </summary>
    public partial class SelectBusinessDateView : UserControl
    {
        public SelectBusinessDateView()
        {
            InitializeComponent();
            calFrom.SelectedDatesChanged += OnSelectedDatesChanged;
            calTo.SelectedDatesChanged += OnSelectedDatesChanged;
        }

        private void OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
