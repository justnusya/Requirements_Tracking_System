using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly ApplicationDbContext _context;

        public RequirementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Requirement>> GetAllDetailedAsync()
        {
            return await _context.Requirements
                .Include(r => r.Project)
                .Include(r => r.Status)
                .Include(r => r.Priority)
                .Include(r => r.Author)
                .ToListAsync();
        }

        public async Task<bool> CreateRequirementAsync(Requirement req)
        {
            try 
            {
                req.CreatedAt = DateTime.UtcNow;
                req.UpdatedAt = DateTime.UtcNow;

                _context.Requirements.Add(req);
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
                var count = await _context.Requirements.CountAsync();
                return $"Success! Requirement count: {count}";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }
    }
}