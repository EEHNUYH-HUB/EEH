using EEH.WEB;
using EEH.WEB.Auth;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using EEH;
using EEH.WEB.File;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ZENC Settings
ExServiceSetting zencServiceSetting = new ExServiceSetting(builder.Services);

zencServiceSetting.Init();
string allowOrigins = zencServiceSetting.AddCors("http://localhost:8080", "http://localhost:8081", "http://jmfc.eehnuyh.com");
zencServiceSetting.InitAuthenticator(new TokenAuthenticator());
var configuration = builder.Configuration;
var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

zencServiceSetting.InitSmartSql(contentRoot.ExCombine("smartPostgreMapConfig.xml"));
zencServiceSetting.InitNLogConfig(contentRoot.ExCombine("nlog.config"));

string filePath = Path.Combine(contentRoot, "imagefolder");

zencServiceSetting.InitFileHandler(new FileHandler(filePath));
#endregion
builder.Services.AddDirectoryBrowser();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });


});


var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


var provider = new FileExtensionContentTypeProvider();
// Add new mappings

provider.Mappings[".html"] = "text/html";
provider.Mappings[".image"] = "image/png";
provider.Mappings[".png"] = "image/png";
provider.Mappings[".jpeg"] = "image/jpeg";
provider.Mappings[".jpg"] = "image/jpeg";
// Replace an existing mapping
provider.Mappings[".msg"] = "application/x-msdownload";



var fileProvider = new PhysicalFileProvider(filePath);

var requestPath = "/image";



app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = fileProvider,
    RequestPath = requestPath,
    ContentTypeProvider = provider
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath
});

app.UseFileServer(new FileServerOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath,
    EnableDirectoryBrowsing = true,

});
app.UseCors(allowOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
