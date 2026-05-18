using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Finepayment
{
    public int Paymentid { get; set; }

    public int Fineid { get; set; }

    public decimal Amountpaid { get; set; }

    public DateTime? Paymentdate { get; set; }

    public string? Paymentmode { get; set; }

    public string? Remarks { get; set; }

    public virtual Fine Fine { get; set; } = null!;
}
