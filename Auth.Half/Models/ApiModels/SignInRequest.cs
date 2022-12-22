namespace Auth.Half.Models.ApiModels;

public record SignInRequest()
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}