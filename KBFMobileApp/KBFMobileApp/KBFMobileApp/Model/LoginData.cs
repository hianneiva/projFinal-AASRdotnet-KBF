using KBFMobileApp.Services;
using System;
using System.Text;

namespace KBFMobileApp.Model
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginData (string username, string password) 
        {
            Username = username;
            Password = $"{EncodeText(password)}.{EncodeText(AppSettingsManager.Settings["Cypher"])}";
        }

        private string EncodeText(string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }
}
