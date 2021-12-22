// -----------------------------------------------------------------------------------------------
//  ICustomerCrud.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerCrud
    {
        Task CreateCustomer(Models.Customer owner);
        Task DeleteCustomerById(string id);
        Task<List<Models.Customer>> GetAllCustomers();
        Task<Models.Customer> GetCustomerById(string id);
        Task<List<Models.Customer>> GetCustomersEitherNameBeginsWith(string beginsWith);
        Task UpdateCustomer(Models.Customer owner);
    }
}