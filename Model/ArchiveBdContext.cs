using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdminArchive.Model;

public partial class ArchiveBdContext : DbContext
{
    public ArchiveBdContext()
    {
    }

    public ArchiveBdContext(DbContextOptions<ArchiveBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acess> Acesses { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CharRestrict> CharRestricts { get; set; }

    public virtual DbSet<DocType> DocTypes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Fond> Fonds { get; set; }

    public virtual DbSet<FondLog> FondLogs { get; set; }

    public virtual DbSet<FondName> FondNames { get; set; }

    public virtual DbSet<FondType> FondTypes { get; set; }

    public virtual DbSet<FondView> FondViews { get; set; }

    public virtual DbSet<HistoricalPeriod> HistoricalPeriods { get; set; }

    public virtual DbSet<IncomeSource> IncomeSources { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryLog> InventoryLogs { get; set; }

    public virtual DbSet<InventoryType> InventoryTypes { get; set; }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<MovementType> MovementTypes { get; set; }

    public virtual DbSet<Ownership> Ownerships { get; set; }

    public virtual DbSet<ReceiptReason> ReceiptReasons { get; set; }

    public virtual DbSet<Reproduction> Reproductions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SecretChar> SecretChars { get; set; }

    public virtual DbSet<StorageTime> StorageTimes { get; set; }

    public virtual DbSet<StorageUnit> StorageUnits { get; set; }

    public virtual DbSet<StorageUnitFeature> StorageUnitFeatures { get; set; }

    public virtual DbSet<UndocumentPeriod> UndocumentPeriods { get; set; }

    public virtual DbSet<UnitCategory> UnitCategories { get; set; }

    public virtual DbSet<UnitLog> UnitLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ArchiveBD;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acess>(entity =>
        {
            entity.ToTable("Acess");

            entity.Property(e => e.AcessName).HasMaxLength(50);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK_UnitActivity");

            entity.ToTable("Activity");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.ToTable("Carrier");

            entity.Property(e => e.CarrierId).HasColumnName("CarrierID");
            entity.Property(e => e.CarrierName).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<CharRestrict>(entity =>
        {
            entity.HasKey(e => e.RestrictId);

            entity.ToTable("CharRestrict");

            entity.Property(e => e.RestrictId)
                .ValueGeneratedNever()
                .HasColumnName("RestrictID");
            entity.Property(e => e.RestrictName).HasMaxLength(50);
        });

        modelBuilder.Entity<DocType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("DocType");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.DocumentId)
                .ValueGeneratedNever()
                .HasColumnName("DocumentID");
            entity.Property(e => e.Applications).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DocumentName).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(50);

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocType)
                .HasConstraintName("FK_Document_DocType");

            entity.HasOne(d => d.ReproductionNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.Reproduction)
                .HasConstraintName("FK_Document_Reproduction");

            entity.HasOne(d => d.StorageUnitNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.StorageUnit)
                .HasConstraintName("FK_Document_StorageUnit");

            entity.HasMany(d => d.Features).WithMany(p => p.Documents)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentFeature",
                    r => r.HasOne<Feature>().WithMany()
                        .HasForeignKey("Feature")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DocumentFeatures_Feature"),
                    l => l.HasOne<Document>().WithMany()
                        .HasForeignKey("Document")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DocumentFeatures_Document"),
                    j =>
                    {
                        j.HasKey("Document", "Feature");
                        j.ToTable("DocumentFeatures");
                    });
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.ToTable("Feature");

            entity.Property(e => e.FeatureName).HasMaxLength(50);
        });

        modelBuilder.Entity<Fond>(entity =>
        {
            entity.ToTable("Fond");

            entity.Property(e => e.FondId).HasColumnName("FondID");
            entity.Property(e => e.FondName).HasMaxLength(50);
            entity.Property(e => e.FondNumber).HasMaxLength(10);
            entity.Property(e => e.FondShortName).HasMaxLength(50);
            entity.Property(e => e.MovementNote).HasMaxLength(350);

            entity.HasOne(d => d.AcessNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.Acess)
                .HasConstraintName("FK_Fond_Acess");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_Fond_Category");

            entity.HasOne(d => d.CharRestrictNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.CharRestrict)
                .HasConstraintName("FK_Fond_CharRestrict");

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.DocType)
                .HasConstraintName("FK_Fond_DocType");

            entity.HasOne(d => d.HistoricalPeriodNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.HistoricalPeriod)
                .HasConstraintName("FK_Fond_HistoricalPeriods");

            entity.HasOne(d => d.IncomeSourceNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.IncomeSource)
                .HasConstraintName("FK_Fond_IncomeSource");

            entity.HasOne(d => d.MovementNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.Movement)
                .HasConstraintName("FK_Fond_Movement");

            entity.HasOne(d => d.MovementTypeNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.MovementType)
                .HasConstraintName("FK_Fond_MovementType");

            entity.HasOne(d => d.OwnerShipNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.OwnerShip)
                .HasConstraintName("FK_Fond_Ownership");

            entity.HasOne(d => d.ReceiptReasonNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.ReceiptReason)
                .HasConstraintName("FK_Fond_ReceiptReason");

            entity.HasOne(d => d.SecretCharNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.SecretChar)
                .HasConstraintName("FK_Fond_SecretChar");

            entity.HasOne(d => d.StorageTimeNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.StorageTime)
                .HasConstraintName("FK_Fond_StorageTime");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_Fond_FondType");

            entity.HasOne(d => d.ViewNavigation).WithMany(p => p.Fonds)
                .HasForeignKey(d => d.View)
                .HasConstraintName("FK_Fond_FondView");
        });

        modelBuilder.Entity<FondLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("FondLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ActivityNavigation).WithMany(p => p.FondLogs)
                .HasForeignKey(d => d.Activity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FondLog_UnitActivity");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.FondLogs)
                .HasForeignKey(d => d.Fond)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FondLog_Fond");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.FondLogs)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_User");
        });

        modelBuilder.Entity<FondName>(entity =>
        {
            entity.HasKey(e => e.NamesId);

            entity.Property(e => e.NamesId)
                .ValueGeneratedNever()
                .HasColumnName("NamesID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.FondNames)
                .HasForeignKey(d => d.Fond)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FondNames_Fond");
        });

        modelBuilder.Entity<FondType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("FondType");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<FondView>(entity =>
        {
            entity.HasKey(e => e.ViewId);

            entity.ToTable("FondView");

            entity.Property(e => e.ViewName).HasMaxLength(50);
        });

        modelBuilder.Entity<HistoricalPeriod>(entity =>
        {
            entity.HasKey(e => e.PeriodId);

            entity.Property(e => e.PeriodName).HasMaxLength(50);
        });

        modelBuilder.Entity<IncomeSource>(entity =>
        {
            entity.HasKey(e => e.SourceId);

            entity.ToTable("IncomeSource");

            entity.Property(e => e.SourceName).HasMaxLength(50);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("InventoryID");
            entity.Property(e => e.InventoryNumber).HasMaxLength(50);
            entity.Property(e => e.MovementNote).HasMaxLength(350);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(350);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.AcessNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Acess)
                .HasConstraintName("FK_Inventory_Acess");

            entity.HasOne(d => d.CarrierNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Carrier)
                .HasConstraintName("FK_Inventory_Carrier");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_Inventory_Category");

            entity.HasOne(d => d.CharRestrictNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.CharRestrict)
                .HasConstraintName("FK_Inventory_CharRestrict");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Fond)
                .HasConstraintName("FK_Inventory_Fond");

            entity.HasOne(d => d.IncomeSourceNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.IncomeSource)
                .HasConstraintName("FK_Inventory_IncomeSource");

            entity.HasOne(d => d.MovementNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Movement)
                .HasConstraintName("FK_Inventory_Movement");

            entity.HasOne(d => d.MovementTypeNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.MovementType)
                .HasConstraintName("FK_Inventory_MovementType");

            entity.HasOne(d => d.ReceiptReasonNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ReceiptReason)
                .HasConstraintName("FK_Inventory_ReceiptReason");

            entity.HasOne(d => d.StorageTimeNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StorageTime)
                .HasConstraintName("FK_Inventory_StorageTime");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_Inventory_InventoryType");
        });

        modelBuilder.Entity<InventoryLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("InventoryLog");

            entity.Property(e => e.LogId)
                .ValueGeneratedNever()
                .HasColumnName("LogID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ActivityNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.Activity)
                .HasConstraintName("FK_InventoryLog_UnitActivity");

            entity.HasOne(d => d.InventoryNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.Inventory)
                .HasConstraintName("FK_InventoryLog_Inventory");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.User)
                .HasConstraintName("FK_InventoryLog_User");
        });

        modelBuilder.Entity<InventoryType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("InventoryType");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.ToTable("Movement");

            entity.Property(e => e.MovementName).HasMaxLength(50);
        });

        modelBuilder.Entity<MovementType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("MovementType");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Ownership>(entity =>
        {
            entity.ToTable("Ownership");

            entity.Property(e => e.OwnershipId).HasColumnName("OwnershipID");
            entity.Property(e => e.OwershipName).HasMaxLength(50);
        });

        modelBuilder.Entity<ReceiptReason>(entity =>
        {
            entity.HasKey(e => e.ReasonId);

            entity.ToTable("ReceiptReason");

            entity.Property(e => e.ReasonId)
                .ValueGeneratedNever()
                .HasColumnName("ReasonID");
            entity.Property(e => e.ReasonName).HasMaxLength(50);
        });

        modelBuilder.Entity<Reproduction>(entity =>
        {
            entity.ToTable("Reproduction");

            entity.Property(e => e.ReproductionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SecretChar>(entity =>
        {
            entity.HasKey(e => e.CharId);

            entity.ToTable("SecretChar");

            entity.Property(e => e.CharName).HasMaxLength(50);
        });

        modelBuilder.Entity<StorageTime>(entity =>
        {
            entity.HasKey(e => e.TymeId);

            entity.ToTable("StorageTime");

            entity.Property(e => e.TymeId).HasColumnName("TymeID");
            entity.Property(e => e.TymeName).HasMaxLength(50);
        });

        modelBuilder.Entity<StorageUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK_StorageUnit1");

            entity.ToTable("StorageUnit");

            entity.Property(e => e.UnitId)
                .ValueGeneratedNever()
                .HasColumnName("UnitID");
            entity.Property(e => e.AccessRestrictionNote).HasMaxLength(50);
            entity.Property(e => e.Date).HasMaxLength(50);
            entity.Property(e => e.IsFm).HasColumnName("IsFM");
            entity.Property(e => e.IsSf).HasColumnName("IsSF");
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.Tite).HasMaxLength(50);
            entity.Property(e => e.UnitName).HasMaxLength(50);
            entity.Property(e => e.UnitNumber).HasMaxLength(5);

            entity.HasOne(d => d.AcessNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Acess)
                .HasConstraintName("FK_StorageUnit_Acess");

            entity.HasOne(d => d.CarrierNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Carrier)
                .HasConstraintName("FK_StorageUnit1_Carrier");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("FK_StorageUnit_Category1");

            entity.HasOne(d => d.InventoryNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Inventory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnit_Inventory");
        });

        modelBuilder.Entity<StorageUnitFeature>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.FeatureNavigation).WithMany()
                .HasForeignKey(d => d.Feature)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnitFeatures_Feature");

            entity.HasOne(d => d.UnitNavigation).WithMany()
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnitFeatures_StorageUnit");
        });

        modelBuilder.Entity<UndocumentPeriod>(entity =>
        {
            entity.HasKey(e => e.PeriodId);

            entity.Property(e => e.PeriodId)
                .ValueGeneratedNever()
                .HasColumnName("PeriodID");
            entity.Property(e => e.DocumentLocation).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Note).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.UndocumentPeriods)
                .HasForeignKey(d => d.Fond)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UndocumentPeriods_Fond");
        });

        modelBuilder.Entity<UnitCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Category_1");

            entity.ToTable("UnitCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<UnitLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("UnitLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ActivityNavigation).WithMany(p => p.UnitLogs)
                .HasForeignKey(d => d.Activity)
                .HasConstraintName("FK_UnitLog_UnitActivity");

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.UnitLogs)
                .HasForeignKey(d => d.Unit)
                .HasConstraintName("FK_UnitLog_StorageUnit");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UnitLogs)
                .HasForeignKey(d => d.User)
                .HasConstraintName("FK_UnitLog_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Midname).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
