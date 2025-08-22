using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Services;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.Authorization;
using MaduveSiteBackend.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IGenericRepository<Admin>, GenericRepository<Admin>>();
builder.Services.AddScoped<IGenericRepository<UserRequest>, GenericRepository<UserRequest>>();
builder.Services.AddScoped<IGenericRepository<ConnectRequest>, GenericRepository<ConnectRequest>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRequestRepository, UserRequestRepository>();
builder.Services.AddScoped<IConnectRequestRepository, ConnectRequestRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserRequestService, UserRequestService>();
builder.Services.AddScoped<IConnectRequestService, ConnectRequestService>();
builder.Services.AddScoped<IProfilePhotoService, ProfilePhotoService>();
builder.Services.AddScoped<IProfileImageService, ProfileImageService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

if (builder.Environment.IsProduction())
{
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<AdminPermissionAttribute>();
    });
}
else
{
    builder.Services.AddControllers();
}

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    int defaultPortHttp = 5000;
    int defaultPortHttps = 5001;
    
    int kestrelPortHttp = builder.Configuration.GetValue<int?>("KestrelPort:Http") ?? defaultPortHttp;
    int kestrelPortHttps = builder.Configuration.GetValue<int?>("KestrelPort:Https") ?? defaultPortHttps;

    serverOptions.ListenAnyIP(kestrelPortHttp);
    if (builder.Configuration.GetSection("UseHttps").Value == "true")
    {
        serverOptions.ListenAnyIP(kestrelPortHttps, listenOptions =>
        {
            listenOptions.UseHttps(builder.Configuration.GetSection("SSL:SSLCertPath").Value, builder.Configuration.GetSection("SSL:SSLCertPwd").Value);
        });
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS with the AllowAll policy
app.UseCors("AllowAll");

app.UseAuthorization();
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
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred", details = ex.Message });
    }
});

using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await databaseInitializer.InitializeAsync();
}

app.Run();