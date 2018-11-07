# WPFEventBinding

> My rough implementation of WPF "Event to Command" behavior

The `EventToCommand.EventBindings` attached property enables you to execute a command when a specific event is raised.

## Why?

* Microsoft Expression Blend 4 SDK comes with this feature, but it's for .NET Framework 4 and 4.5, and it won't work if your project is targeting .NET Framework 4.6 or higher.

* Many WPF libraries such as MVVM Light Toolkit also provides this functionality, but I didn't want to include a whole library just for that feature.

&hellip;Never mind. Honestly, I just wanted to try implementing "Event to Command" on my own and see if it works.

## Sample Program

The sample program consists of `ViewModel`, `TestControl`, and `MainWindow`.

`ViewModel` is a ViewModel class which exposes an integer value and three `ICommand`s for manipulating that value: `IncreaseValueCommand`, `DecreaseValueCommand`, and `ResetValueCommand`.

`TestControl` is a simple `FrameworkElement` with two custom routed events, which are `MouseWheelUp` and `MouseWheelDown`.

Finally, `MainWindow` shows that a `TestControl` has three event bindings. Each event will trigger the execution of a corresponding command.

## Limitations

This implementation lacks some functionalities, such as sending an `WhateverEventArgs` as a command parameter and so on.
