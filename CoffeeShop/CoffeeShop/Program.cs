using CoffeeShop.API.DataDB;
using CoffeeShop.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoffeeShop API",
        Version = "v1"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(
                JwtBearerDefaults.AuthenticationScheme,
                doc
            ),
            new List<string>()
        }
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CoffeeShopDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeShopConnectionString")));
builder.Services.AddDbContext<CoffeeShopAuthDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeShopAuthConnectionString")));
builder.Services.AddScoped<IOrderDetailRepository, SQLOrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, SQLProductRepository>();
builder.Services.AddScoped<IOrderRepository, SQLOrderRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
}
);
builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<CoffeeShopAuthDBContext>()
.AddDefaultTokenProviders();


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("angular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = null;
    });
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CoffeeShopAuthDBContext>()
    .AddDefaultTokenProviders(); // This line adds ASP.NET Core Identity services to the application, configuring it to use IdentityRole for role management and DataProtectorTokenProvider for token generation, and specifies that the identity data should be stored in the NZWalksAuthDbContext .

var app = builder.Build();
// This line adds authentication services to the application and configures it to use JWT Bearer tokens for authentication.
app.UseHttpsRedirection();
app.UseRateLimiter();
// Swagger Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("angular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
