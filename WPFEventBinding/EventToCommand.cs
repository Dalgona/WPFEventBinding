using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace WPFEventBinding
{
    public static class EventToCommand
    {
        private static readonly EventBindingCollection _empty = new EventBindingCollection();

        private static readonly Dictionary<UIElement, Dictionary<string, EventHandlerHolder>> _handlerTable =
            new Dictionary<UIElement, Dictionary<string, EventHandlerHolder>>();

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
            if (e.OldValue is EventBindingCollection oldCollection)
            {
                foreach (EventBinding eventBinding in oldCollection)
                {
                    UnHookEvent(d as UIElement, eventBinding);
                }
            }

            if (e.NewValue is EventBindingCollection newCollection)
            {
                foreach (EventBinding eventBinding in newCollection)
                {
                    HookEvent(d as UIElement, eventBinding);
                }
            }
        }

        private static void UnHookEvent(UIElement that, EventBinding eventBinding)
        {
            string eventName = eventBinding.EventName;

            EventHandlerHolder handlerHolder = _handlerTable[that][eventName];
            EventInfo eventInfo = that.GetType().GetEvent(eventName);
            MethodInfo methodInfo = handlerHolder.GetType().GetMethod(nameof(EventHandlerHolder.HandleEvent), BindingFlags.Instance | BindingFlags.Public);
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, handlerHolder, methodInfo);

            eventInfo.RemoveEventHandler(that, handler);
            _handlerTable[that].Remove(eventName);
        }

        private static void HookEvent(UIElement that, EventBinding eventBinding)
        {
            if (!_handlerTable.TryGetValue(that, out var subTable))
            {
                subTable = new Dictionary<string, EventHandlerHolder>();

                _handlerTable.Add(that, subTable);
            }

            string eventName = eventBinding.EventName;

            if (subTable.ContainsKey(eventName))
            {
                throw new InvalidOperationException($"An event binding for the '{eventName}' event already exists.");
            }

            EventHandlerHolder handlerHolder = new EventHandlerHolder(eventBinding);
            subTable[eventName] = handlerHolder;

            EventInfo eventInfo = that.GetType().GetEvent(eventName) ?? throw new Exception($"The target object does not have an event named '{eventName}'.");
            MethodInfo methodInfo = handlerHolder.GetType().GetMethod(nameof(EventHandlerHolder.HandleEvent), BindingFlags.Instance | BindingFlags.Public);
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, handlerHolder, methodInfo);

            eventInfo.AddEventHandler(that, handler);
        }

        private class EventHandlerHolder
        {
            private readonly ICommand _cmd;
            private readonly object _cmdParam;

            public EventHandlerHolder(EventBinding eventBinding)
            {
                _cmd = eventBinding.Command;
                _cmdParam = eventBinding.CommandParameter;
            }

            public void HandleEvent(object sender, EventArgs e)
            {
                if (_cmd?.CanExecute(_cmdParam) ?? false)
                {
                    _cmd?.Execute(_cmdParam);
                }
            }
        }
    }
}
