using System;
using System.Collections.Generic;
using System.Text;

namespace KBFMobileApp.Model
{
    public class LoginResponse
    {
        public bool Result { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
