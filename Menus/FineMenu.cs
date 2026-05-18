using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus;

public class FineMenu
{
    private readonly FineService fineService;

    public FineMenu(FineService fineService)
    {
        this.fineService = fineService;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== FINE MANAGEMENT =====");
            Console.WriteLine("1. View Pending Fines");
            Console.WriteLine("2. Pay Fine");
            Console.WriteLine("3. View Fine History");
            Console.WriteLine("4. Calculate Total Fine");
            Console.WriteLine("5. Exit");
            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ViewPendingFines();
                    break;
                case 2:
                    PayFine();
                    break;
                case 3:
                    ViewFineHistory();
                    break;
                case 4:
                        CalculateTotalFine();
                    break;
                case 5:
                    return;
            }
        }
    }

    private void ViewPendingFines()
    {
        try{
        Console.Write("Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        var fines = fineService.GetPendingFines(memberId);

        if (fines.Count == 0)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("No pending fines on this member id");
            Console.WriteLine("==============================");

            return;
        }

        foreach (var item in fines)
        {
            Console.WriteLine();
            Console.WriteLine($"Fine Id : {item.Fineid}");
            Console.WriteLine($"Amount  : {item.Pendingamount}");
            Console.WriteLine($"Reason  : {item.Finereason}");
            
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("==============================");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("==============================");
        }
    }

    private void PayFine()
    {
    try{
        Console.Write("Fine Id: ");

        int fineId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Amount: ");

        decimal amount = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Payment Mode: ");

        string paymentMode = Console.ReadLine()!;

        fineService.PayFine(fineId,amount,paymentMode);

        Console.WriteLine("Fine paid successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine("==============================");
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("==============================");
    }
    }

    private void ViewFineHistory()
    {

        try{
        Console.Write("Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        var fines = fineService.GetFineHistory(memberId);

        if(fines.Count == 0)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("No fine history on this member id");
            Console.WriteLine("==============================");

            return;
        }

        foreach (var item in fines)
        {
            Console.WriteLine();
            Console.WriteLine($"Fine Id : {item.Fineid}");
            Console.WriteLine($"Total   : {item.Fineamount}");
            Console.WriteLine($"Paid    : {item.Paidamount}");
            Console.WriteLine($"Pending : {item.Pendingamount}");
            Console.WriteLine( $"Status  : {(item.Ispaid == true ? "Paid" : "Pending")}");
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("==============================");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("==============================");
        }
    }

    private void CalculateTotalFine()
    {   
        try{
        Console.Write("Member Id: ");

        int memberId = Convert.ToInt32(Console.ReadLine());

        decimal totalFine = fineService.CalculateMemberFine(memberId);

        if(totalFine == 0)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("No pending fines on this member id");
            Console.WriteLine("==============================");

            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Total Fine for Member {memberId} : {totalFine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("==============================");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("==============================");
        }
    }
}