using Learn.Net.Repository;
using Learn.Net.Repository.Models;
using Learn.Net.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Learn.Net.Container
{
    public class RefreshHandler: IRefreshHandler
    {
        private readonly LearnContext context;
        public RefreshHandler(LearnContext context)
        {
            this.context = context;
        }
        //we have to store it in the database
        public async Task<string> GenerateToken(string username)
        {
            var randNr = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randNr);
                string refreshToken = Convert.ToBase64String(randNr); //convert to string
                var existingToken = await context.TblRefreshtokens.FirstOrDefaultAsync(i=>i.Userid==username);
                if(existingToken!=null)
                {
                    existingToken.Refreshtoken = refreshToken;
                    
                }
                else
                {
                    await context.TblRefreshtokens.AddAsync(new TblRefreshtoken 
                    { 
                        Userid=username,
                        Tokenid = new Random().Next().ToString(),
                        Refreshtoken=refreshToken
                    });
                }
                await context.SaveChangesAsync();  //save changes
                return refreshToken;
            }
        }
    }
}
