using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaMovie.Viewsmodel
{
    public class LoginViewModel
    {
        [StringLength(256), Required]
        //[RegularExpression(@"^\w +@[a-zA - Z_]+?\.[a-zA - Z]{ 2,3}$")]
        //joe@aol.com | ssmith@aspalliance.com | a@b.cc
        public string Email { get; set; }
      
        [Required]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}
