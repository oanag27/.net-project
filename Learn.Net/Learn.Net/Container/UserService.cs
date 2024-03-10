using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Repository.Models;
using Learn.Net.Services;
using Microsoft.EntityFrameworkCore;

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
        public async Task<APIResponse> ConfirmRegistration(int userId, string username, string outputText)
        {
            APIResponse response = new APIResponse();
            bool otpValid = await ValidateOTP(username, outputText);
            if(!otpValid)
            {
                response.Result = "fail";
                response.ErrorMessage = "Invalid OTP";
            }
            else
            {
                var _tempuser = await _context.TblTempusers.FirstOrDefaultAsync(i=>i.Id == userId);
                var user = new TblUser()
                {
                    Username = username,
                    Name = _tempuser.Name,
                    Email = _tempuser.Email,
                    Phone = _tempuser.Phone,
                    Password = _tempuser.Password,
                    Isactive = true,
                    Role = "user"
                };
                await _context.TblUsers.AddAsync(user); //add user
                await _context.SaveChangesAsync(); //save changes
                await UpdatePWDManager(username, _tempuser.Password);
                response.Result = "pass";
                response.ErrorMessage = "Registered successfully";
            }
            return response;
        }
        private async Task<bool> ValidateOTP(string username, string outputText)
        {
            bool response = false;
            var data = await _context.TblOtpManagers.FirstOrDefaultAsync(i=>i.Username == username 
            && i.Otptext == outputText && i.Expiration > DateTime.Now); //check if the otp is valid
            if(data!=null)
            {
                response = true;
            }
            return response;
        }

        public async Task<APIResponse> UserRegistration(UserRegister userRegister)
        {
            APIResponse response = new APIResponse();
            int userId = 0;
            bool isValid = true;
            try
            {
                //check for duplicate user
                var user = await _context.TblUsers.Where(i=>i.Username == userRegister.Username).ToListAsync();
                if(user.Count>0)
                {
                    response.Result = "fail";
                    response.ErrorMessage = "Name already exist";
                    isValid = false;
                }
                var userEmail = await _context.TblUsers.Where(i => i.Email == userRegister.Email).ToListAsync();
                if (userEmail.Count > 0)
                {
                    response.Result = "fail";
                    response.ErrorMessage = "Email already exist";
                    isValid = false;
                }
                if (userRegister != null)
                {
                    var _tempuser = new TblTempuser()
                    {
                        Code = userRegister.Username,
                        Name = userRegister.Name,
                        Email = userRegister.Email,
                        Password = userRegister.Password,
                        Phone = userRegister.Phone,
                    };
                    await _context.TblTempusers.AddAsync(_tempuser);
                    await _context.SaveChangesAsync();
                    userId = _tempuser.Id;
                    string OTPText = GenerateRandomNumber();
                    await UpdateOtp(userRegister.Username, OTPText, "register");
                    await SendMail(userRegister.Email, OTPText, userRegister.Name);
                    response.Result = "pass";
                    response.ErrorMessage = userId.ToString();
                }

            }
            catch (Exception ex)
            {
                response.Result = "fail";
            }
            return response;
        }
        private async Task UpdateOutput(string userName, string outputText, string outputType)
        {
            var option = new TblOtpManager()
            {
                Username = userName,
                Otptext = outputText,
                Otptype = outputType,
                Expiration = DateTime.Now.AddMinutes(2),
                Createddate = DateTime.Now
            };
            await _context.TblOtpManagers.AddAsync(option); //add option
            await _context.SaveChangesAsync(); //save changes
        }
        private async Task UpdatePWDManager(string userName, string password)
        {
            var option = new TblPwdManger()
            {
                Username = userName,
                Password = password,
                Modifydate = DateTime.Now
            };
            await _context.TblPwdMangers.AddAsync(option); //add option
            await _context.SaveChangesAsync(); //save changes
        }
        private string GenerateRandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
        private async Task SendMail(string email, string outputText,string name)
        {
        }

        public async Task<APIResponse> ResetPassword(string username, string oldPassword, string newPassword)
        {
            APIResponse aPIResponse = new APIResponse();
            var user = await _context.TblUsers.FirstOrDefaultAsync(i=>i.Username == username && i.Password == oldPassword && i.Isactive==true);
            if(user!=null)
            {
                var pwdHistory = await ValidatePWDHistory(username, newPassword);
                if(pwdHistory)
                {
                    aPIResponse.Result = "fail";
                    aPIResponse.ErrorMessage = "Password already used";
                }
                else { 
                    user.Password = newPassword; //update password
                    await _context.SaveChangesAsync(); //save changes
                    await UpdatePWDManager(username, newPassword);
                    aPIResponse.Result = "pass";
                    aPIResponse.ErrorMessage = "Password updated successfully";
                }
            }
            else
            {
                aPIResponse.Result = "fail";
                aPIResponse.ErrorMessage = "Invalid user";
            }
            return aPIResponse;

        }
        
        private async Task<bool> ValidatePWDHistory(string username,string password)
        {
            bool response = false;
            var data = await _context.TblPwdMangers.Where(i => i.Username == username).OrderByDescending(p=>p.Modifydate).Take(3).ToListAsync();//get the last 3 password
            if(data.Count>0)
            {
                var validate = data.Where(i => i.Password == password);//check if the password is in the last 3
                if(validate.Any())
                {
                    response = true;
                }
            }
            return response;
        }

        public async Task<APIResponse> ForgetPassword(string username)
        {
            APIResponse response = new APIResponse();
            var _user = await _context.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Isactive == true);
            if (_user != null)
            {
                string otptext = GenerateRandomNumber();
                await UpdateOtp(username, otptext, "forgetPassword");
                await SendMail(_user.Email, otptext, _user.Name);
                response.Result = "pass";
                response.ErrorMessage = "OTP sent successfully";

            }
            else
            {
                response.Result = "fail";
                response.ErrorMessage = "Invalid User";
            }
            return response;
        }
        private async Task UpdateOtp(string username, string otptext, string otptype)
        {
            var _opt = new TblOtpManager()
            {
                Username = username,
                Otptext = otptext,
                Expiration = DateTime.Now.AddMinutes(2),
                Createddate = DateTime.Now,
                Otptype = otptype
            };
            await _context.TblOtpManagers.AddAsync(_opt);
            await _context.SaveChangesAsync();
        }

        public async Task<APIResponse> UpdatePassword(string username, string password, string OtpText)
        {
            APIResponse response = new APIResponse();

            bool otpvalidation = await ValidateOTP(username, OtpText);
            if (otpvalidation)
            {
                bool pwdhistory = await ValidatePWDHistory(username, password);
                if (pwdhistory)
                {
                    response.Result = "fail";
                    response.ErrorMessage = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    var _user = await _context.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Isactive == true);
                    if (_user != null)
                    {
                        _user.Password = password;
                        await _context.SaveChangesAsync();
                        await UpdatePWDManager(username, password);
                        response.Result = "pass";
                        response.ErrorMessage = "Password changed succesfully!";
                    }
                }
            }
            else
            {
                response.Result = "fail";
                response.ErrorMessage = "Invalid OTP";
            }
            return response;
        }

        public async Task<APIResponse> UpdateStatus(string username, bool userStatus)
        {
            APIResponse response = new APIResponse();   
            var user = await _context.TblUsers.FirstOrDefaultAsync(i=>i.Username == username);
            if(user!=null)
            {
                user.Isactive = userStatus;
                await _context.SaveChangesAsync(); //save changes
                response.Result = "pass";
                response.ErrorMessage = "Status updated successfully";
            }
            else
            {
                response.Result = "fail";
                response.ErrorMessage = "Invalid user";
            }
            return response;
        }

        public async Task<APIResponse> UpdateRole(string username, string userRole)
        {
            APIResponse response = new APIResponse();
            var user = await _context.TblUsers.FirstOrDefaultAsync(i => i.Username == username);
            if (user != null)
            {
                user.Role = userRole;
                await _context.SaveChangesAsync(); //save changes
                response.Result = "pass";
                response.ErrorMessage = "Role updated successfully";
            }
            else
            {
                response.Result = "fail";
                response.ErrorMessage = "Invalid user";
            }
            return response;
        }
    }
}
