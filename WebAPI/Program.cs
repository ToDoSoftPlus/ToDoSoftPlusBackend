using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);

#region Local appsettings
//Add reading a local appsettings file for development environment
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.Local.json",
        optional: true,
        reloadOnChange: true);
#endregion

#region Dependency Injection Projects
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

#endregion

#region NSwag
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ToDo API";
    document.Version = "v1";

    document.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Введіть токен у форматі: Bearer {ваш_токен}"
    });

    document.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
#endregion

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
