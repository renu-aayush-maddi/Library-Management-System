using LibraryManagementSystem.Data;
using LibraryManagementSystem.Menus;

AppDbContext context = new AppDbContext();

MainMenu mainMenu = new MainMenu(context);

mainMenu.Show();