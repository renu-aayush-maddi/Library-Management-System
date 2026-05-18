using System;
using System.Collections.Generic;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;
using Npgsql;

namespace LibraryManagementSystem.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Bookcategory> Bookcategories { get; set; }

    public virtual DbSet<Bookcopy> Bookcopies { get; set; }

    public virtual DbSet<Borrowtransaction> Borrowtransactions { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Finepayment> Finepayments { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Membershiptype> Membershiptypes { get; set; }

    public virtual DbSet<Useraccount> Useraccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 1. Create a data source builder with your connection string
        var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost;Port=5469;Database=LibraryManagement_db;Username=postgres;Password=aayush05");
        
        // 2. Map your C# enums to the PostgreSQL enum types
        dataSourceBuilder.MapEnum<BookCopyStatus>("book_copy_status_enum");
        dataSourceBuilder.MapEnum<BorrowStatus>("borrow_status_enum");
        dataSourceBuilder.MapEnum<MembershipTypeEnum>("membership_type_enum");

        // 3. Build the data source
        var dataSource = dataSourceBuilder.Build();

        // 4. Pass the data source to EF Core
        optionsBuilder.UseNpgsql(dataSource);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum<BookCopyStatus>("book_copy_status_enum")
            .HasPostgresEnum<BorrowStatus>("borrow_status_enum") 
            .HasPostgresEnum<MembershipTypeEnum>("membership_type_enum");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Authorid).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.Authorid).HasColumnName("authorid");
            entity.Property(e => e.Authorname)
                .HasMaxLength(150)
                .HasColumnName("authorname");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Bookid).HasName("books_pkey");

            entity.ToTable("books");

            entity.HasIndex(e => e.Isbn, "books_isbn_key").IsUnique();

            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isbn)
                .HasMaxLength(30)
                .HasColumnName("isbn");
            entity.Property(e => e.Publishedyear).HasColumnName("publishedyear");
            entity.Property(e => e.Publisher)
                .HasMaxLength(150)
                .HasColumnName("publisher");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_books_categories");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookauthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("Authorid")
                        .HasConstraintName("fk_bookauthors_authors"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("Bookid")
                        .HasConstraintName("fk_bookauthors_books"),
                    j =>
                    {
                        j.HasKey("Bookid", "Authorid").HasName("bookauthors_pkey");
                        j.ToTable("bookauthors");
                        j.IndexerProperty<int>("Bookid").HasColumnName("bookid");
                        j.IndexerProperty<int>("Authorid").HasColumnName("authorid");
                    });
        });

        modelBuilder.Entity<Bookcategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("bookcategories_pkey");

            entity.ToTable("bookcategories");

            entity.HasIndex(e => e.Categoryname, "bookcategories_categoryname_key").IsUnique();

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(150)
                .HasColumnName("categoryname");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Bookcopy>(entity =>
        {
            entity.HasKey(e => e.Copyid).HasName("bookcopies_pkey");

            entity.ToTable("bookcopies");

            entity.HasIndex(e => e.Copycode, "bookcopies_copycode_key").IsUnique();

            entity.Property(e => e.Copyid).HasColumnName("copyid");
            entity.Property(e => e.Addeddate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("addeddate");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.Copycode)
                .HasMaxLength(50)
                .HasColumnName("copycode");
            entity.Property(e => e.Isavailable)
                .HasDefaultValue(true)
                .HasColumnName("isavailable");
            entity.Property(e => e.Isdamaged)
                .HasDefaultValue(false)
                .HasColumnName("isdamaged");
            entity.Property(e => e.Shelflocation)
                .HasMaxLength(50)
                .HasColumnName("shelflocation");
            entity.Property(e => e.Status)
                .HasColumnType("book_copy_status_enum")
                .HasColumnName("status");

            entity.HasOne(d => d.Book).WithMany(p => p.Bookcopies)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookcopies_books");
        });

        modelBuilder.Entity<Borrowtransaction>(entity =>
        {
            entity.HasKey(e => e.Borrowid).HasName("borrowtransactions_pkey");

            entity.ToTable("borrowtransactions");

            entity.Property(e => e.Borrowid).HasColumnName("borrowid");
            entity.Property(e => e.Borrowdate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("borrowdate");
            entity.Property(e => e.Copyid).HasColumnName("copyid");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Memberid).HasColumnName("memberid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Returndate).HasColumnName("returndate");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Copy).WithMany(p => p.Borrowtransactions)
                .HasForeignKey(d => d.Copyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_borrow_bookcopies");

            entity.HasOne(d => d.Member).WithMany(p => p.Borrowtransactions)
                .HasForeignKey(d => d.Memberid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_borrow_members");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Fineid).HasName("fines_pkey");

            entity.ToTable("fines");

            entity.HasIndex(e => e.Borrowid, "fines_borrowid_key").IsUnique();

            entity.Property(e => e.Fineid).HasColumnName("fineid");
            entity.Property(e => e.Borrowid).HasColumnName("borrowid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Fineamount)
                .HasPrecision(10, 2)
                .HasColumnName("fineamount");
            entity.Property(e => e.Finereason).HasColumnName("finereason");
            entity.Property(e => e.Ispaid)
                .HasDefaultValue(false)
                .HasColumnName("ispaid");
            entity.Property(e => e.Memberid).HasColumnName("memberid");
            entity.Property(e => e.Paidamount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("paidamount");
            entity.Property(e => e.Pendingamount)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("(fineamount - paidamount)", true)
                .HasColumnName("pendingamount");

            entity.HasOne(d => d.Borrow).WithOne(p => p.Fine)
                .HasForeignKey<Fine>(d => d.Borrowid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fines_borrow");

            entity.HasOne(d => d.Member).WithMany(p => p.Fines)
                .HasForeignKey(d => d.Memberid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fines_members");
        });

        modelBuilder.Entity<Finepayment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("finepayments_pkey");

            entity.ToTable("finepayments");

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Amountpaid)
                .HasPrecision(10, 2)
                .HasColumnName("amountpaid");
            entity.Property(e => e.Fineid).HasColumnName("fineid");
            entity.Property(e => e.Paymentdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Paymentmode)
                .HasMaxLength(30)
                .HasColumnName("paymentmode");
            entity.Property(e => e.Remarks).HasColumnName("remarks");

            entity.HasOne(d => d.Fine).WithMany(p => p.Finepayments)
                .HasForeignKey(d => d.Fineid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_finepayments_fines");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Memberid).HasName("members_pkey");

            entity.ToTable("members");

            entity.HasIndex(e => e.Email, "members_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "members_phone_key").IsUnique();

            entity.Property(e => e.Memberid).HasColumnName("memberid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Joineddate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("joineddate");
            entity.Property(e => e.Membershiptypeid).HasColumnName("membershiptypeid");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");

            entity.HasOne(d => d.Membershiptype).WithMany(p => p.Members)
                .HasForeignKey(d => d.Membershiptypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_members_membershiptypes");
        });

        modelBuilder.Entity<Membershiptype>(entity =>
        {
            entity.HasKey(e => e.Membershiptypeid).HasName("membershiptypes_pkey");

            entity.ToTable("membershiptypes");

            entity.Property(e => e.Membershiptypeid).HasColumnName("membershiptypeid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Maxbooks).HasColumnName("maxbooks");
            entity.Property(e => e.Maxborrowdays).HasColumnName("maxborrowdays");
            entity.Property(e => e.Typename).HasColumnName("typename");
        });

        modelBuilder.Entity<Useraccount>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("useraccounts_pkey");

            entity.ToTable("useraccounts");

            entity.HasIndex(e => e.Username, "useraccounts_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Memberid).HasColumnName("memberid");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Member).WithMany(p => p.Useraccounts)
                .HasForeignKey(d => d.Memberid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_useraccounts_members");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
