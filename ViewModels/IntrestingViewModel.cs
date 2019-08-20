using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fliper.Models;

namespace fliper.ViewModels
{
    public class IntrestingViewModel
    {
        public string Nazwa { get; set; }

        public IEnumerable<zainteresowania> zainteresowania { get; set; }
    }
}