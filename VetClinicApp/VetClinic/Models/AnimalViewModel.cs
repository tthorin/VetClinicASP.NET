// -----------------------------------------------------------------------------------------------
//  AnimalViewModel.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Models
{
    using MongoDbAccess.Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AnimalViewModel : IAnimal
    {
        [Required(ErrorMessage ="You must enter a name for the animal.")]
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
