using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Menus;

public class AdminMenu
{
    private readonly MemberService memberService;

    private readonly BorrowService borrowService;
    
    private readonly BookService bookService;

    private readonly FineService fineService;

    private readonly ReportService reportService;

    public AdminMenu(AppDbContext context)
    {
        MemberRepository memberRepository = new MemberRepository(context);

        BorrowRepository borrowRepository = new BorrowRepository(context);

        BookRepository bookRepository = new BookRepository(context);

        FineRepository fineRepository = new FineRepository(context);  

        ReportRepository reportRepository = new ReportRepository(context);


        reportService = new ReportService(reportRepository);      

        memberService = new MemberService(memberRepository, context);

        bookService = new BookService(bookRepository, context);

        fineService = new FineService(fineRepository);

        borrowService = new BorrowService(context, memberRepository,borrowRepository);
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== ADMIN MENU =====");

            Console.WriteLine("1. Member Management");
            Console.WriteLine("2. Borrow Book");
            Console.WriteLine("3. Return Book");
            Console.WriteLine("4. Book Management");
            Console.WriteLine("5. Fine Management");
            Console.WriteLine("6. Reports");
            Console.WriteLine("7. Exit");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    MemberMenu memberMenu = new MemberMenu(memberService);
                    memberMenu.Show();
                    break;
                case 2:
                    BorrowBook();
                    break;
                case 3:
                    ReturnBook();
                    break;
                case 4:
                    BookMenu bookMenu = new BookMenu(bookService);
                    bookMenu.Show();
                    break;
                case 5:
                    FineMenu fineMenu = new FineMenu(fineService);
                    fineMenu.Show();
                    break;
                case 6:
                    ReportMenu reportMenu = new ReportMenu(reportService);
                    reportMenu.Show();
                    break;
                case 7:
                    return;

            }
        }
    }

        private void ViewMembers()
        {
            var members = memberService.GetAllMembers();

            Console.WriteLine("========================================");
            Console.WriteLine("           MEMBERS LIST");
            Console.WriteLine("========================================");
            Console.WriteLine($"{"S.No",-6} {"Member ID",-12} {"Full Name"}");
            Console.WriteLine("========================================");

            int serialNo = 1;

            foreach (var item in members)
            {
                Console.WriteLine($"{serialNo,-6} {item.Memberid,-12} {item.Fullname}");
                serialNo++;
            }

            Console.WriteLine("========================================");
        }

    private void AddMember()
    {
        try {
        Member member = new Member();

        Console.Write("Name: ");
        member.Fullname = Console.ReadLine()!;

        Console.Write("Email: ");
        member.Email = Console.ReadLine()!;

        Console.Write("Phone: ");
        member.Phone = Console.ReadLine()!;

        Console.Write("Address: ");
        member.Address = Console.ReadLine()!;

        Console.WriteLine("Membership Types:");
        Console.WriteLine("1 - Basic Membership");
        Console.WriteLine("2 - Premium Membership");
        Console.WriteLine("3 - Premium Membership");

        Console.Write("Membership Type Id: ");


        member.Membershiptypeid = Convert.ToInt32(Console.ReadLine());

        memberService.AddMember(member);

        Console.WriteLine("==============================");
        Console.WriteLine("Member added successfully");
        Console.WriteLine("==============================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void BorrowBook()
    {
        try {

        Console.Write("Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Copy Id: ");

        int copyId = Convert.ToInt32(Console.ReadLine());

        borrowService.BorrowBook(memberId, copyId);

        Console.WriteLine("Book borrowed successfully");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ReturnBook()
    {
        try
        {
            Console.Write("Borrow Id: ");

            int borrowId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Return Type");

            Console.WriteLine("1. Normal");
            Console.WriteLine("2. Damaged");
            Console.WriteLine("3. Lost");

            Console.Write("Choose option: ");

            int returnType =
                Convert.ToInt32(Console.ReadLine());

            borrowService.ReturnBook(borrowId,returnType);

            Console.WriteLine("==============================");
            Console.WriteLine("Book returned successfully");
            Console.WriteLine("==============================");
        }
        catch (Exception ex)
        {
            Console.WriteLine("==============================");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("==============================");
        }
    }

    
}