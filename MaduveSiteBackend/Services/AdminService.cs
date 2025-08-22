using MaduveSiteBackend.Repositories;

namespace MaduveSiteBackend.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;

    public AdminService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<string>> GetDashboardStatsAsync()
    {
        var users = (await _userRepository.GetAllAsync()).ToList();
        var activeUsers = users.Count(u => u.Status == Models.ProfileStatus.Active);
        var pendingUsers = users.Count(u => u.Status == Models.ProfileStatus.Pending);

        return new List<string>
        {
            $"Total Users: {users.Count}",
            $"Active Users: {activeUsers}",
            $"Pending Approvals: {pendingUsers}"
        };
    }

    public Task ApproveUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task RejectUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}