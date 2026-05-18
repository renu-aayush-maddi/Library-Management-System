using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus;

public class ReportMenu
{
    private readonly ReportService reportService;

    public ReportMenu(ReportService reportService)
    {
        this.reportService = reportService;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== REPORTS =====");
            Console.WriteLine("1. Books Currently Borrowed");
            Console.WriteLine("2. Overdue Books");
            Console.WriteLine("3. Members with Pending Fines");
            Console.WriteLine("4. Most Borrowed Books");
            Console.WriteLine("5. Available Books by Category");
            Console.WriteLine("6. Member Borrowing History");
            Console.WriteLine("0. Back");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CurrentBorrowedBooks();
                    break;

                case 2:
                    OverdueBooks();
                    break;

                case 3:
                    PendingFines();
                    break;

                case 4:
                    MostBorrowedBooks();
                    break;

                case 5:
                    AvailableBooksByCategory();
                    break;

                case 6:
                    MemberBorrowingHistory();
                    break;

                case 0:
                    return;
            }
        }
    }

    private void CurrentBorrowedBooks()
    {
        var books = reportService.GetCurrentlyBorrowedBooks();

        foreach (var item in books)
        {
            Console.WriteLine();
            Console.WriteLine($"Book     : {item.Copy.Book.Title}");
            Console.WriteLine($"Member   : {item.Member.Fullname}");
            Console.WriteLine($"Due Date : {item.Duedate}");
        }
    }

    private void OverdueBooks()
    {
        var books = reportService.GetOverdueBooks();

        foreach (var item in books)
        {
            Console.WriteLine();
            Console.WriteLine($"Book   : {item.Copy.Book.Title}");
            Console.WriteLine($"Member : {item.Member.Fullname}");
            Console.WriteLine($"Due    : {item.Duedate}");
        }
    }

    private void PendingFines()
    {
        var fines = reportService.GetMembersWithPendingFines();

        foreach (var item in fines)
        {
            Console.WriteLine();
            Console.WriteLine($"Member  : {item.Member.Fullname}");
            Console.WriteLine($"Pending : {item.Pendingamount}");
        }
    }

    private void MostBorrowedBooks()
    {
        var books = reportService.GetMostBorrowedBooks();

        foreach (var item in books)
        {
            int borrowCount = item.Bookcopies.Sum(c => c.Borrowtransactions.Count);

            Console.WriteLine();
            Console.WriteLine($"Book   : {item.Title}");
            Console.WriteLine($"Count  : {borrowCount}");
        }
    }

    private void AvailableBooksByCategory()
    {
        Console.Write("Category Id: ");

        int categoryId = Convert.ToInt32(Console.ReadLine());

        var books = reportService.GetAvailableBooksByCategory(categoryId);

        foreach (var item in books)
        {
            Console.WriteLine();
            Console.WriteLine($"{item.Bookid} - {item.Title}");
        }
    }

    private void MemberBorrowingHistory()
    {
        Console.Write("Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        var history = reportService.GetMemberBorrowingHistory(memberId);

        foreach (var item in history)
        {
            Console.WriteLine();
            Console.WriteLine($"Book     : {item.Copy.Book.Title}");
            Console.WriteLine($"Borrowed : {item.Borrowdate}");
            Console.WriteLine($"Returned : {item.Returndate}");
            Console.WriteLine($"Status   : {item.Status}");
        }
    }
}