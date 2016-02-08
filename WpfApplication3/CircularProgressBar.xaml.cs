using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar
    {
        private readonly DispatcherTimer _animationTimer;

        public static readonly DependencyProperty ChangeCursorProperty =
            DependencyProperty.Register("ChangeCursor", typeof(bool), typeof(CircularProgressBar), new PropertyMetadata(false));

        public static readonly DependencyProperty HeightAndWidthProperty =
            DependencyProperty.Register("HeightAndWidth", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(120));
        public static readonly DependencyProperty BallSizeProperty =
            DependencyProperty.Register("BallSize", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(20));

        public bool ChangeCursor { get { return (bool)GetValue(ChangeCursorProperty); } set { SetValue(ChangeCursorProperty, value); } }
        public int HeightAndWidth { get { return (int)GetValue(HeightAndWidthProperty); } set { SetValue(HeightAndWidthProperty, value); } }
        public int BallSize { get { return (int)GetValue(BallSizeProperty); } set { SetValue(BallSizeProperty, value); } }

        public CircularProgressBar()
        {
            InitializeComponent();

            _animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher) { Interval = new TimeSpan(0, 0, 0, 0, 85) };
        }

        private void Start()
        {
            if (ChangeCursor)
                Mouse.OverrideCursor = Cursors.Wait;

            _animationTimer.Tick += HandleAnimationTick;
            _animationTimer.Start();
        }

        private void Stop()
        {
            _animationTimer.Stop();

            if (ChangeCursor)
                Mouse.OverrideCursor = Cursors.Arrow;

            _animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
        }

        private void SetPosition(Ellipse ellipse, double offset, double posOffSet, double step)
        {
            var rotation = (double)(2.5M * BallSize);

            ellipse.SetValue(Canvas.LeftProperty, rotation
                + Math.Sin(offset + posOffSet * step) * rotation);

            ellipse.SetValue(Canvas.TopProperty, rotation
                + Math.Cos(offset + posOffSet * step) * rotation);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }
    }
}
