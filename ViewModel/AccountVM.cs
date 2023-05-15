using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel
{
    class AccountVM : BaseViewModel
    {
        private ArchiveBdContext dc;
        public ObservableCollection<User> Users { get; set; }
        public AccountVM()
        {
            dc = new ArchiveBdContext();
            Users = new ObservableCollection<User>(dc.Users.Include(u => u.RoleNavigation));
        }
    }
}
