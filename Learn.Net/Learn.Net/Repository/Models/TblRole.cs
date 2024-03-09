using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblRole
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool? Status { get; set; }
}
