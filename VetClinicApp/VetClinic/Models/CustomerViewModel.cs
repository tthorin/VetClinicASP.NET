// -----------------------------------------------------------------------------------------------
//  PetOwnerViewModel.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Models
{
    using MongoDbAccess.Models;
    using System.ComponentModel.DataAnnotations;

    public class CustomerViewModel
    {
        [Display(AutoGenerateField = false)]
        public string Id { get; set; } = "";
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You need to enter a first name")]
        public string FirstName { get; set; } = "";

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You need to enter a last name")]
        public string LastName { get; set; } = "";

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = "";

        [Display(Name = "Email adress")]
        [Required(ErrorMessage ="You must enter an email adress")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";

        [Display(Name = "Confirm Email adress")]
        [Compare("Email", ErrorMessage = "The Email Adress and Confirm Email must match")]
        public string ConfirmEmail { get; set; } = "";
        public List<Animal> Pets { get; set; } = new();
    }
}
