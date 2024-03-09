using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblSubtable
{
    public string Code { get; set; } = null!;

    public string Menucode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool? Status { get; set; }
}
