using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Menus;

public class MainMenu
{
    private readonly AppDbContext context;

    public MainMenu(AppDbContext context)
    {
        this.context = context;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== LIBRARY SYSTEM =====");

            Console.WriteLine("1. Admin");
            Console.WriteLine("2. User");
            Console.WriteLine("3. Exit");

            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:

                    AdminMenu adminMenu = new AdminMenu(context);
                    adminMenu.Show();
                    break;

                case 2:

                    UserMenu userMenu = new UserMenu(context);
                    userMenu.Show();
                    break;

                case 3:
                    return;
            }
        }
    }
}