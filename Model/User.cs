using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Midname { get; set; } = null!;

    public int Role { get; set; }

    public virtual ICollection<FondLog> FondLogs { get; set; } = new List<FondLog>();

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<UnitLog> UnitLogs { get; set; } = new List<UnitLog>();
}
