using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblProductimage
{
    public int Id { get; set; }

    public string? Productcode { get; set; }

    public byte[]? Productimage { get; set; }
}
