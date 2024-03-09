using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblRolepermission
{
    public int Id { get; set; }

    public string Userrole { get; set; } = null!;

    public string Menucode { get; set; } = null!;

    public bool Haveview { get; set; }

    public bool Haveadd { get; set; }

    public bool Haveedit { get; set; }

    public bool Havedelete { get; set; }
}
