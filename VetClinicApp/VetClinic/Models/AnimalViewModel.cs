// -----------------------------------------------------------------------------------------------
//  AnimalViewModel.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AnimalViewModel
    {
        [Required(ErrorMessage ="You must enter a name for the animal.")]
        [StringLength(50, ErrorMessage = "Name must be between 2 and 50 characters long.", MinimumLength=2)]
        public string Name { get; set; } = "";

        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2000/1/1", "2021/12/31")]
        public DateTime Birthdate { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="You must enter what type of animal.")]
        public string Race { get; set; } = "";
        [Required(ErrorMessage = "You must set a gender for the animal.")]
        public string Gender { get; set; } = "";

        [Display(AutoGenerateField = false)]
        public string Id { get; set; } = "";

        public string OwnerId { get; set; } = "";
    }
}
