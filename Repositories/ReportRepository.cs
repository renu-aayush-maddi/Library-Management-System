using LibraryManagementSystem.Data;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext context;

    public ReportRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<Borrowtransaction> GetCurrentlyBorrowedBooks()
    {
        return context.Borrowtransactions
            .Include(b => b.Member)
            .Include(b => b.Copy)
            .ThenInclude(c => c.Book)
            .Where(b =>b.Status == BorrowStatus.borrowed)
            .ToList();
    }

    public List<Borrowtransaction> GetOverdueBooks()
    {
        DateOnly today =
            DateOnly.FromDateTime(DateTime.Now);

        return context.Borrowtransactions
            .Include(b => b.Member)
            .Include(b => b.Copy)
            .ThenInclude(c => c.Book)
            .Where(b => b.Status ==BorrowStatus.borrowed && b.Duedate < today)
            .ToList();
    }

    public List<Fine> GetMembersWithPendingFines()
    {
        return context.Fines
            .Include(f => f.Member)
            .Where(f => f.Pendingamount > 0)
            .ToList();
    }

    public List<Book> GetMostBorrowedBooks()
    {
        return context.Books
            .Include(b => b.Bookcopies)
            .ThenInclude(c => c.Borrowtransactions)
            .OrderByDescending(b => b.Bookcopies
            .Sum(c => c.Borrowtransactions.Count))
            .Take(10)
            .ToList();
    }

    public List<Book> GetAvailableBooksByCategory(int categoryId)
    {
        return context.Books
            .Include(b => b.Bookcopies)
            .Where(b => b.Categoryid == categoryId && b.Bookcopies.Any(c => c.Status == BookCopyStatus.available))
            .ToList();
    }

    public List<Borrowtransaction> GetMemberBorrowingHistory(int memberId)
    {
        return context.Borrowtransactions
            .Include(b => b.Copy)
            .ThenInclude(c => c.Book)
            .Where(b => b.Memberid == memberId)
            .OrderByDescending(b => b.Borrowdate)
            .ToList();
    }
}