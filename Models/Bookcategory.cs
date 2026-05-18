using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class Bookcategory
{
    public int Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
