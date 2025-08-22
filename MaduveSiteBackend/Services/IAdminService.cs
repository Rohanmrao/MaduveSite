namespace MaduveSiteBackend.Services;

public interface IAdminService
{
    Task<IEnumerable<string>> GetDashboardStatsAsync();
    Task ApproveUserAsync(Guid userId);
    Task RejectUserAsync(Guid userId);
}