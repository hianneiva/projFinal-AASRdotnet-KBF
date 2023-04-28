using KBFMobileApp.Model;
using KBFMobileApp.Services;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KBFMobileApp.Utils;

namespace KBFMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly string baseUrl;
        private readonly string loginEndPoint;
        
        public LoginPage()
        {
            InitializeComponent();
            baseUrl = AppSettingsManager.Settings["ApiHost"];
            loginEndPoint = AppSettingsManager.Settings["ApiAuthLogin"];
        }

        private async void LogonBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                CancelBtn.IsVisible = false;
                LogonBtn.IsVisible = false;
                ApiIndicator.IsRunning = true;
                LoginData data = new LoginData(LoginInput.Text.Trim(), PwdInput.Text.Trim());
                HttpHelper<LoginResponse, LoginData> httpPoster = new HttpHelper<LoginResponse, LoginData>(baseUrl);
                LoginResponse response = await httpPoster.Post(loginEndPoint, data);
                Debug.WriteLine($"Login result: {response.Result}");

                if (response.Result)
                {
                    Preferences.Set(Constants.PREFERENCES_JWT_KEY, response.Token);
                    Application.Current.MainPage = new NavigationPage(new HomePage());
                    await this.Navigation.PopToRootAsync();
                }
                else
                {
                    CancelBtn.IsVisible = true;
                    LogonBtn.IsVisible = true;
                    ApiIndicator.IsRunning = false;
                    await DisplayAlert("Falha", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                CancelBtn.IsVisible = true;
                LogonBtn.IsVisible = true;
                ApiIndicator.IsRunning = false;
                await DisplayAlert("Falha", ex.Message, "OK");
            }
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e) => await this.Navigation.PopToRootAsync();
    }
}