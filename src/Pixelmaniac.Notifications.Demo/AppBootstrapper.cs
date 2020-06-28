using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Pixelmaniac.Notifications.Demo.ViewModels;

namespace Pixelmaniac.Notifications.Demo
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<NotificationManager>();

            _container.PerRequest<MainViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // IMPORTANT
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            DisplayRootViewFor<MainViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
