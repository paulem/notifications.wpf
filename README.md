![Logo](https://user-images.githubusercontent.com/2874236/85953132-f40d1900-b976-11ea-89bf-a07a935225d5.png)
<br/>
<br/>
[![Build status](https://github.com/paulem/pixelmaniac.notifications/workflows/build/badge.svg)](https://github.com/paulem/pixelmaniac.notifications/actions)
[![Nuget version](https://img.shields.io/nuget/v/Pixelmaniac.Notifications?label=NuGet)](https://nuget.org/packages/Pixelmaniac.Notifications)
[![Nuget downloads](https://img.shields.io/nuget/dt/Pixelmaniac.Notifications?label=Downloads)](https://nuget.org/packages/Pixelmaniac.Notifications)
[![Twitter Pavel](https://img.shields.io/badge/twitter-%40upavel-55acee.svg?label=Twitter)](https://twitter.com/upavel)

## Notifications
Accurate toast notifications for WPF, visually similar to Windows 10 notifications.

#### Features
* Default toast notification template, visually similar to Windows 10
  * App identity
  * Attribution text
  * Vector & raster icon support
  * Choose between small (16px) or large icon (48px)
* Unobtrusive, smooth animations
* Easy to customize

#### Installation
##### Requirements
`.NET Framework 4.7.2`, `Net Core 3.1`
```
Install-Package Pixelmaniac.Notifications
```
#### Usage
##### Simple
```C#
var notificationManager = new NotificationManager();

notificationManager.Notify(
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    "Simple notification");
```

##### Advanced
```C#
var notificationManager = new NotificationManager();

var content = new NotificationContent
{
    Title = "Simple notification",
    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    AppIdentity = "App",
    AttributionText = "Via PXMC",
    VectorIcon = Application.Current.TryFindResource("Geometry.Icon.16") as StreamGeometry,
    UseLargeIcon = true
};

notificationManager.Notify(content);
```

##### Notification inside application window
- Adding namespace:
```XAML
xmlns:pxmc="http://7room.net/xaml/pixelmaniac"
```
- Adding new NotificationArea:
```XAML
<pxmc:NotificationArea MaxNotificationsCount="3" Position="BottomRight" />
```
- Displaying notification:
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
### Caliburn.Micro MVVM support
- App.xaml:
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
- MainViewModel
```C#
var vm = new NotificationViewModel
{
    Title = "Custom notification",
    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
};

_notificationManager.Notify(vm, expirationTime: TimeSpan.FromSeconds(30));
```
- NotificationView
```XAML
<DockPanel LastChildFill="False">
    <!--Using CloseOnClick attached property to close notification when button is pressed-->
    <Button x:Name="Ok" Content="Ok" DockPanel.Dock="Right" controls:Notification.CloseOnClick="True"/>
    <Button x:Name="Cancel" Content="Cancel" DockPanel.Dock="Right" Margin="0,0,8,0" controls:Notification.CloseOnClick="True"/>
</DockPanel>
```
##### Thanks
https://github.com/Federerer
