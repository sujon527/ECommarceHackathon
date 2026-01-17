using UserManagement.Core.Interfaces;
using UserManagement.Data.Configuration;
using UserManagement.Data.Repositories;
using UserManagement.Services;
using UserManagement.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register concrete validator types for explicit injection into UserService
builder.Services.AddScoped<NameValidator>();
builder.Services.AddScoped<AgeValidator>();
builder.Services.AddScoped<PasswordValidator>();
builder.Services.AddScoped<UniquenessValidator>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
