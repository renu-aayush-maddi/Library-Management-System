using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Useraccount
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? Memberid { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Member? Member { get; set; }
}