using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signup_example.Models
{
    public class Password
    {
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
