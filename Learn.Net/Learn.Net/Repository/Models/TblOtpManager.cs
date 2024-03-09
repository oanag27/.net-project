using System;
using System.Collections.Generic;

namespace Learn.Net.Repository.Models;

public partial class TblOtpManager
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string Otptext { get; set; } = null!;

    public string? Otptype { get; set; }

    public DateTime Expiration { get; set; }

    public DateTime? Createddate { get; set; }
}
