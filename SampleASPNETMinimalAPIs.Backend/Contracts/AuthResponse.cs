namespace SampleASPNETMinimalAPIs.Backend.Contracts;

public class AuthResponse
{
    public string RefreshToken { get; set; }
    public string Token { get; set; }
}