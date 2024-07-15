using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using refreshProjectDotNet.Data;
using refreshProjectDotNet.Dtos;
using refreshProjectDotNet.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication((X)=>{
    X.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    X.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
    X.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x=>{
    x.TokenValidationParameters=new TokenValidationParameters{
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=builder.Configuration["JwtSettings:Issuer"],
        ValidAudience=builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey
        (Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:key"]))
    };
});

builder.Services.AddAuthorization(options=>{
    options.AddPolicy(IdentityData.AdminUserPolicyName,policy=>
    policy.RequireClaim(IdentityData.ClaimName,"admin"));
});

var connString=builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString);


var app = builder.Build();

await app.MigrateDbAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapGamesEndpoints();
app.MapAuthEndpoints();


app.Run();
