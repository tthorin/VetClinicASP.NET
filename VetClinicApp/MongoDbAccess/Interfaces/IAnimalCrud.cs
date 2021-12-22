// -----------------------------------------------------------------------------------------------
//  IAnimalCrud.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnimalCrud
    {
        Task CreateAnimal(Models.Animal animal);
        Task DeleteAnimal(Models.Animal animal);
        Task<List<Models.Animal>> GetAllAnimal();
        Task<Models.Animal> GetAnimalById(string id);
        Task<List<Models.Animal>> GetAnimalsByNameBeginsWith(string searchString);
        Task UpdateAnimal(Models.Animal animal);
    }
}