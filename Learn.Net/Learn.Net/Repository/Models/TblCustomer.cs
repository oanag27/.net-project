using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblCustomer
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal? Creditlimit { get; set; }

    public bool? IsActive { get; set; }

    public int? Taxcode { get; set; }
}
