using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();
}
