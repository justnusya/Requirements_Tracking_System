using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
