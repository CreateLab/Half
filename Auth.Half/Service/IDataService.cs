namespace Auth.Half.Service;

public interface IDataService
{
    Task<string?> GetNameAsync(string login);
}