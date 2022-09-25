using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MotorX.Auth.Configuration;
using System.Text;

namespace MotorX.Api.Installers
{
    public class JwtInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

            var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

            var JwtTokenParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, //TODO: update
                ValidateAudience = false, //TODO: update
                RequireExpirationTime = false, //TODO: update
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero 
            };

            builder.Services.AddSingleton(JwtTokenParameters);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = JwtTokenParameters;
                //jwt.Events.OnMessageReceived = context =>
                //{
                //    if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                //    {
                //        context.Token = context.Request.Cookies["X-Access-Token"];
                //    }

                //    return Task.CompletedTask;
                //};
            });
        }
    }
}
