using Microsoft.Extensions.Options;

namespace BookingWebApiV1.Authentication;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>

{
    private IConfiguration Configuration { get; }

    private const string JwtSectionName = "JwtSettings";
    
    public JwtOptionsSetup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void Configure(JwtOptions options)
    {
        Configuration.GetSection(JwtSectionName).Bind(options);
    }
}