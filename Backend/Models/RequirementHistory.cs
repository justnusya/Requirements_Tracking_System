using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class RequirementHistory
{
    public int HistoryId { get; set; }

    public int RequirementId { get; set; }

    public int? ChangedBy { get; set; }

    public int? OldStatusId { get; set; }

    public int? NewStatusId { get; set; }

    public string? ChangeReason { get; set; }

    public DateTime ChangedAt { get; set; }
}
