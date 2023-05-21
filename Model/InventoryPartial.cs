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
                int? q = dc.StorageUnits
                    .Where(u => u.Inventory == InventoryId)
                    .Select(u => (int?)u.StartDate)
                    .DefaultIfEmpty()
                    .Max(); if (q != null) return q;
                else return 0;
            } 
        }

        public int? EndDate
        { 
            get  
            { 
                dc = new();
                int? q = dc.StorageUnits
                    .Where(u => u.Inventory == InventoryId)
                    .Select(u => (int?)u.EndDate)
                    .DefaultIfEmpty()
                    .Max(); if (q != null) return q;
                else return 0;
            } 
        }

        public string FullNumber
        {
            get
            {
                return InventoryNumber + "" + InventoryLiteral;
            }
        }
    }
}
