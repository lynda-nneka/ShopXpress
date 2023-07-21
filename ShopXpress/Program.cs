using System.Reflection;
using System.Text;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ShopXpress.Data.Implementations;
using ShopXpress.Models.Entities;
using ShopXpress.Services.Implementations;
using ShopXpress.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IMongoDatabase>(options =>
{
    var mongosettings = builder.Configuration.GetSection("DatabaseSettings").Get<MongoDbSettings>();
    var client = new MongoClient(mongosettings.ConnectionString);
    return client.GetDatabase(mongosettings.DatabaseName);
});

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout as per your requirement
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// mongo identity configuration.
var mongoDbIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"),
        DatabaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName")

    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = false;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(12);
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.User.RequireUniqueEmail = true;
    }
};


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = "http://localhost:24946",
        ValidAudience = "http://localhost:24946",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdf;lkjtebnshsdkds")),
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopXpress", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            Array.Empty<string>()
                    },
                });
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();

