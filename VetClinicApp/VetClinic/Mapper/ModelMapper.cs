// -----------------------------------------------------------------------------------------------
//  ModelMapper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Mapper
{
    using MongoDbAccess.Models;
    using VetClinic.Models;
    using static MongoDbAccess.Factory;

    internal static class ModelMapper
    {
        internal static Customer ToCustomer(CustomerViewModel vPerson)
        {
            var output = GetCustomer();
            if (vPerson != null)
            {
                if (!string.IsNullOrWhiteSpace(vPerson.Id)) output.Id = vPerson.Id;
                if (!string.IsNullOrWhiteSpace(vPerson.FirstName)) output.FirstName = vPerson.FirstName;
                if (!string.IsNullOrWhiteSpace(vPerson.LastName)) output.LastName = vPerson.LastName;
                if (!string.IsNullOrWhiteSpace(vPerson.PhoneNumber)) output.PhoneNumber = vPerson.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(vPerson.Email)) output.Email = vPerson.Email;
                if (vPerson.Pets?.Count > 0) output.Pets = vPerson.Pets;
            }
            return output;
        }
        internal static CustomerViewModel ToCustomerViewModel(Customer owner)
        {
            var output = new CustomerViewModel();
            if (owner != null)
            {
                if (!string.IsNullOrWhiteSpace(owner.Id)) output.Id = owner.Id;
                if (!string.IsNullOrWhiteSpace(owner.FirstName)) output.FirstName = owner.FirstName;
                if (!string.IsNullOrWhiteSpace(owner.LastName)) output.LastName = owner.LastName;
                if (!string.IsNullOrWhiteSpace(owner.PhoneNumber)) output.PhoneNumber = owner.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(owner.Email))
                {
                    output.Email = owner.Email;
                    output.ConfirmEmail = owner.Email;
                }
                if (owner.Pets?.Count > 0) output.Pets = owner.Pets;
            }
            return output;
        }
        internal static Animal ToAnimal(AnimalViewModel animalView)
        {
            var output = new Animal();
            if (animalView != null)
            {
                if (!string.IsNullOrWhiteSpace(animalView.Id)) output.Id = animalView.Id;
                if (!string.IsNullOrWhiteSpace(animalView.Name)) output.Name = animalView.Name;
                if (!string.IsNullOrWhiteSpace(animalView.Race)) output.Race = animalView.Race;
                if (!string.IsNullOrWhiteSpace(animalView.Gender)) output.Gender = animalView.Gender;
                if (!string.IsNullOrWhiteSpace(animalView.OwnerId)) output.OwnerId = animalView.OwnerId;
                output.Birthdate = animalView.Birthdate;
            }
            return output;
        }
        internal static AnimalViewModel ToAnimalViewModel(Animal animal)
        {
            var output = new AnimalViewModel();
            if (animal != null)
            {
                if (!string.IsNullOrWhiteSpace(animal.Id)) output.Id = animal.Id;
                if (!string.IsNullOrWhiteSpace(animal.Name)) output.Name = animal.Name;
                if (!string.IsNullOrWhiteSpace(animal.Race)) output.Race = animal.Race;
                if (!string.IsNullOrWhiteSpace(animal.Gender)) output.Gender = animal.Gender;
                if (!string.IsNullOrWhiteSpace(animal.OwnerId)) output.OwnerId = animal.OwnerId;
                output.Birthdate = animal.Birthdate;
            }
            return output;
        }
    }
}
