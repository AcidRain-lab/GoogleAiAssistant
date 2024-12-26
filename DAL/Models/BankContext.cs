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

    public virtual DbSet<Cashback> Cashbacks { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<CreditType> CreditTypes { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<DepositTerm> DepositTerms { get; set; }

    public virtual DbSet<DepositType> DepositTypes { get; set; }

    public virtual DbSet<DocumentsDatum> DocumentsData { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<MediaDatum> MediaData { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<NestedSubTerm> NestedSubTerms { get; set; }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<PaymentSystemType> PaymentSystemTypes { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<PhoneType> PhoneTypes { get; set; }

    public virtual DbSet<RegularPayment> RegularPayments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubTermsAndRule> SubTermsAndRules { get; set; }

    public virtual DbSet<TermsAndRule> TermsAndRules { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionSourceType> TransactionSourceTypes { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-A1DDL4K\\SQLEXPRESS;Database=bank;Trusted_Connection=True;Encrypt=False;");*/

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
            entity.Property(e => e.IsFop).HasColumnName("IsFOP");

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
            entity.Property(e => e.AllowExternalTransfers).HasDefaultValue(true);
            entity.Property(e => e.CardHolderName).HasMaxLength(100);
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.CreditLimit).HasColumnType("money");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .HasColumnName("CVV");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PinCode).HasMaxLength(4);

            entity.HasOne(d => d.BankAccount).WithMany(p => p.BankCards)
                .HasForeignKey(d => d.BankAccountId)
                .HasConstraintName("FK__BankCards__BankA__5EE9FC26");

            entity.HasOne(d => d.CardType).WithMany(p => p.BankCards)
                .HasForeignKey(d => d.CardTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankCards_CardTypes");
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
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Caption1).HasMaxLength(50);
            entity.Property(e => e.Caption2).HasMaxLength(50);
            entity.Property(e => e.Caption3).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Value1).HasMaxLength(50);
            entity.Property(e => e.Value2).HasMaxLength(50);
            entity.Property(e => e.Value3).HasMaxLength(50);

            entity.HasOne(d => d.PaymentSystemType).WithMany(p => p.CardTypes)
                .HasForeignKey(d => d.PaymentSystemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CardTypes__Payme__5A254709");
        });

        modelBuilder.Entity<Cashback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cashback__3214EC07E5F58B65");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Cashbacks)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Cashbacks_Clients");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC070BABE4E7");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PassportData).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.TaxId).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC07A497F59F");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Credits__3214EC07E48492B0");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreditAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Client).WithMany(p => p.Credits)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Credits__ClientI__0A93743A");

            entity.HasOne(d => d.CreditType).WithMany(p => p.Credits)
                .HasForeignKey(d => d.CreditTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Credits_CreditTypes");
        });

        modelBuilder.Entity<CreditType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CreditTy__3214EC0752844FDB");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreditAmount).HasMaxLength(50);
            entity.Property(e => e.CreditName).HasMaxLength(100);
            entity.Property(e => e.CreditTerm).HasMaxLength(50);
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deposits__3214EC071ACCF911");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.DepositAmount).HasColumnType("money");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Client).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Deposits_Clients");
        });

        modelBuilder.Entity<DepositTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DepositT__3214EC07F2A03135");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.DepositType).WithMany(p => p.DepositTerms)
                .HasForeignKey(d => d.DepositTypeId)
                .HasConstraintName("FK__DepositTe__Depos__10174366");
        });

        modelBuilder.Entity<DepositType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DepositT__3214EC076197C9CD");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DepositName).HasMaxLength(100);
            entity.Property(e => e.MaximumAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumAmount).HasColumnType("decimal(18, 2)");
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

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC07B8D12D12");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(50);
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

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC0724CA6D82");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__UserId__4E7E8A33");
        });

        modelBuilder.Entity<NestedSubTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NestedSu__3214EC0775FA1938");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.SubTermsAndRules).WithMany(p => p.NestedSubTerms)
                .HasForeignKey(d => d.SubTermsAndRulesId)
                .HasConstraintName("FK_NestedSubTerms_SubTermsAndRules");
        });

        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
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

        modelBuilder.Entity<RegularPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RegularP__3214EC070F9CAD29");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Frequency).HasMaxLength(50);
            entity.Property(e => e.PaymentType).HasMaxLength(50);

            entity.HasOne(d => d.Client).WithMany(p => p.RegularPayments)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_RegularPayments_Clients");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07EAEDA248");

            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SubTermsAndRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubTerms__3214EC07A37C8616");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.TermsAndRules).WithMany(p => p.SubTermsAndRules)
                .HasForeignKey(d => d.TermsAndRulesId)
                .HasConstraintName("FK_SubTermsAndRules_TermsAndRules");
        });

        modelBuilder.Entity<TermsAndRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TermsAnd__3214EC0721D2C64C");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
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

            entity.HasOne(d => d.PreferredLanguage).WithMany(p => p.Users)
                .HasForeignKey(d => d.PreferredLanguageId)
                .HasConstraintName("FK__Users__Preferred__59F03CDF");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
