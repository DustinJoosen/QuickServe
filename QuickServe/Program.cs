using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using QuickServe.Services;
using QuickServe.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IJsonService, JsonService>();
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddAutoMapper(typeof(Program));

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
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
