using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IBorrowRepository
{
    void AddBorrow(Borrowtransaction borrow);

    Borrowtransaction? GetBorrowById(int borrowId);

    int GetActiveBorrowCount(int memberId);

    void SaveChanges();
}