using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Learn.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly LearnContext _context; 
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshHandler _refreshHandler;
        //constructor
        public AuthorizeController(LearnContext learnContext, IOptions<JwtSettings> jwtSettings,IRefreshHandler refreshHandler)
        {
            _context = learnContext;
            _jwtSettings = jwtSettings.Value;
            _refreshHandler = refreshHandler;
        }
        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] UserCredentials userCredentials)
        {
            var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Name == userCredentials.Username && x.Password == userCredentials.Password);
            if (user != null)
            {
                //generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(300),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new TokenResponse() 
                { Token=tokenString,RefreshToken=await _refreshHandler.GenerateToken(userCredentials.Username)}
                );
            }else
            {              
                return Unauthorized();     
            }
        }
        [HttpPost("GenerateRefreshToken")]
        public async Task<IActionResult> GenerateRefreshToken([FromBody] TokenResponse tokenResponse)
        {
            var refreshToken = await _context.TblRefreshtokens.FirstOrDefaultAsync(x => x.Refreshtoken == tokenResponse.RefreshToken);
            if (refreshToken != null)
            {
                //generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);
                SecurityToken securityToken; //used to extract principal info
                var principal = tokenHandler.ValidateToken(tokenResponse.Token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false, 
                }, out securityToken);
                var token = securityToken as JwtSecurityToken;
                //validate token
                if(token!=null && token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    string username = principal.Identity?.Name;//the value can be null
                    var _existingdata = _context.TblRefreshtokens.FirstOrDefaultAsync(x => x.Userid == username&&
                    x.Refreshtoken==tokenResponse.RefreshToken);
                    if(_existingdata!=null)
                    {
                        //generate token
                        var tkn = new JwtSecurityToken(
                                 claims:principal.Claims.ToArray(),
                                 expires:DateTime.UtcNow.AddSeconds(30),
                                 signingCredentials:new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256)
                            );
                        var tokenString = tokenHandler.WriteToken(tkn);
                        return Ok(new TokenResponse()
                        { Token = tokenString, RefreshToken = await _refreshHandler.GenerateToken(username) }
                );
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
