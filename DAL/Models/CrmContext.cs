using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class CrmContext : DbContext
{
    public CrmContext()
    {
    }

    public CrmContext(DbContextOptions<CrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<CatalogType> CatalogTypes { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<ContactsType> ContactsTypes { get; set; }

    public virtual DbSet<ContactsTypesList> ContactsTypesLists { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CuisineType> CuisineTypes { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishTechnologyCard> DishTechnologyCards { get; set; }

    public virtual DbSet<DishesMeal> DishesMeals { get; set; }

    public virtual DbSet<DocumentsDatum> DocumentsData { get; set; }

    public virtual DbSet<Estimate> Estimates { get; set; }

    public virtual DbSet<EstimateCalculateType> EstimateCalculateTypes { get; set; }

    public virtual DbSet<EstimateLabor> EstimateLabors { get; set; }

    public virtual DbSet<EstimateMaterial> EstimateMaterials { get; set; }

    public virtual DbSet<EstimateSection> EstimateSections { get; set; }

    public virtual DbSet<EstimateStatus> EstimateStatuses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobCategory> JobCategories { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<LeadSource> LeadSources { get; set; }

    public virtual DbSet<LeadStatus> LeadStatuses { get; set; }

    public virtual DbSet<LeadsTrade> LeadsTrades { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MealHistoryDetail> MealHistoryDetails { get; set; }

    public virtual DbSet<MealTemplate> MealTemplates { get; set; }

    public virtual DbSet<MealTemplateDetail> MealTemplateDetails { get; set; }

    public virtual DbSet<MealType> MealTypes { get; set; }

    public virtual DbSet<Measure> Measures { get; set; }

    public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }

    public virtual DbSet<MediaDatum> MediaData { get; set; }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<OrdersMeal> OrdersMeals { get; set; }

    public virtual DbSet<OrdersMealsHistory> OrdersMealsHistories { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<OrdersState> OrdersStates { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<PhoneType> PhoneTypes { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsCookingBasket> ProductsCookingBaskets { get; set; }

    public virtual DbSet<ProductsExtInfo> ProductsExtInfos { get; set; }

    public virtual DbSet<ProductsType> ProductsTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesTool> RolesTools { get; set; }

    public virtual DbSet<SectionsItem> SectionsItems { get; set; }

    public virtual DbSet<Shoping> Shopings { get; set; }

    public virtual DbSet<ShopingList> ShopingLists { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamUser> TeamUsers { get; set; }

    public virtual DbSet<Tool> Tools { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<TradeTemplate> TradeTemplates { get; set; }

    public virtual DbSet<TradeTemplateLabor> TradeTemplateLabors { get; set; }

    public virtual DbSet<TradeTemplateMaterial> TradeTemplateMaterials { get; set; }

    public virtual DbSet<TradeType> TradeTypes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersTool> UsersTools { get; set; }

    public virtual DbSet<WorkType> WorkTypes { get; set; }

    public virtual DbSet<WriteOff> WriteOffs { get; set; }

    public virtual DbSet<WriteOffType> WriteOffTypes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-9M6MTGC\\SQLEXPRESS02;Database=CRM;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.EventLabel).HasMaxLength(50);
            entity.Property(e => e.EventLocation).HasMaxLength(50);
            entity.Property(e => e.EventUrl)
                .HasMaxLength(50)
                .HasColumnName("EventURL");
            entity.Property(e => e.GoogleMapsPlaceId)
                .HasMaxLength(255)
                .HasColumnName("GoogleMapsPlaceID");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(50);

            entity.HasOne(d => d.Contact).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Contacts");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Users");
        });

        modelBuilder.Entity<Avatar>(entity =>
        {
            entity.HasKey(e => e.AssociatedRecordId);

            entity.Property(e => e.AssociatedRecordId).ValueGeneratedNever();
            entity.Property(e => e.Extension).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<CatalogType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_AfterDeleteContacts"));

            entity.HasIndex(e => e.Email, "UQ_Contacts_Email").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CompanyJobTitle).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.CrossReference).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.BilllingAddress).WithMany(p => p.ContactBilllingAddresses)
                .HasForeignKey(d => d.BilllingAddressId)
                .HasConstraintName("FK_Contacts_Locations");

            entity.HasOne(d => d.MailingAddress).WithMany(p => p.ContactMailingAddresses)
                .HasForeignKey(d => d.MailingAddressId)
                .HasConstraintName("FK_Contacts_Locations1");
        });

        modelBuilder.Entity<ContactsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leads__3214EC07EA5678EA");

            entity.ToTable("ContactsType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ContactsTypesList>(entity =>
        {
            entity.ToTable("ContactsTypesList");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Contact).WithMany(p => p.ContactsTypesLists)
                .HasForeignKey(d => d.ContactId)
                .HasConstraintName("FK_ContactsTypesList_Contacts");

            entity.HasOne(d => d.ContactType).WithMany(p => p.ContactsTypesLists)
                .HasForeignKey(d => d.ContactTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContactsTypesList_ContactsType");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CuisineType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Calories).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CousineType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CousineTypeId)
                .HasConstraintName("FK_Dishes_CuisineTypes");
        });

        modelBuilder.Entity<DishTechnologyCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DishTechnologyCard_1");

            entity.ToTable("DishTechnologyCard");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.Calories).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.Netto).HasColumnType("decimal(18, 1)");

            entity.HasOne(d => d.Dish).WithMany(p => p.DishTechnologyCards)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK_DishTechnologyCard_Dishes");

            entity.HasOne(d => d.Product).WithMany(p => p.DishTechnologyCards)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_DishTechnologyCard_Products");
        });

        modelBuilder.Entity<DishesMeal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DishesMeal_1");

            entity.ToTable("DishesMeal");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Dish).WithMany(p => p.DishesMeals)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishesMeal_Dishes");

            entity.HasOne(d => d.MealType).WithMany(p => p.DishesMeals)
                .HasForeignKey(d => d.MealTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishesMeal_MealTypes");
        });

        modelBuilder.Entity<DocumentsDatum>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Extension).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.ObjectType).WithMany(p => p.DocumentsData)
                .HasForeignKey(d => d.ObjectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentsData_ObjectTypes");
        });

        modelBuilder.Entity<Estimate>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PriceLabor)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PriceMaterial)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Square).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CalculateType).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.CalculateTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimates_EstimateCalculateType");

            entity.HasOne(d => d.JobCategory).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.JobCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimates_JobCategories");

            entity.HasOne(d => d.Status).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Estimates_EstimateStatus");

            entity.HasOne(d => d.WorkType).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.WorkTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimates_WorkTypes");
        });

        modelBuilder.Entity<EstimateCalculateType>(entity =>
        {
            entity.ToTable("EstimateCalculateType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<EstimateLabor>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CostUnit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Measurement)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PriceUnit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Waste)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.AssociatedRecord).WithMany(p => p.EstimateLabors)
                .HasForeignKey(d => d.AssociatedRecordId)
                .HasConstraintName("FK_EstimateLabors_SectionsItems");

            entity.HasOne(d => d.MeasurementType).WithMany(p => p.EstimateLabors)
                .HasForeignKey(d => d.MeasurementTypeId)
                .HasConstraintName("FK_EstimateLabors_MeasurementTypes");
        });

        modelBuilder.Entity<EstimateMaterial>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CatalogImportCode).HasMaxLength(50);
            entity.Property(e => e.Cost)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.AssociatedRecord).WithMany(p => p.EstimateMaterials)
                .HasForeignKey(d => d.AssociatedRecordId)
                .HasConstraintName("FK_EstimateMaterials_SectionsItems");

            entity.HasOne(d => d.CatalogType).WithMany(p => p.EstimateMaterials)
                .HasForeignKey(d => d.CatalogTypeId)
                .HasConstraintName("FK_EstimateMaterials_CatalogTypes");
        });

        modelBuilder.Entity<EstimateSection>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Estimate).WithMany(p => p.EstimateSections)
                .HasForeignKey(d => d.EstimateId)
                .HasConstraintName("FK_EstimateSections_Estimates");
        });

        modelBuilder.Entity<EstimateStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EstimateStates");

            entity.ToTable("EstimateStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Estimate).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.EstimateId)
                .HasConstraintName("FK_Invoices_Estimates");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_AfterDeleteJobs"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Estimate).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.EstimateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Jobs_Estimates");

            entity.HasOne(d => d.Status).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Jobs_JobStatus");

            entity.HasOne(d => d.Team).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Jobs_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Jobs_Users1");
        });

        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.ToTable("JobStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_AfterDeleteLeads"));

            entity.HasIndex(e => e.Email, "UQ_Lead_Email").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.CrossReference).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.LeadBillingAddresses)
                .HasForeignKey(d => d.BillingAddressId)
                .HasConstraintName("FK_Leads_Locations1");

            entity.HasOne(d => d.JobCategory).WithMany(p => p.Leads)
                .HasForeignKey(d => d.JobCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leads_JobCategories");

            entity.HasOne(d => d.LeadSource).WithMany(p => p.Leads)
                .HasForeignKey(d => d.LeadSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leads_LeadSource");

            entity.HasOne(d => d.LeadStatus).WithMany(p => p.Leads)
                .HasForeignKey(d => d.LeadStatusId)
                .HasConstraintName("FK_Leads_LeadStatus");

            entity.HasOne(d => d.LocationAddress).WithMany(p => p.LeadLocationAddresses)
                .HasForeignKey(d => d.LocationAddressId)
                .HasConstraintName("FK_Leads_Locations2");

            entity.HasOne(d => d.MailingAddress).WithMany(p => p.LeadMailingAddresses)
                .HasForeignKey(d => d.MailingAddressId)
                .HasConstraintName("FK_Leads_Locations");

            entity.HasOne(d => d.Priority).WithMany(p => p.Leads)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK_Leads_Priority");

            entity.HasOne(d => d.WorkType).WithMany(p => p.Leads)
                .HasForeignKey(d => d.WorkTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leads_WorkTypes");
        });

        modelBuilder.Entity<LeadSource>(entity =>
        {
            entity.ToTable("LeadSource");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LeadStatus>(entity =>
        {
            entity.ToTable("LeadStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LeadsTrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Table_1");

            entity.HasOne(d => d.Lead).WithMany(p => p.LeadsTrades)
                .HasForeignKey(d => d.LeadId)
                .HasConstraintName("FK_LeadsTrades_Leads");

            entity.HasOne(d => d.TradeType).WithMany(p => p.LeadsTrades)
                .HasForeignKey(d => d.TradeTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeadsTrades_TradeTypes");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.GoogleMapsPlaceId)
                .HasMaxLength(255)
                .HasColumnName("GoogleMapsPlaceID");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Street).HasMaxLength(150);
            entity.Property(e => e.SuiteAptUnit).HasMaxLength(50);
            entity.Property(e => e.Zip).HasColumnName("ZIP");

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Locations_Countries");

            entity.HasOne(d => d.State).WithMany(p => p.Locations)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Locations_States");
        });

        modelBuilder.Entity<MealHistoryDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CustomerPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Dish).WithMany(p => p.MealHistoryDetails)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MealHistoryDetails_Dishes");

            entity.HasOne(d => d.MealHistory).WithMany(p => p.MealHistoryDetails)
                .HasForeignKey(d => d.MealHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MealHistoryDetails_OrdersMealsHistory");
        });

        modelBuilder.Entity<MealTemplate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.MealType).WithMany(p => p.MealTemplates)
                .HasForeignKey(d => d.MealTypeId)
                .HasConstraintName("FK_MealTemplates_MealTypes");
        });

        modelBuilder.Entity<MealTemplateDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MealTemplateDetails)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MealTemplateDetails_Dishes");

            entity.HasOne(d => d.MealTemplate).WithMany(p => p.MealTemplateDetails)
                .HasForeignKey(d => d.MealTemplateId)
                .HasConstraintName("FK_MealTemplateDetails_MealTemplates");
        });

        modelBuilder.Entity<MealType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Measure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Measures_1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MeasurementType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
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

            entity.HasOne(d => d.ObjectType).WithMany(p => p.MediaData)
                .HasForeignKey(d => d.ObjectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MediaData_ObjectTypes");
        });

        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<OrdersMeal>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateDelivery).HasColumnType("datetime");
            entity.Property(e => e.Invoice).HasMaxLength(50);

            entity.HasOne(d => d.MealHistory).WithMany(p => p.OrdersMeals)
                .HasForeignKey(d => d.MealHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersMeals_OrdersMealsHistory");

            entity.HasOne(d => d.OrderState).WithMany(p => p.OrdersMeals)
                .HasForeignKey(d => d.OrderStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersMeals_OrdersStates");
        });

        modelBuilder.Entity<OrdersMealsHistory>(entity =>
        {
            entity.ToTable("OrdersMealsHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<OrdersProduct>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.CustomerPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersProducts_OrdersMeals");
        });

        modelBuilder.Entity<OrdersState>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Number).HasMaxLength(20);

            entity.HasOne(d => d.PhoneType).WithMany(p => p.Phones)
                .HasForeignKey(d => d.PhoneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phones_PhoneTypes");
        });

        modelBuilder.Entity<PhoneType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.ToTable("Priority");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Products_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CaloriesPer100).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarbsPer100).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FatsPer100).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProteinsPer100).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Measure).WithMany(p => p.Products)
                .HasForeignKey(d => d.MeasureId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Products_Measures");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Products_ProductsTypes");
        });

        modelBuilder.Entity<ProductsCookingBasket>(entity =>
        {
            entity.ToTable("ProductsCookingBasket");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsCookingBaskets)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductsCookingBasket_Products");

            entity.HasOne(d => d.Stock).WithMany(p => p.ProductsCookingBaskets)
                .HasForeignKey(d => d.StockId)
                .HasConstraintName("FK_ProductsCookingBasket_Stock");
        });

        modelBuilder.Entity<ProductsExtInfo>(entity =>
        {
            entity.ToTable("ProductsExtInfo");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsExtInfos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsExtInfo_Products");
        });

        modelBuilder.Entity<ProductsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductsTypes_1");

            entity.HasIndex(e => e.Name, "UQ_ProductsTypes_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RolesTool>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.RolesTools)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesTools_Roles");

            entity.HasOne(d => d.Tool).WithMany(p => p.RolesTools)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesTools_Tools");
        });

        modelBuilder.Entity<SectionsItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EstimateTamplates");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.EstimateSection).WithMany(p => p.SectionsItems)
                .HasForeignKey(d => d.EstimateSectionId)
                .HasConstraintName("FK_SectionsItems_EstimateSections");
        });

        modelBuilder.Entity<Shoping>(entity =>
        {
            entity.ToTable("Shoping");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ShopingDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Shopings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shoping_Users");
        });

        modelBuilder.Entity<ShopingList>(entity =>
        {
            entity.ToTable("ShopingList");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ShopingLists)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopingList_Products");

            entity.HasOne(d => d.Shoping).WithMany(p => p.ShopingLists)
                .HasForeignKey(d => d.ShopingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopingList_Shoping");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stock");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Products");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.MainUser).WithMany(p => p.Teams)
                .HasForeignKey(d => d.MainUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_Users");
        });

        modelBuilder.Entity<TeamUser>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamUsers_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamUsers_Users");
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TradeTemplate>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Trade).WithMany(p => p.TradeTemplates)
                .HasForeignKey(d => d.TradeId)
                .HasConstraintName("FK_TradeTemplates_Trades");
        });

        modelBuilder.Entity<TradeTemplateLabor>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CostUnit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Measurement)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PriceUnit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Waste)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.AssociatedRecord).WithMany(p => p.TradeTemplateLabors)
                .HasForeignKey(d => d.AssociatedRecordId)
                .HasConstraintName("FK_TradeTemplateLabors_TradeTemplates");

            entity.HasOne(d => d.MeasurementType).WithMany(p => p.TradeTemplateLabors)
                .HasForeignKey(d => d.MeasurementTypeId)
                .HasConstraintName("FK_TradeTemplateLabors_MeasurementTypes");
        });

        modelBuilder.Entity<TradeTemplateMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TradeTemplateItems");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CatalogImportCode).HasMaxLength(50);
            entity.Property(e => e.Cost)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.AssociatedRecord).WithMany(p => p.TradeTemplateMaterials)
                .HasForeignKey(d => d.AssociatedRecordId)
                .HasConstraintName("FK_TradeTemplateMaterials_TradeTemplates");

            entity.HasOne(d => d.CatalogType).WithMany(p => p.TradeTemplateMaterials)
                .HasForeignKey(d => d.CatalogTypeId)
                .HasConstraintName("FK_TradeTemplateMaterials_CatalogTypes");
        });

        modelBuilder.Entity<TradeType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC078C1C0E56");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users");

            entity.ToTable(tb => tb.HasTrigger("trg_AfterDeleteUser"));

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ_Users_Phone").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ_Users_UserName").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("((sysdatetimeoffset() AT TIME ZONE 'Eastern Standard Time'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmailPassword).HasMaxLength(150);
            entity.Property(e => e.EmailSmtpServer).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UsersTool>(entity =>
        {
            entity.HasOne(d => d.Tool).WithMany(p => p.UsersTools)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersTools_Tools");
        });

        modelBuilder.Entity<WorkType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<WriteOff>(entity =>
        {
            entity.ToTable("WriteOff");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Brutto).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.Count).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);

            entity.HasOne(d => d.WriteOffType).WithMany(p => p.WriteOffs)
                .HasForeignKey(d => d.WriteOffTypeId)
                .HasConstraintName("FK_WriteOff_WriteOffTypes");
        });

        modelBuilder.Entity<WriteOffType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
