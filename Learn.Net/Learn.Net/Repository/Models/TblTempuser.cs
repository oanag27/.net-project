using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblTempuser
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }
}
