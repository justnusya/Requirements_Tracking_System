using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public virtual DbSet<Requirement> Requirements { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<RequirementStatus> RequirementStatuses { get; set; }
    public virtual DbSet<RequirementPriority> RequirementPriorities { get; set; }
    public virtual DbSet<RequirementHistory> RequirementHistories { get; set; }
    public virtual DbSet<RequirementLink> RequirementLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Requirement>(entity =>
        {
            entity.ToTable("Requirements");
            
            entity.HasIndex(e => e.AuthorId, "IX_Requirements_AuthorId");
            entity.HasIndex(e => e.ProjectId, "IX_Requirements_ProjectId");

            entity.HasOne(d => d.Project)
                  .WithMany(p => p.Requirements)
                  .HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.Author)
                  .WithMany(p => p.Requirements)
                  .HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Status)
                  .WithMany()
                  .HasForeignKey(d => d.StatusId);

            entity.HasOne(d => d.Priority)
                  .WithMany()
                  .HasForeignKey(d => d.PriorityId);
        });

        modelBuilder.Entity<RequirementLink>(entity =>
        {
            entity.ToTable("RequirementLinks");
            entity.HasKey(rl => new { rl.MainRequirementId, rl.DependentRequirementId });

            entity.HasOne(rl => rl.MainRequirement)
                  .WithMany()
                  .HasForeignKey(rl => rl.MainRequirementId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(rl => rl.DependentRequirement)
                  .WithMany()
                  .HasForeignKey(rl => rl.DependentRequirementId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Project>(entity => {
            entity.ToTable("Projects");
            entity.HasOne(p => p.Client).WithMany().HasForeignKey(p => p.ClientId);
        });

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Client>().ToTable("Clients");
        modelBuilder.Entity<RequirementHistory>().ToTable("RequirementHistory").HasKey(e => e.HistoryId);
        modelBuilder.Entity<RequirementStatus>().ToTable("RequirementStatuses");
        modelBuilder.Entity<RequirementPriority>().ToTable("RequirementPriorities");
    }   
}