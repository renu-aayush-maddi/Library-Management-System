using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Member
{
    public int Memberid { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public int Membershiptypeid { get; set; }

    public bool? Isactive { get; set; }

    public DateOnly? Joineddate { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Borrowtransaction> Borrowtransactions { get; set; } = new List<Borrowtransaction>();

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Membershiptype Membershiptype { get; set; } = null!;

    public virtual ICollection<Useraccount> Useraccounts { get; set; } = new List<Useraccount>();
}
