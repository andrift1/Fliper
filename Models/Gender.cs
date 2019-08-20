using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fliper.Models
{
    public enum Gender
    {

        [Display(Name= "Mężczyzna")]
        M,
        [Display(Name = "Kobieta")]
        K
    }
}