using Microsoft.Extensions.FileProviders;
using MotorX.Api.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.InstallServicesInAssembly();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    ServeUnknownFileTypes = true,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
       // ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    },

    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
