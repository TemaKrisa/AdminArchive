using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class StorageUnit
{
    public int Id { get; set; }

    public int? Vol { get; set; }

    public int Volume { get; set; }

    public string Title { get; set; } = null!;

    public int DocType { get; set; }

    public int Carrier { get; set; }

    public int Acess { get; set; }

    public string? AccessRestrictionNote { get; set; }

    public int Inventory { get; set; }

    public int Number { get; set; }

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

    public int? Category { get; set; }

    public int? SecretChar { get; set; }

    public int? CharRestrict { get; set; }

    public string? Literal { get; set; }

    public string? Index { get; set; }

    public string? Annotation { get; set; }

    public virtual Acess AcessNavigation { get; set; } = null!;

    public virtual Carrier CarrierNavigation { get; set; } = null!;

    public virtual UnitCategory? CategoryNavigation { get; set; }

    public virtual CharRestrict? CharRestrictNavigation { get; set; }

    public virtual DocType DocTypeNavigation { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Inventory InventoryNavigation { get; set; } = null!;

    public virtual SecretChar? SecretCharNavigation { get; set; }

    public virtual ICollection<UnitCompletedWork> UnitCompletedWorks { get; set; } = new List<UnitCompletedWork>();

    public virtual ICollection<UnitCondition> UnitConditions { get; set; } = new List<UnitCondition>();

    public virtual ICollection<UnitLog> UnitLogs { get; set; } = new List<UnitLog>();

    public virtual ICollection<UnitRequiredWork> UnitRequiredWorks { get; set; } = new List<UnitRequiredWork>();

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();
}
