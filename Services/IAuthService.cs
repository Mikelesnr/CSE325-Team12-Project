using CSE325_Team12_Project.Models;

namespace CSE325_Team12_Project.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string name, string email, string password);
        Task<string?> LoginAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid userId);
    }
}
