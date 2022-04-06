using System;

namespace EXhibition.Models
{
    public class LoginToken
    {
        public string token { get; set; }

        static public LoginToken getToken()
        {
            return new LoginToken() { token = Guid.NewGuid().ToString().Substring(6) };
        }
    }
}