global using api.Models;
global using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Service;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); 

builder.Services.AddControllers().AddNewtonsoftJson(optoins => 
{
    optoins.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore ;
});

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser ,IdentityRole>(options => {
    options.Password.RequiredLength = 0 ;
    options.Password.RequireNonAlphanumeric = false ;
    options.Password.RequireUppercase = false ;
    options.Password.RequireLowercase = false ;
    options.Password.RequireDigit = false ;
    //add password roles
})
.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignOutScheme =
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme ;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidIssuer = builder.Configuration["JWT:Issuer"] ,
        ValidateAudience = true ,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true ,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddScoped<IStockRepository , StockRepository>() ;
builder.Services.AddScoped<ICommentRepository , CommentRepository>() ;
builder.Services.AddScoped<ITokenService , TokenService>() ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

