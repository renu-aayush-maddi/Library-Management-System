using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services;

public class FineService
{
    private readonly IFineRepository fineRepository;

    public FineService(IFineRepository fineRepository)
    {
        this.fineRepository = fineRepository;
    }

    public List<Fine> GetPendingFines(int memberId)
    {
        return fineRepository.GetPendingFines(memberId);
    }

    public List<Fine> GetFineHistory(int memberId)
    {
        return fineRepository.GetFineHistory(memberId);
    }

    public void PayFine(int fineId,decimal amount,string paymentMode)
    {
        Fine? fine = fineRepository.GetFineById(fineId);

        if (fine == null)
        {
            throw new Exception("Fine not found");
        }

        if (fine.Ispaid == true)
        {
            throw new Exception("Fine already paid");
        }

        decimal pending = fine.Pendingamount ?? 0;

        if (amount <= 0)
        {
            throw new Exception("Amount must be greater than 0");
        }

        if (pending == 0)
        {
            throw new Exception("No pending fine available");
        }

        if (amount > pending)
        {
            throw new Exception(
                $"Pending amount is only {pending}"
            );
        }

        Finepayment payment =
            new Finepayment
            {
                Fineid = fineId,
                Amountpaid = amount,
                Paymentdate = DateTime.Now,
                Paymentmode = paymentMode
            };

        fineRepository.AddFinePayment(payment);

        fine.Paidamount = (fine.Paidamount ?? 0) + amount;

        fine.Pendingamount = pending - amount;

        if (fine.Pendingamount == 0)
        {
            fine.Ispaid = true;
        }

        fineRepository.UpdateFine(fine);

        fineRepository.SaveChanges();
    }

    public decimal CalculateMemberFine(int memberId)
    {
        return fineRepository.CalculateMemberFine(memberId);
    }

}