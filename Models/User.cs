using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? mobile { get; set; }
        [Required]
        public int userLevel { get; set; }

        public string? adminpassword { get; set; }

        public string? superadminpassword { get; set; }
    }
}
