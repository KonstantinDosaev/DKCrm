using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.UserAuth
{
    public class LoginRequest
    {
        [Required] public string UserName { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
