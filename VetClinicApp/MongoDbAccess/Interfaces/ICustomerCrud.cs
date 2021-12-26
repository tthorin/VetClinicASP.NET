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
        Task<bool> DeleteCustomerById(string id);
        Task<List<Models.Customer>> GetAllCustomers();
        Task<Models.Customer> GetCustomerById(string id);
        Task<List<Models.Customer>> GetCustomersEitherNameBeginsWith(string beginsWith);
        Task<bool> UpdateCustomer(Models.Customer owner);
    }
}