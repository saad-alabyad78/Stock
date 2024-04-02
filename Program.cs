global using api.Models;
global using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using Newtonsoft.Json;



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

builder.Services.AddScoped<IStockRepository , StockRepository>() ;
builder.Services.AddScoped<ICommentRepository , CommentRepository>() ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

