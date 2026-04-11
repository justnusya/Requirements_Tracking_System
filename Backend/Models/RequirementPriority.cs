using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class RequirementPriority : IDictionaryEntity {
    public int Id { get; set; }
    public string PriorityLevel { get; set; } = null!;
    public string Name => PriorityLevel;
}
