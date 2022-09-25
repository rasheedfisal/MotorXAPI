using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MotorX.Api.Services;

namespace MotorX.Api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor?.HttpContext?.Request;
                var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
                return new UriService(uri);
            });
            builder.Services.AddControllers();
            //builder.Services.AddDataProtection()
            //    .PersistKeysToFileSystem(new DirectoryInfo(@"h:\root\home\trialonev10-001\www\trialv2\data\keys\"));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "MotorX API", Version = "v1" });
                option.OperationFilter<SwaggerFileOperationFilter>();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                 Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });


            builder.Services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;

                opt.AssumeDefaultVersionWhenUnspecified = true;

                opt.DefaultApiVersion = ApiVersion.Default;
                //opt.DefaultApiVersion = new ApiVersion(1,0);
            });

            builder.Services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            builder.Services.AddScoped<IImageProcessor, ImageProcessor>();
        }
    }
}
