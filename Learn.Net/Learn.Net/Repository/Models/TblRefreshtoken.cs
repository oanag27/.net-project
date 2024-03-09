using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblRefreshtoken
{
    public string Userid { get; set; } = null!;

    public string? Tokenid { get; set; }

    public string? Refreshtoken { get; set; }
}
