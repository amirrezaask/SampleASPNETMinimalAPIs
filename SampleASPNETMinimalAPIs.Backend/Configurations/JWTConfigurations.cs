namespace SampleASPNETMinimalAPIs.Backend.Configurations;

public class JWTConfigurations
{
    public string Secret { get; set; }
    public TimeSpan ExpiresIn { get; set; }

    
}