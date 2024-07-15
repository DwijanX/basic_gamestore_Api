using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using refreshProjectDotNet.Dtos.auth;

namespace refreshProjectDotNet.Endpoints;

public static class AuthEndpoint
{
    private const string TokenSecret="fFB7rBv7tIq3UHym0whhw6yZqlFM2e3b";
    private static readonly TimeSpan TokenLifetime=TimeSpan.FromHours(24);
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app){
         var group=app.MapGroup("auth").WithParameterValidation();
         group.MapPost("/getJWTTesting", async (TokenGenerationRequestDTO request) => {
            //check creds idk
            
            var tokenHandler=new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(TokenSecret);
            var claims=new List<Claim>{
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub,request.Email),
                new(JwtRegisteredClaimNames.Email,request.Email),
            };  
            foreach(var customClaim in request.CustomClaims){
                claims.Add(new Claim("Type",customClaim.Type));
            }
            var tokenDescriptor=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.UtcNow+TokenLifetime,
                Issuer="localhost",
                Audience="localhost",
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token=tokenHandler.CreateToken(tokenDescriptor);

            return Results.Ok(tokenHandler.WriteToken(token));
         });

         return group;
    }
}
