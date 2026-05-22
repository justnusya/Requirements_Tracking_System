using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                req.CreatedAt = DateTime.UtcNow;
                req.UpdatedAt = DateTime.UtcNow;

                _context.Requirements.Add(req);
                await _context.SaveChangesAsync(); 

                if (req.DependentRequirementIds != null && req.DependentRequirementIds.Any())
                {
                    foreach (var depId in req.DependentRequirementIds)
                    {
                        _context.RequirementLinks.Add(new RequirementLink
                        {
                            MainRequirementId = req.Id,
                            DependentRequirementId = depId,
                            DependencyType = "Requires"
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> UpdateRequirementAsync(int id, Requirement req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
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

                var oldLinks = _context.RequirementLinks.Where(rl => rl.MainRequirementId == id);
                _context.RequirementLinks.RemoveRange(oldLinks);
                await _context.SaveChangesAsync();

                if (req.DependentRequirementIds != null && req.DependentRequirementIds.Any())
                {
                    foreach (var depId in req.DependentRequirementIds)
                    {
                        _context.RequirementLinks.Add(new RequirementLink
                        {
                            MainRequirementId = id,
                            DependentRequirementId = depId,
                            DependencyType = "Requires"
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        
        public async Task<bool> DeleteRequirementAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var links = _context.RequirementLinks.Where(rl => rl.MainRequirementId == id || rl.DependentRequirementId == id);
                _context.RequirementLinks.RemoveRange(links);
                await _context.SaveChangesAsync();

                var req = await _context.Requirements.FindAsync(id);
                if (req == null) return false;

                _context.Requirements.Remove(req);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<int>> GetDependentIdsAsync(int requirementId)
        {
            return await _context.RequirementLinks
                .Where(rl => rl.MainRequirementId == requirementId)
                .Select(rl => rl.DependentRequirementId) 
                .ToListAsync();
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