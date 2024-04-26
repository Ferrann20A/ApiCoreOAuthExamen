using ApiCoreOAuthExamen.Helpers;
using ApiCoreOAuthExamen.Models;
using ApiCoreOAuthExamen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCoreOAuthExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private RepositoryCubos repo;
        private HelperOAuth helper;

        public OAuthController(RepositoryCubos repo, HelperOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuario user = await this.repo.LogInUsuarioAsync(model.Email, model.Password);
            if(user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials
                    (this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonUser = JsonConvert.SerializeObject(user);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonUser)
                };
                JwtSecurityToken token = new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
    }
}
