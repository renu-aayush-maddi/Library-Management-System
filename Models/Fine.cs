using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Fine
{
    public int Fineid { get; set; }

    public int Borrowid { get; set; }

    public int Memberid { get; set; }

    public decimal Fineamount { get; set; }

    public decimal? Paidamount { get; set; }

    public decimal? Pendingamount { get; set; }

    public string? Finereason { get; set; }

    public bool? Ispaid { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Borrowtransaction Borrow { get; set; } = null!;

    public virtual ICollection<Finepayment> Finepayments { get; set; } = new List<Finepayment>();

    public virtual Member Member { get; set; } = null!;
}
