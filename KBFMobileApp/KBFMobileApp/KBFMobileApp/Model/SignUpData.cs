using KBFMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace KBFMobileApp.Model
{
    public class SignUpData
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }

        public SignUpData(string email, string login, string nome, string senha)
        {
            string cypher = AppSettingsManager.Settings["Cypher"];

            Email = email;
            Login = login;
            Nome = nome;
            Senha = $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(senha))}.{Convert.ToBase64String(Encoding.UTF8.GetBytes(cypher))}";
        }   
    }
}
