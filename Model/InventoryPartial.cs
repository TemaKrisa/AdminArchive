namespace AdminArchive.Model
{
    partial class Inventory
    {
        private ArchiveBdContext? dc;
        public int? StartDate
        { 
            get 
            { 
                dc = new();
                //return dc.StorageUnits.Where(u => u.Inventory == InventoryId).Max(u => u.StartDate);
                return 0;
            } 
        }

        public int? EndDate
        { 
            get  
            { 
                dc = new();
                //return dc.StorageUnits.Where(u => u.Inventory == InventoryId).Max(u => u.EndDate); 
                return 0;
            } 
        }
    }
}
