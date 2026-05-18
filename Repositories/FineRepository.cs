using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Repositories;

public class FineRepository : IFineRepository
{
    private readonly AppDbContext context;

    public FineRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<Fine> GetPendingFines(int memberId)
    {
        return context.Fines
            .Where(f => f.Memberid == memberId && f.Ispaid != true).ToList();
    }

    public List<Fine> GetFineHistory(int memberId)
    {
        return context.Fines
            .Include(f => f.Finepayments)
            .Where(f => f.Memberid == memberId).ToList();
    }

    public Fine? GetFineById(int fineId)
    {
        return context.Fines.FirstOrDefault(f => f.Fineid == fineId);
    }

    public void UpdateFine(Fine fine)
    {
        context.Fines.Update(fine);
    }

    public void AddFinePayment(Finepayment payment)
    {
        context.Finepayments.Add(payment);
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }

    public decimal CalculateMemberFine(int memberId)
    {
        decimal totalFine = context.Database.SqlQueryRaw<decimal>
            ($"SELECT calculate_member_fine({memberId})")
            .AsEnumerable()
            .FirstOrDefault();

        return totalFine;
    }
}