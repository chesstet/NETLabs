using PrivateLibrary.WebApi.Extensions.Startup;

var builder = WebApplication.CreateBuilder(args);

// Configuring services
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Configure();

app.Run();
