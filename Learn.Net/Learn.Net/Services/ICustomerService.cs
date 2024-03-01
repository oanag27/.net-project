using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository.Models;

namespace Learn.Net.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerModal>> GetAll();
        Task<CustomerModal> GetByCode(string code);
        Task<APIResponse> Add(CustomerModal customerModal);
        Task<APIResponse> Update(CustomerModal customerModal,string code);
        Task<APIResponse> Delete(string code);
    }
}
