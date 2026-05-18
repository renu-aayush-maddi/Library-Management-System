using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Membershiptype
{
    public int Membershiptypeid { get; set; }

    public int Maxbooks { get; set; }

    public int Maxborrowdays { get; set; }

    public string Typename { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
