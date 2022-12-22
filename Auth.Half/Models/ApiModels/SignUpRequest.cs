namespace Auth.Half.Models.ApiModels;

public record SignUpRequest()
{
    public string Email { get; set; }
    public string Password { get; set; }
}
