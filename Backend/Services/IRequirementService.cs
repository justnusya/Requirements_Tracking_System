using Backend.Models;

public interface IRequirementService
{
    Task<IEnumerable<Requirement>> GetAllDetailedAsync();
    Task<bool> CreateRequirementAsync(Requirement req);
    Task<string> TestConnectionAsync();
}