using Learn.Net.Helper;
using Learn.Net.Modal;

namespace Learn.Net.Services
{
    public interface IUserService
    {
        Task<APIResponse> UserRegistration(UserRegister userRegister);

        Task<APIResponse> ConfirmRegistration(int userId, string username, string outputText);
    }
}
