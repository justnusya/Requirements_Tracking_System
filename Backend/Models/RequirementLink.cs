using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class RequirementLink
{
    public int MainRequirementId { get; set; }

    public int DependentRequirementId { get; set; }

    public string? DependencyType { get; set; }
    public virtual Requirement MainRequirement { get; set; } = null!;
    public virtual Requirement DependentRequirement { get; set; } = null!;
}
