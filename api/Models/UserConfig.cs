using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signup_api.Models
{
    public class UserConfig
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
