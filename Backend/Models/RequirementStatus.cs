using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class RequirementStatus : IDictionaryEntity {
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
    public string Name => StatusName;
}
