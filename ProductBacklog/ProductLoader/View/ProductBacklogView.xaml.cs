using ProductBacklog.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductBacklog
{
    /// <summary>
    /// Interaction logic for ProductBacklogView.xaml
    /// </summary>
    public partial class ProductBacklogView : UserControl
    {
        public ProductBacklogView()
        {
            InitializeComponent();
        }

        private void ProductBacklogMouseEnter(object sender, MouseEventArgs args)
        {
            var generalTransformation = this.TransformToVisual(navigationGrid);
            var pt = generalTransformation.Transform(new Point());

            pt.X += navigationTransform.X;
            pt.X -= navigationColumn.ActualWidth;
            pt.Y = 0;

            navigationTransform.AnimateTo(pt);
        }

        private void OnNavigationMouseEnter(object sender, MouseEventArgs args)
        {
            navigationTransform.AnimateTo(new Point());
        }
    }
}
