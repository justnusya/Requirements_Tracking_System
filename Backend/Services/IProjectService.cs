using Backend.Models;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task<bool> CreateProjectAsync(Project project);
    Task<bool> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(int id);
    Task<string> TestConnectionAsync();
}