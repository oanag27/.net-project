using Learn.Net.Helper;
using Learn.Net.Modal;

namespace Learn.Net.Services
{
    public interface IUserService
    {
        Task<APIResponse> UserRegistration(UserRegister userRegister);
        Task<APIResponse> ResetPassword(string username, string oldPassword, string newPassword);
        Task<APIResponse> ConfirmRegistration(int userId, string username, string outputText);
        Task<APIResponse> ForgetPassword(string username);
        Task<APIResponse> UpdatePassword(string username,string password, string OtpText);

        Task<APIResponse> UpdateStatus(string username, bool userStatus);

        Task<APIResponse> UpdateRole(string username, string userRole);
    }
}
