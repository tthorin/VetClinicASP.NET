// -----------------------------------------------------------------------------------------------
//  PhoneNumber.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PhoneNumber
    {
        [Display(Name ="Label for phone number")]
        public string Label { get; set; } = "";
        public string Number { get; set; } = "";
    }
}
