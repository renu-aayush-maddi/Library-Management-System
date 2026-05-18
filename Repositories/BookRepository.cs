using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext context;

    public BookRepository(AppDbContext context)
    {
        this.context = context;
    }

    public void AddBook(Book book)
    {
        context.Books.Add(book);
    }

    public void AddBookCopy(Bookcopy copy)
    {
        context.Bookcopies.Add(copy);
    }

    public List<Book> GetAllBooks()
    {
        return context.Books
            .Include(b => b.Category)
            .Include(b => b.Authors)
            .ToList();
    }

    public List<Book> GetAvailableBooks()
    {
        return context.Books
            .Include(b => b.Category)
            .Include(b => b.Authors)
            .Include(b => b.Bookcopies)
            .Where(b =>
                b.Bookcopies.Any(c =>
                    c.Status == BookCopyStatus.available))
            .ToList();
    }

    public List<Book> SearchBooks(string keyword)
    {
        keyword = keyword.ToLower();

        return context.Books
            .Include(b => b.Category)
            .Include(b => b.Authors)
            .Where(b =>
                b.Title.ToLower().Contains(keyword)
                ||
                b.Category.Categoryname.ToLower().Contains(keyword)
                ||
                b.Authors.Any(a =>a.Authorname.ToLower().Contains(keyword))).ToList();
    }

    public Book? GetBookById(int bookId)
    {
        return context.Books
            .Include(b => b.Bookcopies)
            .FirstOrDefault(b => b.Bookid == bookId);
    }

    public Bookcopy? GetCopyById(int copyId)
    {
        return context.Bookcopies.FirstOrDefault(c => c.Copyid == copyId);
    }

    public void UpdateBookCopy(Bookcopy copy)
    {
        context.Bookcopies.Update(copy);
    }

    public void AddCategory(Bookcategory category)
    {
        context.Bookcategories.Add(category);
    }

    public List<Bookcategory> GetAllCategories()
    {
        return context.Bookcategories.ToList();
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
}