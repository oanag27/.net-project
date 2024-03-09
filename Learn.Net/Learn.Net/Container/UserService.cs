using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Repository.Models;
using Learn.Net.Services;

namespace Learn.Net.Container
{
    public class UserService : IUserService
    {
        private readonly Repository.LearnContext _context;
        //create constructor
        public UserService(Repository.LearnContext learnContext)
        {
            _context = learnContext;
        }
        public Task<APIResponse> ConfirmRegistration(int userId, string username, string outputText)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse> UserRegistration(UserRegister userRegister)
        {
            APIResponse response = new APIResponse();
            int userId = 0;
            try
            {
                if(userRegister!=null)
                {
                    var _tempuser = new TblTempuser()
                    {
                        Code = userRegister.Username,
                        Name = userRegister.Name,
                        Email = userRegister.Email,
                        Phone = userRegister.Phone,
                        Password = userRegister.Password
                    };
                    await _context.TblTempusers.AddAsync(_tempuser); //add user
                    await _context.SaveChangesAsync(); //save changes
                    userId = _tempuser.Id;
                    response.Result = "pass";
                    response.ErrorMessage = userId.ToString();
                }
            }
            catch (Exception ex)
            {
                response = new APIResponse();
            }
            return response;
        }
    }
}
