using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProductBacklog.Composition
{
    public static class TranslateTransformExtensions
    {
        public static void AnimateTo(this GeneralTransform transform, Point pt)
        {
            transform.StartAnimation(TranslateTransform.XProperty, pt.X);
            transform.StartAnimation(TranslateTransform.YProperty, pt.Y);
        }

        public static void StartAnimation(this GeneralTransform transform, DependencyProperty dp, double dValue)
        {
            var animation = new DoubleAnimation
            {
                To = dValue,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                EasingFunction = new QuadraticEase()
            };
            transform.BeginAnimation(dp, animation);
        }
    }
}
