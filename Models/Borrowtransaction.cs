using System;
using System.Collections.Generic;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models;

public partial class Borrowtransaction
{
    public int Borrowid { get; set; }

    public int Memberid { get; set; }

    public int Copyid { get; set; }

    public DateOnly? Borrowdate { get; set; }

    public DateOnly Duedate { get; set; }

    public DateOnly? Returndate { get; set; }

    public string? Remarks { get; set; }
    
    public BorrowStatus Status { get; set; }

    public virtual Bookcopy Copy { get; set; } = null!;

    public virtual Fine? Fine { get; set; }

    public virtual Member Member { get; set; } = null!;
}
