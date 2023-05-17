using System.Collections.ObjectModel;
using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;

namespace AdminArchive.ViewModel
{
    class LogUCVM : BaseViewModel
    {
        private ArchiveBdContext dc;
        public ObservableCollection<object> Log { get; set; }

        public LogUCVM(Fond fond)
        {
            Log = new ObservableCollection<object> { dc.FondLogs.Where(u=> u.Fond == fond.FondId).Include(u => u.UserNavigation).Include(u=> u.ActivityNavigation) };
        }        
        public LogUCVM(Inventory inventory)
        {
            Log = new ObservableCollection<object> { dc.InventoryLogs.Where(u=> u.Inventory == inventory.InventoryId).Include(u => u.UserNavigation).Include(u => u.ActivityNavigation) };
        }         
        public LogUCVM(StorageUnit unit)
        {
            Log = new ObservableCollection<object> { dc.UnitLogs.Where(u=> u.Unit == unit.UnitId).Include(u=>u.UserNavigation).Include(u => u.ActivityNavigation) };
        }         
        public LogUCVM()
        {

        }        


        //public LogUCVM(Document document)
        //{
        //    Log = new ObservableCollection<object> { dc.document.Where(u=> u. == document.DocumentId) };
        //}
    }
}
