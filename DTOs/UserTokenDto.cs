using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserTokenDto
    {
        public string userName { get; set; }
        public string token { get; set; }
        public string roleName { get; set; }
    }
}