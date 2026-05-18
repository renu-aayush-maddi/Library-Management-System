using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IReportRepository
{
    List<Borrowtransaction> GetCurrentlyBorrowedBooks();

    List<Borrowtransaction> GetOverdueBooks();

    List<Fine> GetMembersWithPendingFines();

    List<Book> GetMostBorrowedBooks();

    List<Book> GetAvailableBooksByCategory(int categoryId);

    List<Borrowtransaction> GetMemberBorrowingHistory(int memberId);
}