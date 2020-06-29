![Logo](https://user-images.githubusercontent.com/2874236/85953132-f40d1900-b976-11ea-89bf-a07a935225d5.png)
<br/>
<br/>
[![Build status](https://github.com/paulem/pixelmaniac.notifications/workflows/build/badge.svg)](https://github.com/paulem/pixelmaniac.notifications/actions)
[![NuGet version](https://img.shields.io/nuget/v/Pixelmaniac.Notifications)](https://nuget.org/packages/Pixelmaniac.Notifications)
[![NuGet downloads](https://img.shields.io/nuget/dt/Pixelmaniac.Notifications)](https://nuget.org/packages/Pixelmaniac.Notifications)
[![Pavel's Twitter](https://img.shields.io/badge/twitter-%40upavel-55acee.svg)](https://twitter.com/upavel)

## Notifications
Accurate toast notifications for **WPF**, visually similar to Windows 10 notifications.

**[See demo video](https://paulem.com/pxmc/notifications/demo)**
<br/>
<br/>
![Notification screenshot](https://user-images.githubusercontent.com/2874236/85958318-6133a500-b99d-11ea-8c46-ae57b95a6f34.png) ![Notification screenshot](https://user-images.githubusercontent.com/2874236/85958380-d3a48500-b99d-11ea-9d47-ac8cd7128396.png) ![Notification screenshot](https://user-images.githubusercontent.com/2874236/85958379-d2735800-b99d-11ea-857b-06b823d5faf2.png)

## Features
* Default toast notification template, visually similar to Windows 10
  * App identity
  * Attribution text
  * Vector & raster icon support
  * Choose between small `16px` or large icon `48px`
* Unobtrusive, smooth animations
* Easy to customize

## Supported platforms
* `.NET Framework 4.5.2 +`
* `.NET Core 3.1`

## Installation
`Pixelmaniac.Notifications` is available on [NuGet](https://www.nuget.org/packages/Pixelmaniac.Notifications).

Install using NuGet:
```
Install-Package Pixelmaniac.Notifications
```
Install using .NET CLI:
```
dotnet add package Pixelmaniac.Notifications
```

## Usage 
#### Show simple toast notification
```C#
var notificationManager = new NotificationManager();

notificationManager.Notify(
    message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    title: "Simple notification");
```

#### Use NotificationContent to override AppIdentity, set icons and etc.
```C#
var notificationManager = new NotificationManager();

var content = new NotificationContent
{
    Title = "Simple notification",
    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    AppIdentity = "App",
    AttributionText = "Via PXMC",
    VectorIcon = Application.Current.TryFindResource("[...]") as StreamGeometry,
    UseLargeIcon = true
};

notificationManager.Notify(content);
```

#### Show notification inside application window
1. Add namespace:
```XAML
xmlns:pxmc="http://7room.net/xaml/pixelmaniac"
```
2. Add `NotificationArea` within which notifications will be displayed:
```XAML
<pxmc:NotificationArea MaxNotificationsCount="3" Position="BottomRight" />
```
3. Show notification:
```C#
notificationManager.Options.InAppNotificationPlacement = true;

notificationManager.Notify(
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    "Simple notification");
```

#### OnClick & OnClose actions
```C#
notificationManager.Notify(
    content,
    onClick: () => Console.WriteLine("Click"),
    onClose: () => Console.WriteLine("Closed"));
```

#### Caliburn.Micro MVVM support
>[Here](src/Pixelmaniac.Notifications.Demo) is a fully working example of an app with Caliburn.Micro support.

1. Modify App.xaml:
```XAML
xmlns:pxmc="http://7room.net/xaml/pixelmaniac"

<Application.Resources>
    [...]
    <Style TargetType="{x:Type pxmc:Notification}">
        <Style.Resources>
            <DataTemplate DataType="{x:Type micro:PropertyChangedBase}">
                <ContentControl cal:View.Model="{Binding}"/>
            </DataTemplate>
        </Style.Resources>
    </Style>
</Application.Resources>
```
2. Create a ViewModel and use it as a notification content:
```C#
var vm = new NotificationViewModel
{
    Title = "Custom notification",
    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
};

_notificationManager.Notify(vm, expirationTime: TimeSpan.FromSeconds(30));
```
3. You may use `controls:Notification.CloseOnClick` in your view:
```XAML
<DockPanel LastChildFill="False">
    <!--Using CloseOnClick attached property to close notification when button is pressed-->
    <Button x:Name="Ok" Content="Ok" DockPanel.Dock="Right" controls:Notification.CloseOnClick="True"/>
    <Button x:Name="Cancel" Content="Cancel" DockPanel.Dock="Right" Margin="0,0,8,0" controls:Notification.CloseOnClick="True"/>
</DockPanel>
```

## Thanks
https://github.com/Federerer
