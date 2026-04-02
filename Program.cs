using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Travel_Agent.Auth;
using Travel_Agent.Entities.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
 .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//add identity
builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
//Add Authetication
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
    
})
//Add jwt
.AddJwtBearer(options =>
{
    options.SaveToken=true;
    options.RequireHttpsMetadata =false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey =true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),

        ValidateIssuer = true,
        ValidIssuer= builder.Configuration["JWT:Issuer"],

         ValidateAudience = true,
        ValidAudience= builder.Configuration["JWT:Audience"]

    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapScalarApiReference();
app.Run();
