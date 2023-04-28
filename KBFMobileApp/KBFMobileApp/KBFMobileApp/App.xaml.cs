using KBFMobileApp.Utils;
using KBFMobileApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBFMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Preferences.Get(Constants.PREFERENCES_JWT_KEY, string.Empty)))
            {
                MainPage = new NavigationPage(new HomePage());
            }

            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
