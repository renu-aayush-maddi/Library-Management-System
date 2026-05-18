using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Menus;

public class UserMenu
{
    private readonly AppDbContext context;

    private readonly BookService bookService;

    private readonly FineService fineService;

    public UserMenu(AppDbContext context)
    {
        this.context = context;

        BookRepository bookRepository = new BookRepository(context);

        FineRepository fineRepository = new FineRepository(context);

        bookService =  new BookService(bookRepository, context);

        fineService = new FineService(fineRepository);
    }

    public void Show()
    {
        Console.Write("Enter Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        var member = context.Members.Find(memberId);

        if (member == null)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("Invalid Member");
            Console.WriteLine("==============================");
            return;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== USER MENU =====");

            Console.WriteLine("1. View Profile");
            Console.WriteLine("2. View Available Books");
            Console.WriteLine("3. Search Books");
            Console.WriteLine("4. View My Borrowed Books");
            Console.WriteLine("5. View Pending Fines");
            Console.WriteLine("6. View Fine History");
            Console.WriteLine("7. Calculate Total Fine");
            Console.WriteLine("8. Exit");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ViewProfile(memberId);
                    break;

                case 2:
                    ViewAvailableBooks();
                    break;

                case 3:
                    SearchBooks();
                    break;

                case 4:
                    ViewBorrowedBooks(memberId);
                    break;

                case 5:
                    ViewPendingFines(memberId);
                    break;

                case 6:
                    ViewFineHistory(memberId);
                    break;

                case 7:
                    CalculateTotalFine(memberId);
                    break;

                case 8:
                    return;
            }
        }
    }

    private void ViewProfile(int memberId)
    {
        var member = context.Members.Find(memberId);

        Console.WriteLine();
        Console.WriteLine("===== PROFILE =====");

        Console.WriteLine($"Id      : {member!.Memberid}");
        Console.WriteLine($"Name    : {member.Fullname}");
        Console.WriteLine($"Email   : {member.Email}");
        Console.WriteLine($"Phone   : {member.Phone}");
        Console.WriteLine($"Address : {member.Address}");
        Console.WriteLine($"Active  : {member.Isactive}");
    }

    private void ViewAvailableBooks()
    {
        try
        {
            var books = bookService.GetAvailableBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No books available");
                Console.WriteLine("==============================");

                return;
            }

            foreach (var item in books)
            {
                Console.WriteLine();
                Console.WriteLine($"Book Id          : {item.Bookid}");
                Console.WriteLine($"Title            : {item.Title}");
                Console.WriteLine($"Available Copies : {item.AvailableCopies}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void SearchBooks()
    {
        try
        {
            Console.Write("Enter title/category: ");

            string keyword = Console.ReadLine()!;

            var books = bookService.SearchBooks(keyword);

            if (books.Count == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No books found");
                Console.WriteLine("==============================");

                return;
            }

            foreach (var item in books)
            {
                Console.WriteLine();
                Console.WriteLine($"Book Id      : {item.Bookid}");
                Console.WriteLine($"Title        : {item.Title}");
                Console.WriteLine($"Publisher    : {item.Publisher}");
                Console.WriteLine($"Published Yr : {item.Publishedyear}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewBorrowedBooks(int memberId)
    {
        try
        {
            var borrows = context.Borrowtransactions.Where(b => b.Memberid == memberId && b.Status == BorrowStatus.borrowed) .ToList();

            if (borrows.Count == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No borrowed books");
                Console.WriteLine("==============================");

                return;
            }

            foreach (var item in borrows)
            {
                Console.WriteLine();
                Console.WriteLine($"Borrow Id : {item.Borrowid}");
                Console.WriteLine($"Copy Id   : {item.Copyid}");
                Console.WriteLine($"Borrowed  : {item.Borrowdate}");
                Console.WriteLine($"Due Date  : {item.Duedate}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewPendingFines(int memberId)
    {
        try
        {
            var fines = fineService.GetPendingFines(memberId);

            if (fines.Count == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No pending fines");
                Console.WriteLine("==============================");

                return;
            }

            foreach (var item in fines)
            {
                Console.WriteLine();
                Console.WriteLine($"Fine Id : {item.Fineid}");
                Console.WriteLine($"Amount  : {item.Pendingamount}");
                Console.WriteLine($"Reason  : {item.Finereason}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ViewFineHistory(int memberId)
    {
        try
        {
            var fines = fineService.GetFineHistory(memberId);

            if (fines.Count == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No fine history found");
                Console.WriteLine("==============================");

                return;
            }

            foreach (var item in fines)
            {
                Console.WriteLine();
                Console.WriteLine($"Fine Id : {item.Fineid}");
                Console.WriteLine($"Total   : {item.Fineamount}");
                Console.WriteLine($"Paid    : {item.Paidamount}");
                Console.WriteLine($"Pending : {item.Pendingamount}");
                Console.WriteLine($"Status  : {(item.Ispaid == true ? "Paid" : "Pending")}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void CalculateTotalFine(int memberId)
    {
        try
        {
            decimal totalFine = fineService.CalculateMemberFine(memberId);

            if (totalFine == 0)
            {
                Console.WriteLine("==============================");
                Console.WriteLine("No existing fines");
                Console.WriteLine("==============================");

                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Total Fine : {totalFine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}