using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

public class ReportService
{
    private readonly IReportRepository reportRepository;

    public ReportService ( IReportRepository reportRepository)
    {
        this.reportRepository = reportRepository;
    }

    public List<Borrowtransaction> GetCurrentlyBorrowedBooks()
    {
        return reportRepository.GetCurrentlyBorrowedBooks();
    }

    public List<Borrowtransaction> GetOverdueBooks()
    {
        return reportRepository.GetOverdueBooks();
    }

    public List<Fine> GetMembersWithPendingFines()
    {
        return reportRepository.GetMembersWithPendingFines();
    }

    public List<Book> GetMostBorrowedBooks()
    {
        return reportRepository.GetMostBorrowedBooks();
    }

    public List<Book> GetAvailableBooksByCategory(int categoryId)
    {
        return reportRepository.GetAvailableBooksByCategory(categoryId);
    }

    public List<Borrowtransaction> GetMemberBorrowingHistory(int memberId)
    {
        return reportRepository.GetMemberBorrowingHistory(memberId);
    }
}