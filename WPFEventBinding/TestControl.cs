using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFEventBinding
{
    class TestControl : FrameworkElement
    {
        #region Routed Events

        public static readonly RoutedEvent MouseWheelUpEvent =
            EventManager.RegisterRoutedEvent(
                nameof(MouseWheelUp), RoutingStrategy.Bubble, typeof(MouseWheelUpDownEventHandler), typeof(TestControl)
            );

        public static readonly RoutedEvent MouseWheelDownEvent =
            EventManager.RegisterRoutedEvent(
                nameof(MouseWheelDown), RoutingStrategy.Bubble, typeof(MouseWheelUpDownEventHandler), typeof(TestControl)
            );

        #endregion

        #region Events

        public event MouseWheelUpDownEventHandler MouseWheelUp
        {
            add => AddHandler(MouseWheelUpEvent, value);
            remove => RemoveHandler(MouseWheelUpEvent, value);
        }

        public event MouseWheelUpDownEventHandler MouseWheelDown
        {
            add => AddHandler(MouseWheelDownEvent, value);
            remove => RemoveHandler(MouseWheelDownEvent, value);
        }

        #endregion

        private static readonly Pen _borderPen = new Pen(Brushes.Navy, 2.0);

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                RaiseEvent(new MouseWheelUpDownEventArgs(MouseWheelUpEvent, e.Delta));
            }
            else
            {
                RaiseEvent(new MouseWheelUpDownEventArgs(MouseWheelDownEvent, e.Delta));
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.AliceBlue, _borderPen, new Rect(new Point(0, 0), RenderSize));

            FormattedText msg = new FormattedText(
                "Try rolling the mouse wheel here!",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                18.0,
                Brushes.Black
            )
            {
                MaxTextWidth = RenderSize.Width,
                MaxTextHeight = RenderSize.Height
            };

            dc.DrawText(msg, new Point(RenderSize.Width / 2.0 - msg.Width / 2.0, RenderSize.Height / 2.0 - msg.Height / 2.0));

            base.OnRender(dc);
        }
    }

    delegate void MouseWheelUpDownEventHandler(object sender, MouseWheelUpDownEventArgs e);

    class MouseWheelUpDownEventArgs : RoutedEventArgs
    {
        public double Delta { get; }

        public MouseWheelUpDownEventArgs(RoutedEvent id, double delta)
        {
            RoutedEvent = id;
            Delta = delta;
        }
    }
}
