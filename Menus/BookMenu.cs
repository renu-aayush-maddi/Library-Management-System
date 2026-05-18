using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus;

public class BookMenu
{
    private readonly BookService bookService;

    public BookMenu(BookService bookService)
    {
        this.bookService = bookService;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine
            (
                "===== BOOK MANAGEMENT ====="
            );

            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Add Book Copy");
            Console.WriteLine("3. View All Books");
            Console.WriteLine("4. View Available Books");
            Console.WriteLine("5. Search Books");
            Console.WriteLine("6. Mark Damaged Copy");
            Console.WriteLine("7. Add Category");
            Console.WriteLine("8. View Categories");
            Console.WriteLine("9. Exit");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    AddBookCopy();
                    break;
                case 3:
                    ViewAllBooks();
                    break;
                case 4:
                    ViewAvailableBooks();
                    break;
                case 5:
                    SearchBooks();
                    break;
                case 6:
                    MarkDamagedCopy();
                    break;
                case 7:
                    AddCategory();
                    break;
                case 8:
                    ViewCategories();
                    break;
                case 9:
                    return;
            }
        }
    }

    private void AddBook()
    {
        try{
        Book book = new Book();

        Console.Write("Title: ");
        book.Title = Console.ReadLine()!;

        Console.Write("ISBN: ");
        book.Isbn = Console.ReadLine()!;

        Console.Write("Published Year: ");

        book.Publishedyear =Convert.ToInt32(Console.ReadLine());

        Console.Write("Category Id: ");

        book.Categoryid = Convert.ToInt32(Console.ReadLine());

        Console.Write("Publisher: ");
        book.Publisher = Console.ReadLine();

        Console.Write("Description: ");
        book.Description = Console.ReadLine();

        bookService.AddBook(book);

        Console.WriteLine("Book added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void AddBookCopy()
    {
        Bookcopy copy = new Bookcopy();

        Console.Write("Book Id: ");

        copy.Bookid = Convert.ToInt32(Console.ReadLine());

        Console.Write("Copy Code: ");

        copy.Copycode = Console.ReadLine()!;

        Console.Write("Shelf Location: ");

        copy.Shelflocation = Console.ReadLine();

        bookService.AddBookCopy(copy);

        Console.WriteLine("Book copy added successfully");
    }

    private void ViewAllBooks()
    {
        var books = bookService.GetAllBooks();

        foreach (var item in books)
        {
            Console.WriteLine();
            Console.WriteLine($"Book Id : {item.Bookid}");
            Console.WriteLine($"Title   : {item.Title}");
            Console.WriteLine($"ISBN    : {item.Isbn}");
            Console.WriteLine($"Category: {item.Category.Categoryname}");
            Console.WriteLine($"Publisher: {item.Publisher}");
        }
    }

    private void ViewAvailableBooks()
    {
        var books =
            bookService.GetAvailableBooks();

        Console.WriteLine();
        Console.WriteLine("==============================================================");
        Console.WriteLine("                    AVAILABLE BOOKS");
        Console.WriteLine("==============================================================");
        Console.WriteLine();

        Console.WriteLine($"{"ID",-5} {"TITLE",-35} {"CATEGORY",-15} {"COPIES",-5}");

        Console.WriteLine(new string('-', 65));

        foreach (var book in books)
        {
            Console.WriteLine
            (
                $"{book.Bookid,-5} " +
                $"{book.Title,-35} " +
                $"{book.Category,-15} " +
                $"{book.AvailableCopies,-5}"
            );
        }

        Console.WriteLine();
        Console.WriteLine("==============================================================");

        Console.WriteLine($"Total Available Books : {books.Count}");

        Console.WriteLine("==============================================================");
    }

    private void SearchBooks()
    {
        Console.Write("Enter title/author/category: ");

        string keyword =
            Console.ReadLine()!;

        var books =
            bookService.SearchBooks(keyword);

        Console.WriteLine();
        Console.WriteLine("================================================================================================");
        Console.WriteLine($"                                      SEARCH RESULTS : {keyword.ToUpper()}");
        Console.WriteLine("================================================================================================");
        Console.WriteLine();

        if (books.Count == 0)
        {
            Console.WriteLine("No books found");
            return;
        }

        Console.WriteLine
        (
            $"{"ID",-5} " +
            $"{"TITLE",-35} " +
            $"{"CATEGORY",-20} " +
            $"{"ISBN",-20}"
        );

        Console.WriteLine(new string('-', 90));

        foreach (var book in books)
        {
            Console.WriteLine
            (
                $"{book.Bookid,-5} " +
                $"{book.Title,-35} " +
                $"{book.Category.Categoryname,-20} " +
                $"{book.Isbn,-20}"
            );
        }

        Console.WriteLine();
        Console.WriteLine("================================================================================================");

        Console.WriteLine($"Total Results : {books.Count}");

        Console.WriteLine("================================================================================================");
    }

    private void MarkDamagedCopy()
    {
        Console.Write("Copy Id: ");

        int copyId = Convert.ToInt32(Console.ReadLine());

        bookService.MarkDamaged(copyId);

        Console.WriteLine("Book copy marked damaged");
    }

    private void AddCategory()
    {
        Bookcategory category = new Bookcategory();

        Console.Write("Category Name: ");

        category.Categoryname = Console.ReadLine()!;

        Console.Write("Description: ");

        category.Description = Console.ReadLine();

        bookService.AddCategory(category);

        Console.WriteLine("Category added successfully");
    }

    private void ViewCategories()
    {
        var categories = bookService.GetAllCategories();

        foreach (var item in categories)
        {
            Console.WriteLine();
            Console.WriteLine($"Category Id   : {item.Categoryid}");
            Console.WriteLine($"Category Name : {item.Categoryname}");
            Console.WriteLine($"Description   : {item.Description}");
        }
}
}