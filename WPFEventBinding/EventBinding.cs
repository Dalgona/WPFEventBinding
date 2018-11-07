using System.Windows;
using System.Windows.Input;

namespace WPFEventBinding
{
    public class EventBindingCollection : FreezableCollection<EventBinding> { }

    public class EventBinding : Freezable
    {
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register(nameof(EventName), typeof(string), typeof(EventBinding));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(EventBinding));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(EventBinding));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new EventBinding();
    }
}
