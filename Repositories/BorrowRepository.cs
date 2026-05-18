using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;


namespace LibraryManagementSystem.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly AppDbContext context;

    public BorrowRepository(AppDbContext context)
    {
        this.context = context;
    }

    public void AddBorrow(Borrowtransaction borrow)
    {
        context.Borrowtransactions.Add(borrow);
    }

    public Borrowtransaction? GetBorrowById(int borrowId)
    {
        return context.Borrowtransactions.Include(b => b.Copy).FirstOrDefault(b => b.Borrowid == borrowId);
    }

    public int GetActiveBorrowCount(int memberId)
    {
        return context.Borrowtransactions
            .Count(b => b.Memberid == memberId && b.Status == BorrowStatus.borrowed);
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
}