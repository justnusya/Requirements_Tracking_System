using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    
    [JsonIgnore]
    public virtual User? Author { get; set; }
    public virtual Project? Project { get; set; }

    [JsonIgnore]
    public virtual RequirementStatus? Status { get; set; }

    [JsonIgnore]
    public virtual RequirementPriority? Priority { get; set; }
}