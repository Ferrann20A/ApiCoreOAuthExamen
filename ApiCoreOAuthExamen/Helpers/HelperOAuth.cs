using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCoreOAuthExamen.Helpers
{
    public class HelperOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        private SecretClient secretClient;

        public HelperOAuth(SecretClient secretClient)
        {
            this.secretClient = secretClient;
            KeyVaultSecret secretIssuer = this.secretClient.GetSecret("Issuer");
            KeyVaultSecret secretAudience = this.secretClient.GetSecret("Audience");
            KeyVaultSecret secretSecretKey = this.secretClient.GetSecret("SecretKey");
            this.Issuer = secretIssuer.Value;
            this.Audience = secretAudience.Value;
            this.SecretKey = secretSecretKey.Value;
        }

        public SymmetricSecurityKey GetKeyToken()
        {
            //CONVERTIMOS EL SECRET KEY A BYTES[]
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            //DEVOLVEMOS LA KEY GENERADA MEDIANTE LOS bytes[]
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                //INDICAMOS QUE DESEAMOS VALIDAR DE NUESTRO TOKEN, ISSUER
                //AUDIENCE, TIME
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = this.GetKeyToken()
                };
            });
            return options;
        }

        public Action<AuthenticationOptions> GetAuthenticationSchema()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}
