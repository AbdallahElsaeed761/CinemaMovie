using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaMovie.Viewsmodel
{
    public class RegisterViewModel
    {
        [StringLength(256),Required]
        //[RegularExpression(@"^\w +@[a-zA - Z_]+?\.[a-zA - Z]{ 2,3}$")]
        //joe@aol.com | ssmith@aspalliance.com | a@b.cc
        public string Email { get; set; }
        [StringLength(256), Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
