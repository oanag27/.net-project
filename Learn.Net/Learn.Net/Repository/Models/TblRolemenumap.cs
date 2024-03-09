using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblRolemenumap
{
    public int Id { get; set; }

    public string Userrole { get; set; } = null!;

    public string Menucode { get; set; } = null!;
}
