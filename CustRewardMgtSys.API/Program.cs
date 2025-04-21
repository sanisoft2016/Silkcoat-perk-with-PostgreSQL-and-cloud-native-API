using CustRewardMgtSys.Domain.IRepository;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Application.Service;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Infrastructure.Data;
using CustRewardMgtSys.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using CustRewardMgtSys.API;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<OrderDbContext>(option =>
//option.UseNpgsql(builder.Configuration.GetConnectionString("MicroServiceOrderDb")));

builder.AddNpgsqlDbContext<AppDbContext>("silkcoatDb");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




// Add environment variables to configuration
//builder.Configuration.AddEnvironmentVariables();

//builder.Services.AddDbContext<AppDbContext>(options => options
//    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Register Identity Services

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy => policy.AllowAnyOrigin()
//                        .AllowAnyMethod()
//                        .AllowAnyHeader());
//});


// Register Repositories & Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
builder.Services.AddScoped<IPaintCategoryService, PaintCategoryService>();
builder.Services.AddScoped<IPinCodeService, PinCodeService>();
builder.Services.AddScoped<IPinConsumptionService, PinConsumptionService>();
builder.Services.AddScoped<IPainterService, PainterService>();
builder.Services.AddScoped<IReportService, ReportService>();
//builder.Services.AddScoped<IReportService, IReportService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "naitsPortalUi",//https://promo.silkcoatnigeria.com,  http://localhost:4207, "http://silkcoat-coin-reward.demo.com",
                      policy =>
                      {
                          policy.WithOrigins("http://silkcoat-coin-reward-demo-v2.com").AllowAnyMethod().AllowAnyHeader();
                      });
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AllowAllOrigins",
//                      policy =>
//                      {
//                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//                      });
//});

var app = builder.Build();

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    //await app.ConfigureDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("naitsPortalUi"); // Apply CORS
app.UseAuthorization();

app.MapControllers();

app.Run();
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    dbContext.Database.Migrate();
//}
