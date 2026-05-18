using LibraryManagementSystem.Services;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Menus;

public class MemberMenu
{
    private readonly MemberService memberService;

    public MemberMenu(MemberService memberService)
    {
        this.memberService = memberService;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== MEMBER MANAGEMENT =====");

            Console.WriteLine("1. View Members");
            Console.WriteLine("2. Add Member");
            Console.WriteLine("3. Search Member");
            Console.WriteLine("4. Update Membership");
            Console.WriteLine("5. Deactivate Member");
            Console.WriteLine("6. Exit");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ViewMembers();
                    break;

                case 2:
                    AddMember();
                    break;

                case 3:
                    SearchMember();
                    break;

                case 4:
                    UpdateMembership();
                    break;

                case 5:
                    DeactivateMember();
                    break;

                case 6:
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

        foreach (var item in members)
        {
            Console.WriteLine($"{item.Memberid} - {item.Fullname}");
        }
    }

    private void AddMember()
    {
        try
        {
            Member member = new Member();

            Console.Write("Name: ");
            member.Fullname = Console.ReadLine()!;

            Console.Write("Email: ");
            member.Email = Console.ReadLine()!;

            Console.Write("Phone: ");
            member.Phone = Console.ReadLine()!;

            Console.Write("Address: ");
            member.Address = Console.ReadLine()!;

            Console.Write("Membership Type Id: ");

            member.Membershiptypeid = Convert.ToInt32(Console.ReadLine());

            member.Isactive = true;

            memberService.AddMember(member);

            Console.WriteLine("Member added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void SearchMember()
    {
        try
        {
            Console.Write("Enter keyword: ");

            string keyword = Console.ReadLine()!;

            Member? member = memberService.SearchMember(keyword);

            if (member == null)
            {
                Console.WriteLine("Member not found");
                return;
            }

            Console.WriteLine($"Id: {member.Memberid}");
            Console.WriteLine($"Name: {member.Fullname}");
            Console.WriteLine($"Email: {member.Email}");
            Console.WriteLine($"Phone: {member.Phone}");
            Console.WriteLine($"Membership: {member.Membershiptype?.Typename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void UpdateMembership()
    {
        try
        {
            Console.Write("Member Id: ");

            int memberId = Convert.ToInt32(Console.ReadLine());

            Console.Write("New Membership Type Id: ");

            int membershipTypeId = Convert.ToInt32(Console.ReadLine());

            memberService.UpdateMembership( memberId, membershipTypeId);

            Console.WriteLine("Membership updated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void DeactivateMember()
    {
        try
        {
            Console.Write("Member Id: ");

            int memberId = Convert.ToInt32(Console.ReadLine());

            memberService.DeactivateMember(memberId);

            Console.WriteLine("Member deactivated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}