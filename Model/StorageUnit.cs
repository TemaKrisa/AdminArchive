using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class StorageUnit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public int Vol { get; set; }

    public int Volume { get; set; }

    public string Tite { get; set; } = null!;

    public int? DocType { get; set; }

    public int? Carrier { get; set; }

    public int? Acess { get; set; }

    public string? AccessRestrictionNote { get; set; }

    public int Inventory { get; set; }

    public string UnitNumber { get; set; } = null!;

    public string Date { get; set; } = null!;

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public bool IsWanted { get; set; }

    public bool IsRetired { get; set; }

    public bool IsSf { get; set; }

    public bool IsFm { get; set; }

    public bool IsFault { get; set; }

    public bool IsRolled { get; set; }

    public bool IsPhotocopied { get; set; }

    public bool HasGemstones { get; set; }

    public bool IsMuseumObject { get; set; }

    public string? Note { get; set; }

    public int Category { get; set; }

    public virtual Acess? AcessNavigation { get; set; }

    public virtual Carrier? CarrierNavigation { get; set; }

    public virtual UnitCategory CategoryNavigation { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Inventory InventoryNavigation { get; set; } = null!;

    public virtual ICollection<UnitLog> UnitLogs { get; set; } = new List<UnitLog>();
}
