using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Project
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ProjectName { get; set; } = null!;
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public string? Description { get; set; }

    public virtual Client? Client { get; set; } = null!;
    public virtual ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();

}