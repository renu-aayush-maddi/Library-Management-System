using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Services;

public class BookService
{
    private readonly IBookRepository bookRepository;
    private readonly AppDbContext context;

    public BookService(IBookRepository bookRepository , AppDbContext context)
    {
        this.bookRepository = bookRepository;
        this.context = context;
    }

    public void AddBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new Exception("Book title is required");
            }

        Bookcategory? category = context.Bookcategories.FirstOrDefault(c =>
                                c.Categoryid == book.Categoryid);

        if (category == null)
        {
            throw new Exception("Invalid category id");
        }

        bool duplicateIsbn = bookRepository.GetAllBooks().Any(b => b.Isbn == book.Isbn);

        if (duplicateIsbn)
        {
            throw new Exception("ISBN already exists");
        }

        book.Createdat = DateTime.Now;

        bookRepository.AddBook(book);

        bookRepository.SaveChanges();
    }

    public void AddBookCopy(Bookcopy copy)
    {
        copy.Status = BookCopyStatus.available;

        copy.Addeddate = DateOnly.FromDateTime(DateTime.Now);

        bookRepository.AddBookCopy(copy);

        bookRepository.SaveChanges();
    }

    public List<Book> GetAllBooks()
    {
        return bookRepository.GetAllBooks();
    }

    public List<AvailableBookDto> GetAvailableBooks()
    {
        return context.Books
            .Select(b => new AvailableBookDto
            {
                Bookid = b.Bookid,
                Title = b.Title,
                Category = b.Category.Categoryname,

                AvailableCopies = b.Bookcopies.Count(c =>
                                            c.Status == BookCopyStatus.available)
            })
            .Where(b => b.AvailableCopies > 0)
            .ToList();
    }
    public List<Book> SearchBooks(string keyword)
    {
        return bookRepository.SearchBooks(keyword);
    }

    public void MarkDamaged(int copyId)
    {
        Bookcopy? copy = bookRepository.GetCopyById(copyId);

        if (copy == null)
        {
            throw new Exception("Book copy not found");
        }

        copy.Status = BookCopyStatus.damaged;
        bookRepository.UpdateBookCopy(copy);
        bookRepository.SaveChanges();
    }

    public void AddCategory(Bookcategory category)
    {
        bool categoryExists =
            bookRepository
            .GetAllCategories()
            .Any(c => c.Categoryname.ToLower() == category.Categoryname.ToLower());

        if (categoryExists)
        {
            throw new Exception("Category already exists");
        }

        bookRepository.AddCategory(category);

        bookRepository.SaveChanges();
    }

    public List<Bookcategory> GetAllCategories()
    {
        return bookRepository.GetAllCategories();
    }
}