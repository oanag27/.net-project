using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblPwdManger
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? Modifydate { get; set; }
}
