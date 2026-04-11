using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Requirement
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public int PriorityId { get; set; }

    public decimal EstimatedHours { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
    public virtual RequirementStatus Status { get; set; } = null!;
    public virtual RequirementPriority Priority { get; set; } = null!;
}
