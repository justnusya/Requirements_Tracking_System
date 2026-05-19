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

        public async Task<bool> UpdateRequirementAsync(int id, Requirement req)
        {
            try
            {
                var existingReq = await _context.Requirements.FindAsync(id);
                if (existingReq == null) return false;

                existingReq.Title = req.Title;
                existingReq.Description = req.Description;
                existingReq.ProjectId = req.ProjectId;
                existingReq.PriorityId = req.PriorityId;
                existingReq.StatusId = req.StatusId;
                existingReq.UpdatedAt = DateTime.UtcNow;

                _context.Requirements.Update(existingReq);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<bool> DeleteRequirementAsync(int id)
        {
            try
            {
                var req = await _context.Requirements.FindAsync(id);
                if (req == null) return false;

                _context.Requirements.Remove(req);
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