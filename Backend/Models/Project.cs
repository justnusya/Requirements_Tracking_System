using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;


public partial class Project
{
    public int Id { get; set; }
    public int? ClientId { get; set; }
    [ForeignKey("ClientId")]
    public virtual Client? Client { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();

}