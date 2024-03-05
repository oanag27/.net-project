namespace Learn.Net.Services
{
    public interface IRefreshHandler
    {
        Task<string> GenerateToken(string username);
    }
}
