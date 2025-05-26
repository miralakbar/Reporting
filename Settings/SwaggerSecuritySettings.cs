using System.Collections.Generic;

namespace Application.Settings
{
    public class SwaggerSecuritySettings
    {
        public List<SwaggerSecurity> SecuritySettings { get; set; }
    }

    public class SwaggerSecurity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
