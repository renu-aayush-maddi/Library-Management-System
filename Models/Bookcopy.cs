using System;
using System.Collections.Generic;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models;

public partial class Bookcopy
{
    public int Copyid { get; set; }

    public int Bookid { get; set; }

    public string Copycode { get; set; } = null!;

    public string? Shelflocation { get; set; }

    public bool? Isavailable { get; set; }

    public bool? Isdamaged { get; set; }
    
    public BookCopyStatus Status { get; set; }

    public DateOnly? Addeddate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Borrowtransaction> Borrowtransactions { get; set; } = new List<Borrowtransaction>();
}
