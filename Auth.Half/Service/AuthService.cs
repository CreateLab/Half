using Auth.Half.Enums;
using Auth.Half.Extentions;
using Auth.Half.Models.Dto;
using Db.Half.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Half.Service;

public class AuthService:IAuthService
{
    private readonly IDbContextFactory<HalfContext> _contextFactory;

    public AuthService(IDbContextFactory<HalfContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task<Request> SingInAsync(string login, string password, string username)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = new User
        {
            Login = login,
            Name = username,
            PasswordHash = password.Hash(),
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return Request.Ok;
    }

    /// <inheritdoc />
    public async Task<DataUserInfo> SingUpAsync(string login, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Users.FirstOrDefaultAsync(x => x.Login == login);
        if (user == null || !password.Verify(user.PasswordHash))
            return new DataUserInfo {Request = Request.IncorrectLoginOrPassword};
        return new DataUserInfo
        {
            Request = Request.Ok,
            Name = user.Login
        };
    }
}