using Backend.Models;

public interface IRequirementService
{
    Task<IEnumerable<Requirement>> GetAllDetailedAsync();
    Task<bool> CreateRequirementAsync(Requirement req);
    Task<bool> UpdateRequirementAsync(int id, Requirement req);
    Task<bool> DeleteRequirementAsync(int id);
    Task<IEnumerable<int>> GetDependentIdsAsync(int requirementId);
    Task<string> TestConnectionAsync();
}