using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class BankContext : DbContext
{
    public BankContext()
    {
    }

    public BankContext(DbContextOptions<BankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankAccountTransaction> BankAccountTransactions { get; set; }

    public virtual DbSet<BankAccountType> BankAccountTypes { get; set; }

    public virtual DbSet<BankCard> BankCards { get; set; }

    public virtual DbSet<BankCurrency> BankCurrencies { get; set; }

    public virtual DbSet<CardType> CardTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DocumentsDatum> DocumentsData { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<MediaDatum> MediaData { get; set; }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PaymentSystemType> PaymentSystemTypes { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<PhoneType> PhoneTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionSourceType> TransactionSourceTypes { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-A1DDL4K\\SQLEXPRESS;Database=bank;Trusted_Connection=True;Encrypt=False;");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Ukrainian_CI_AS");

        modelBuilder.Entity<Avatar>(entity =>
        {
            entity.HasKey(e => e.AssociatedRecordId);

            entity.Property(e => e.AssociatedRecordId).ValueGeneratedNever();
            entity.Property(e => e.Extension).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankAcco__3214EC0789479581");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.Balance).HasColumnType("money");

            entity.HasOne(d => d.BankAccountType).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.BankAccountTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_BankAccountTypes");

            entity.HasOne(d => d.BankCurrency).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.BankCurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_BankCurrencies");

            entity.HasOne(d => d.Client).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccounts_Clients");
        });

        modelBuilder.Entity<BankAccountTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankAcco__3214EC07DA8E6AB0");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .HasColumnName("IBAN");
            entity.Property(e => e.Mfo)
                .HasMaxLength(50)
                .HasColumnName("MFO");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BankAccount).WithMany(p => p.BankAccountTransactions)
                .HasForeignKey(d => d.BankAccountId)
                .HasConstraintName("FK_BankAccountTransactions_BankAccounts1");

            entity.HasOne(d => d.TransactionSourceType).WithMany(p => p.BankAccountTransactions)
                .HasForeignKey(d => d.TransactionSourceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccountTransactions_TransactionSourceTypes");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.BankAccountTransactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccountTransactions_TransactionTypes");
        });

        modelBuilder.Entity<BankAccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankAcco__3214EC075BDEC8D9");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BankCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankCard__3214EC07F92DDE2D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CardHolderName).HasMaxLength(100);
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PinCode).HasMaxLength(4);

            entity.HasOne(d => d.BankAccount).WithMany(p => p.BankCards)
                .HasForeignKey(d => d.BankAccountId)
                .HasConstraintName("FK__BankCards__BankA__5EE9FC26");

            entity.HasOne(d => d.CardType).WithMany(p => p.BankCards)
                .HasForeignKey(d => d.CardTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankCards__CardT__5FDE205F");
        });

        modelBuilder.Entity<BankCurrency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankCurr__3214EC0720AD6727");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyCode).HasMaxLength(10);
            entity.Property(e => e.CurrencyName).HasMaxLength(100);
            entity.Property(e => e.ExchangeRate)
                .HasDefaultValueSql("((1.0))")
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastUpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(5);
        });

        modelBuilder.Entity<CardType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CardType__3214EC07ED2565B8");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.PaymentSystemType).WithMany(p => p.CardTypes)
                .HasForeignKey(d => d.PaymentSystemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CardTypes__Payme__5A254709");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC070BABE4E7");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC07A497F59F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DocumentsDatum>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Extension).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Individu__3214EC0774EEE91D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PassportData).HasMaxLength(100);
            entity.Property(e => e.TaxId).HasMaxLength(50);
        });

        modelBuilder.Entity<MediaDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MediaDat__3214EC0742E47824");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Extension).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC079278F197");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AnnualTurnover).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.Industry).HasMaxLength(100);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.TaxId).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentSystemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentS__3214EC075A91AA93");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phones__3214EC07275D46FB");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.Number).HasMaxLength(20);

            entity.HasOne(d => d.PhoneType).WithMany(p => p.Phones)
                .HasForeignKey(d => d.PhoneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phones_PhoneTypes");
        });

        modelBuilder.Entity<PhoneType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhoneTyp__3214EC078A104E17");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07EAEDA248");

            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0798563922");
        });

        modelBuilder.Entity<TransactionSourceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07E0E518D9");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0745989D90");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07291D0A49");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053401972964").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
