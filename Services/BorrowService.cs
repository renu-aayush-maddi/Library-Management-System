using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Services;

public class BorrowService
{
    private readonly AppDbContext context;
    private readonly IMemberRepository memberRepository;
    private readonly IBorrowRepository borrowRepository;

    public BorrowService
    (
        AppDbContext context,
        IMemberRepository memberRepository,
        IBorrowRepository borrowRepository
    )
    {
        this.context = context;
        this.memberRepository = memberRepository;
        this.borrowRepository = borrowRepository;
    }

    public void BorrowBook(int memberId, int copyId)
    {
        using var transaction = context.Database.BeginTransaction();

        try
        {
            Member? member = memberRepository.GetMemberById(memberId);

            if (member == null)
            {
                throw new Exception("Member not found");
            }

            if (member.Isactive != true)
            {
                throw new Exception("Inactive member");
            }

            decimal unpaidFine = context.Fines
                                .Where(f =>
                                    f.Memberid == memberId &&
                                    f.Ispaid != true)
                                .Sum(f => f.Pendingamount ?? 0);

            if (unpaidFine > 500)
            {
                throw new Exception ("Pending fine exceeds limit");
            }

            int activeBorrowings = borrowRepository.GetActiveBorrowCount(memberId);

            if (activeBorrowings >= member.Membershiptype.Maxbooks)
            {
                throw new Exception("Borrow limit reached");
            }

            Bookcopy? copy = context.Bookcopies.FirstOrDefault(c => c.Copyid == copyId);

            if (copy == null)
            {
                throw new Exception("Book copy not found");
            }


            if (copy.Status == BookCopyStatus.borrowed)
            {
                throw new Exception("Book is already borrowed");
            }

            if (copy.Status == BookCopyStatus.damaged)
            {
                throw new Exception("Book is damaged");
            }

            if (copy.Status != BookCopyStatus.available)
            {
                throw new Exception("Book unavailable");
            }

            Borrowtransaction borrow =
                new Borrowtransaction
                {
                    Memberid = memberId,
                    Copyid = copyId,

                    Borrowdate = DateOnly.FromDateTime(DateTime.Now),

                    Duedate = DateOnly.FromDateTime(DateTime.Now.AddDays(member.Membershiptype.Maxborrowdays)),

                    Status = BorrowStatus.borrowed
                };

            borrowRepository.AddBorrow(borrow);

            borrow.Copy.Status = BookCopyStatus.borrowed;

            borrowRepository.SaveChanges();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

   public void ReturnBook(int borrowId,int returnType)
    {
        using var transaction =
            context.Database.BeginTransaction();

        try
        {
            Borrowtransaction? borrow = borrowRepository.GetBorrowById(borrowId);

            if (borrow == null)
            {
                throw new Exception("Borrow not found");
            }

            if (borrow.Returndate != null)
            {
                throw new Exception("Book already returned");
            }

            borrow.Returndate = DateOnly.FromDateTime(DateTime.Now);

            int delayedDays = (borrow.Returndate.Value.ToDateTime(TimeOnly.MinValue)-borrow.Duedate.ToDateTime(TimeOnly.MinValue)).Days;

            Console.WriteLine("===============================");
            Console.WriteLine($"Due Date    : {borrow.Duedate}");
            Console.WriteLine($"Return Date : {borrow.Returndate}");
            Console.WriteLine($"Delayed Days: {delayedDays}");
            Console.WriteLine("===============================");

            decimal lateFine = 0;

            if (delayedDays > 0)
            {
                lateFine = delayedDays * 10;
            }

            decimal extraFine = 0;

            string fineReason = "Late return";

            if (returnType == 1)
            {
                borrow.Status = BorrowStatus.returned;
                borrow.Copy.Status = BookCopyStatus.available;
                borrow.Copy.Isavailable = true;
            }

            else if (returnType == 2)
            {
                borrow.Status = BorrowStatus.returned;
                borrow.Copy.Status = BookCopyStatus.damaged;
                borrow.Copy.Isavailable = false;
                extraFine = 200;
                fineReason = "Book damaged";
            }

            else if (returnType == 3)
            {
                borrow.Status = BorrowStatus.lost;
                borrow.Copy.Status = BookCopyStatus.damaged;
                borrow.Copy.Isavailable = false;
                extraFine = 1000;
                fineReason = "Book lost";
            }

            decimal totalFine = lateFine + extraFine;

            if (totalFine > 0)
            {
                Fine fine = new Fine
                {
                    Borrowid = borrow.Borrowid,
                    Memberid = borrow.Memberid,
                    Fineamount = totalFine,
                    Paidamount = 0,
                    Pendingamount = totalFine,
                    Finereason = fineReason,
                    Ispaid = false,
                    Createdat = DateTime.Now
                };

                context.Fines.Add(fine);

                Console.WriteLine("===============================");
                Console.WriteLine($"Fine Generated : ₹{totalFine}");
                Console.WriteLine($"Reason         : {fineReason}");
                Console.WriteLine("===============================");
            }

            context.SaveChanges();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
