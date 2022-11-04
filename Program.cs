using FoodApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// -------------------  START ADD ---------------------
builder.Services.AddDbContext<RestDbContaxt>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulteConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey =true,
        ValidIssuer = builder.Configuration["Token:Issure"],
        ValidAudience =builder.Configuration["Token:Issure"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"])),
        ClockSkew =TimeSpan.Zero,

    };
});
// -------------------- END ADD -----------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();//add
app.UseAuthentication();//add
app.UseRouting();//add
app.UseAuthorization();
app.MapControllers();

app.Run();
//app.context.Database.EnsureCreated();


//Microsoft.IdentityModel.Tokens.