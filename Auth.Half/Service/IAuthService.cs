using Auth.Half.Enums;
using Auth.Half.Models.Dto;

namespace Auth.Half.Service;

public interface IAuthService
{
    Task<Request> SingInAsync(string login, string password,  string username);
    Task<DataUserInfo> SingUpAsync(string login, string password);
}