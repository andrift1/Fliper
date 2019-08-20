using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fliper.Models;

namespace fliper.ViewModels
{
    public class ProfileViewModel
    {
        public string NickName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public IEnumerable<Photos> Zdjecia { get; set; }
        public bool IsYourProfile { get; set; }
        public bool IsAlreadyLiked { get; set; }

        public IEnumerable<zainteresowania> zainteresowania { get; set; }

        public ProfileInfo profileInfo { get; set; }

    }
}