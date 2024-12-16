using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using QuickServe.Controllers;
using QuickServe.Middleware;
using QuickServe.Services;
using QuickServe.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IJsonService, JsonService>();
builder.Services.AddSingleton<IAppService, AppService>();
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IDataService, DataService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quick Serve",
        Version = "V1",
        Description = """
        <b>Quick Serve API</b><br/>
        Quick Serve API is a personal REST-API that handles online data-storage.<br/>
        It is meant for hobby projects on the frontend, when setting up an entire</br>
        API for storing data is overkill.<br/><br/>
        It can: <ul>
            <li>Manage apps (/apps endpoint)</li>
            <li>Manage file uploads (/files endpoint)</li>
            <li>Get and Set plain-text (/data endpoint)</li>
            <li>Manage data through records and CRUD (/record endpoint) *Unfinished*</li>
        </ul>
        """,
    });

    options.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "X-API-KEY"
        },
        In = ParameterLocation.Header
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { key, new string[] { } } });

});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = null;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapHub<CameraHub>("/hubs/camera");
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
