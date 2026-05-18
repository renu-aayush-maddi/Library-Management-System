using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IBookRepository
{
    void AddBook(Book book);

    void AddBookCopy(Bookcopy copy);

    List<Book> GetAllBooks();

    List<Book> GetAvailableBooks();

    List<Book> SearchBooks(string keyword);

    Book? GetBookById(int bookId);

    Bookcopy? GetCopyById(int copyId);

    void UpdateBookCopy(Bookcopy copy);

    void AddCategory(Bookcategory category);

    List<Bookcategory> GetAllCategories();

    void SaveChanges();
}