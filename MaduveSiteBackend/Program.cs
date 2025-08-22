using MaduveSiteBackend.Data;
using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MaduveSite API",
        Version = "v1",
        Description = "API for MaduveSite Backend - Matrimonial Site"
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRequestRepository, UserRequestRepository>();
builder.Services.AddScoped<IConnectRequestRepository, ConnectRequestRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserRequestService, UserRequestService>();
builder.Services.AddScoped<IConnectRequestService, ConnectRequestService>();
builder.Services.AddScoped<IProfilePhotoService, ProfilePhotoService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
builder.Services.AddScoped<ILoginService, LoginService>();

var app = builder.Build();

app.UseSwagger();
app.UseRouting();
app.UseAuthorization();
app.UseSwaggerUI();

app.MapControllers();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { 
            error = "An unexpected error occurred", 
            details = ex.Message 
        });
    }
});

using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await databaseInitializer.InitializeAsync();
}

app.Run("http://localhost:5000");