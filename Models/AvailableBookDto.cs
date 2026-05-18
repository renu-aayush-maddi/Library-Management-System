namespace LibraryManagementSystem.Models;

public class AvailableBookDto
{
    public int Bookid { get; set; }

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int AvailableCopies { get; set; }
}