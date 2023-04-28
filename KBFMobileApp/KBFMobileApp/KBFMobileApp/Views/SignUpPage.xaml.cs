using KBFMobileApp.Model;
using KBFMobileApp.Services;
using KBFMobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBFMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private readonly string baseUrl;
        private readonly string signUpEndPoint;

        public SignUpPage()
        {
            InitializeComponent();
            baseUrl = AppSettingsManager.Settings["ApiHost"];
            signUpEndPoint = AppSettingsManager.Settings["ApiAuthSignUp"];
        }

        private async void SignUpBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!(PwdInput.Text?.Equals(PwdConfirmInput.Text)).GetValueOrDefault())
                {
                    await DisplayAlert("Erro", "As senhas informadas devem ser iguais", "OK");
                    return;
                }

                SignUpData data = new SignUpData(EmailInput.Text?.Trim(), LoginInput.Text?.Trim(), NameInput.Text?.Trim(), PwdInput.Text?.Trim());

                if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Login) || string.IsNullOrEmpty(data.Nome) || string.IsNullOrEmpty(data.Senha))
                {
                    throw new Exception("Um dos dados não foi informado");
                }

                CancelBtn.IsVisible = false;
                SignUpBtn.IsVisible = false;
                ApiIndicator.IsRunning = true;
                HttpHelper<LoginResponse, SignUpData> httpPoster = new HttpHelper<LoginResponse, SignUpData>(baseUrl);
                LoginResponse response = await httpPoster.Post(signUpEndPoint, data);
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
                    SignUpBtn.IsVisible = true;
                    ApiIndicator.IsRunning = false;
                    await DisplayAlert("Falha", response.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                CancelBtn.IsVisible = true;
                SignUpBtn.IsVisible = true;
                ApiIndicator.IsRunning = false;
                await DisplayAlert("Falha", ex.Message, "OK");
            }
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e) => await this.Navigation.PopToRootAsync();
    }
}