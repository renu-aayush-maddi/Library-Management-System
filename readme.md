dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.11


dotnet ef dbcontext scaffold "Host=localhost;Port=5469;Database=LibraryManagement_db;Username=postgres;Password=aayush05" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c AppDbContext -f