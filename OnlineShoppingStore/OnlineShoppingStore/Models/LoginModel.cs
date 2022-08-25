using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "You must enter your email!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "You must enter your password!")]
        public string Password { get; set; }
    }
}