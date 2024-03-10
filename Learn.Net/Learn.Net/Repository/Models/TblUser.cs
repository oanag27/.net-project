using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblUser
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public bool? Isactive { get; set; }

    public string? Role { get; set; }

    public string? Username { get; set; }
}
