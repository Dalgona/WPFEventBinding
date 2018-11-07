using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFEventBinding
{
    public static class EventToCommand
    {
        private static readonly EventBindingCollection _empty = new EventBindingCollection();

        public static readonly DependencyProperty EventBindingsProperty =
            DependencyProperty.RegisterAttached(
                "EventBindings", typeof(EventBindingCollection), typeof(EventToCommand),
                new PropertyMetadata(_empty, EventBindingsPropertyChangedCallback)
            );

        public static EventBindingCollection GetEventBindings(DependencyObject target)
        {
            return (EventBindingCollection)target.GetValue(EventBindingsProperty);
        }

        public static void SetEventBindings(DependencyObject target, EventBindingCollection value)
        {
            target.SetValue(EventBindingsProperty, value);
        }

        private static void EventBindingsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
