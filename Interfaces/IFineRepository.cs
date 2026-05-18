using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IFineRepository
{
    List<Fine> GetPendingFines(int memberId);

    List<Fine> GetFineHistory(int memberId);

    Fine? GetFineById(int fineId);

    void UpdateFine(Fine fine);

    void AddFinePayment(Finepayment payment);

    void SaveChanges();
    
    decimal CalculateMemberFine(int memberId);
}