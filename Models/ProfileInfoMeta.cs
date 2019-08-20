using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fliper.Models
{
    [MetadataType(typeof(ProfileInfoAddnotation))]
    public partial class ProfileInfo
    {

    }

    public class ProfileInfoAddnotation
    {
        
        [Display(Name = "Co lubisz")]
        [StringLength(1000)]
        public string colubie { get; set; }
        [Display(Name = "Twój Opis")]
        [StringLength(1000)]
        public string opis { get; set; }
        [StringLength(35)]
        [Display(Name = "Twój psedudonim")]
        public string pseudomin { get; set; }
        [Display(Name = "Twój numer telefonu")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string phonenumber { get; set; }
        [Display(Name = "Twója waga")]
        
        [Range(0, Int16.MaxValue)]
        public int waga { get; set; }
        
        [Display(Name = "Twój wzrost")]
        [Range(0, Int16.MaxValue)]
        public int wzrost { get; set; }
        [StringLength(20)]
        [Display(Name = "Twój ulubiony kolor")]
        public string favcolor { get; set; }

        [Display(Name = "Twój samochód")]
        [StringLength(20)]
        public string samochod { get; set; }
    }
}