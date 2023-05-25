using AdminArchive.Classes;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

    public virtual DbSet<Authenticity> Authenticities { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CharRestrict> CharRestricts { get; set; }

    public virtual DbSet<Condition> Conditions { get; set; }

    public virtual DbSet<DocType> DocTypes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentFile> DocumentFiles { get; set; }

    public virtual DbSet<DocumentLog> DocumentLogs { get; set; }

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

    public virtual DbSet<UndocumentPeriod> UndocumentPeriods { get; set; }

    public virtual DbSet<UnitCategory> UnitCategories { get; set; }

    public virtual DbSet<UnitCompletedWork> UnitCompletedWorks { get; set; }

    public virtual DbSet<UnitCondition> UnitConditions { get; set; }

    public virtual DbSet<UnitLog> UnitLogs { get; set; }

    public virtual DbSet<UnitRequiredWork> UnitRequiredWorks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    private string StringCon;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            //StringCon = AppSettings.Default.ConString;
            if (string.IsNullOrWhiteSpace(StringCon))
                StringCon = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt")) ?? "";
            optionsBuilder.UseSqlServer(StringCon).EnableSensitiveDataLogging();
            AppSettings.Default.ConString = StringCon;
        }
        catch
        {
            try
            {
                StringCon = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt")) ?? "";
                optionsBuilder.UseSqlServer(StringCon);
                AppSettings.Default.ConString = StringCon;
            }
            catch (Exception innerEx)
            {
                MessageBoxs.Show(innerEx.ToString(), "Ошибка подключения к базе данных", MessageBoxs.Buttons.OK, MessageBoxs.Icon.Error);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acess>(entity =>
        {
            entity.ToTable("Acess");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitActivity");

            entity.ToTable("Activity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Authenticity>(entity =>
        {
            entity.ToTable("Authenticity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.ToTable("Carrier");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CharRestrict>(entity =>
        {
            entity.ToTable("CharRestrict");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Condition>(entity =>
        {
            entity.ToTable("Condition");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DocType>(entity =>
        {
            entity.ToTable("DocType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Annotation).HasMaxLength(350);
            entity.Property(e => e.Applications).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(50);

            entity.HasOne(d => d.AuthenticityNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.Authenticity)
                .HasConstraintName("FK_Document_Authenticity");

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_DocType");

            entity.HasOne(d => d.ReproductionNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.Reproduction)
                .HasConstraintName("FK_Document_Reproduction");

            entity.HasOne(d => d.StorageUnitNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.StorageUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
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

        modelBuilder.Entity<DocumentFile>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.Extension).HasMaxLength(5);
            entity.Property(e => e.FileName).HasMaxLength(50);

            entity.HasOne(d => d.DocumentNavigation).WithMany(p => p.DocumentFiles)
                .HasForeignKey(d => d.Document)
                .HasConstraintName("FK_DocumentFiles_Document");
        });

        modelBuilder.Entity<DocumentLog>(entity =>
        {
            entity.ToTable("DocumentLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ActivityNavigation).WithMany(p => p.DocumentLogs)
                .HasForeignKey(d => d.Activity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentLog_Activity");

            entity.HasOne(d => d.DocumentNavigation).WithMany(p => p.DocumentLogs)
                .HasForeignKey(d => d.Document)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentLog_Document");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.ToTable("Feature");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Fond>(entity =>
        {
            entity.ToTable("Fond");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Annotation).HasMaxLength(350);
            entity.Property(e => e.HistoricalOverview).HasMaxLength(350);
            entity.Property(e => e.Index).HasMaxLength(1);
            entity.Property(e => e.Literal).HasMaxLength(2);
            entity.Property(e => e.MovementNote).HasMaxLength(350);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
            entity.Property(e => e.ShortName).HasMaxLength(76);

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
                .OnDelete(DeleteBehavior.ClientSetNull)
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
            entity.ToTable("FondLog");

            entity.Property(e => e.Id).HasColumnName("ID");
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
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.FondNames)
                .HasForeignKey(d => d.Fond)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FondNames_Fond");
        });

        modelBuilder.Entity<FondType>(entity =>
        {
            entity.ToTable("FondType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<FondView>(entity =>
        {
            entity.ToTable("FondView");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<HistoricalPeriod>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<IncomeSource>(entity =>
        {
            entity.ToTable("IncomeSource");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Annotation).HasMaxLength(350);
            entity.Property(e => e.Literal).HasMaxLength(5);
            entity.Property(e => e.MovementNote).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.Number).HasMaxLength(50);
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
            entity.ToTable("InventoryLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.ActivityNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.Activity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLog_UnitActivity");

            entity.HasOne(d => d.InventoryNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.Inventory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLog_Inventory");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.InventoryLogs)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLog_User");
        });

        modelBuilder.Entity<InventoryType>(entity =>
        {
            entity.ToTable("InventoryType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.ToTable("Movement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MovementType>(entity =>
        {
            entity.ToTable("MovementType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ownership>(entity =>
        {
            entity.ToTable("Ownership");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ReceiptReason>(entity =>
        {
            entity.ToTable("ReceiptReason");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Reproduction>(entity =>
        {
            entity.ToTable("Reproduction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SecretChar>(entity =>
        {
            entity.ToTable("SecretChar");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<StorageTime>(entity =>
        {
            entity.ToTable("StorageTime");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<StorageUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StorageUnit1");

            entity.ToTable("StorageUnit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccessRestrictionNote).HasMaxLength(50);
            entity.Property(e => e.Annotation).HasMaxLength(350);
            entity.Property(e => e.Date).HasMaxLength(50);
            entity.Property(e => e.Index).HasMaxLength(15);
            entity.Property(e => e.IsFm).HasColumnName("IsFM");
            entity.Property(e => e.IsSf).HasColumnName("IsSF");
            entity.Property(e => e.Literal).HasMaxLength(2);
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.Tite).HasMaxLength(50);

            entity.HasOne(d => d.AcessNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Acess)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnit_Acess");

            entity.HasOne(d => d.CarrierNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Carrier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnit1_Carrier");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnit_Category1");

            entity.HasOne(d => d.InventoryNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.Inventory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StorageUnit_Inventory");

            entity.HasOne(d => d.SecretCharNavigation).WithMany(p => p.StorageUnits)
                .HasForeignKey(d => d.SecretChar)
                .HasConstraintName("FK_StorageUnit_SecretChar");

            entity.HasMany(d => d.Features).WithMany(p => p.Units)
                .UsingEntity<Dictionary<string, object>>(
                    "StorageUnitFeature",
                    r => r.HasOne<Feature>().WithMany()
                        .HasForeignKey("Feature")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StorageUnitFeatures_Feature"),
                    l => l.HasOne<StorageUnit>().WithMany()
                        .HasForeignKey("Unit")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StorageUnitFeatures_StorageUnit"),
                    j =>
                    {
                        j.HasKey("Unit", "Feature");
                        j.ToTable("StorageUnitFeatures");
                    });
        });

        modelBuilder.Entity<UndocumentPeriod>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocumentLocation).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Note).HasMaxLength(150);
            entity.Property(e => e.Reason).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.FondNavigation).WithMany(p => p.UndocumentPeriods)
                .HasForeignKey(d => d.Fond)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UndocumentPeriods_Fond");
        });

        modelBuilder.Entity<UnitCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category_1");

            entity.ToTable("UnitCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<UnitCompletedWork>(entity =>
        {
            entity.ToTable("UnitCompletedWork");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Note).HasMaxLength(150);

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.UnitCompletedWorks)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitCompletedWork_StorageUnit");

            entity.HasOne(d => d.WorkNavigation).WithMany(p => p.UnitCompletedWorks)
                .HasForeignKey(d => d.Work)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitCompletedWork_Work");
        });

        modelBuilder.Entity<UnitCondition>(entity =>
        {
            entity.ToTable("UnitCondition");

            entity.Property(e => e.Note).HasMaxLength(150);

            entity.HasOne(d => d.ConditionNavigation).WithMany(p => p.UnitConditions)
                .HasForeignKey(d => d.Condition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitCondition_Condition");

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.UnitConditions)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitCondition_StorageUnit");
        });

        modelBuilder.Entity<UnitLog>(entity =>
        {
            entity.ToTable("UnitLog");

            entity.Property(e => e.Id).HasColumnName("ID");
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

        modelBuilder.Entity<UnitRequiredWork>(entity =>
        {
            entity.ToTable("UnitRequiredWork");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CheckDate).HasColumnType("date");
            entity.Property(e => e.Note).HasMaxLength(150);

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.UnitRequiredWorks)
                .HasForeignKey(d => d.Unit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitRequiredWork_StorageUnit");

            entity.HasOne(d => d.WorkNavigation).WithMany(p => p.UnitRequiredWorks)
                .HasForeignKey(d => d.Work)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitRequiredWork_Work");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
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

        modelBuilder.Entity<Work>(entity =>
        {
            entity.ToTable("Work");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
