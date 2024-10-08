using ApiGateway;
using ApiGateway.HostingExtensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// add the service and config for all auth related stuff
builder.ConfigureAuth()
    .ConfigureYarpProviders(); // yarp proxy for routing and adding the access token as a header

// redis for the cached session store for cookies
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureAuthMiddleware()
    .ConfigureYarpMiddleware();

app.MapControllers();
app.Run();