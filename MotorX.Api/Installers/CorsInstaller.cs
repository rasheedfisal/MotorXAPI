namespace MotorX.Api.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:7186",
                                                          "http://localhost:3000",
                                                          "https://motor-x-rasheedfisal.vercel.app",
                                                          "http://207.180.223.113:9875",
                                                          "https://autocars-er.herokuapp.com",
                                                          "http://194.195.87.30:84"
                                                          )
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                                      ;
                                  });
            });
        }
    }
}
