using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Client) 
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CreateProjectAsync(Project project)
        {
            try 
            {
                if (project.StartDate != default)
                {
                    project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                }

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка створення: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            try
            {
                if (project.StartDate != default)
                {
                    project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                }

                _context.Entry(project).State = EntityState.Modified;
                _context.Entry(project).Reference(p => p.Client).IsModified = false;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка оновлення: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null) return false;

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> TestConnectionAsync()
        {
            try
            {
                var count = await _context.Projects.CountAsync();
                return $"Success! Projects count: {count}";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }
    }
}