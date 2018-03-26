using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using AuthApp.ViewModels;
using Autofac;


namespace AuthApp.Pages
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
    {
        #region Fields
        TViewModel _viewModel;
        #endregion

        #region Constructors
        protected BaseContentPage()
        {
            BindingContext = ViewModel;

            // Set default background color
            BackgroundColor = (Color)Application.Current.Resources["lightGrayColor"];

            // Helps dealing with the 'notch' on iPhone X
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
        }
        #endregion

        #region Properties
        protected TViewModel ViewModel => _viewModel ?? (_viewModel = App.Container.Resolve<TViewModel>());
        #endregion

        #region Methods
        protected virtual void SubscribeEventHandlers() { }
        protected virtual void UnsubscribeEventHandlers() { }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEventHandlers();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeEventHandlers();
        }
        #endregion
    }
}

