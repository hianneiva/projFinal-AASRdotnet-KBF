using KBFMobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBFMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Preferences.Get(Constants.PREFERENCES_JWT_KEY, string.Empty)))
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
                this.Navigation.PopToRootAsync();
            }
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e) => await this.Navigation.PushAsync(new LoginPage());

        private async void SignUpBtn_Clicked(object sender, EventArgs e) => await this.Navigation.PushAsync(new SignUpPage());
    }
}