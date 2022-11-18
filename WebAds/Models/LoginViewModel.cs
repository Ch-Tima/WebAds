using Microsoft.AspNetCore.Authentication;

namespace WebAds.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }

        public IList<AuthenticationScheme>? ExternalAuthentication { get; set; }
    }
}