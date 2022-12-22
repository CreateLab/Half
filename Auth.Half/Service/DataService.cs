using Db.Half.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Half.Service;

public class DataService:IDataService
{
    private readonly IDbContextFactory<HalfContext> _contextFactory;

    public DataService(IDbContextFactory<HalfContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public Task<string?> GetNameAsync(string login)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Users
            .Where(u => u.Login == login)
            .Select(u => u.Name)
            .FirstOrDefaultAsync();
    }
}