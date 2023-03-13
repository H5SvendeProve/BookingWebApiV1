using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingWebApiV1.Authentication;

public  class JwtProvider : IJwtProvider
{
    
    private readonly JwtOptions jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        this.jwtOptions = jwtOptions.Value;
    }
    
    public async Task<string> GenerateNewToken(string username)
    {
        List<Claim> claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, username)
        };

        var secret = jwtOptions.Secretkey;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        
        return await Task.FromResult("Bearer " + jwt);
    }


    public async Task<bool> IsTokenValid(string token)
    {
        try
        {
            var readToken = new JwtSecurityTokenHandler().ReadToken(token);

            var dateNow = DateTime.UtcNow;

            if (readToken.ValidTo > dateNow)
            {
                return await Task.FromResult(true);
            }
        }
        catch (Exception e)
        {
            return false;
        }

        return await Task.FromResult(false);
    }
}