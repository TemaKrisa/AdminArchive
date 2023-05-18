using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    class FundWindowVM : EditBaseVM
    {
        public FundPageVM pageVM = new();

        private bool _isFirst, _isLast;

        private int currentIndex;

        public ObservableCollection<Movement> Movements { get; set; }

        public ObservableCollection<UndocumentPeriod> UndocumentPeriods { get; set; }

        private ObservableCollection<FondLog> _Log;

        public ObservableCollection<Acess> Acess { get; set; }

        public ObservableCollection<FondView> FondView { get; set; }

        public ObservableCollection<CharRestrict> CharRestrict { get; set; }

        public ObservableCollection<HistoricalPeriod> HistoricalPeriod { get; set; }

        public ObservableCollection<FondType> FondType { get; set; }

        public ObservableCollection<DocType> DocType { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<SecretChar> SecretChar { get; set; }

        public ObservableCollection<IncomeSource> IncomeSource { get; set; }

        public ObservableCollection<Ownership> Ownership { get; set; }

        public ObservableCollection<StorageTime> StorageTime { get; set; }

        private ObservableCollection<FondName> fondNames;

        public ObservableCollection<FondName> FondNames 
        {
            get { return fondNames; }
            set { fondNames = value; OnPropertyChanged(); }
        }        
        private ObservableCollection<FondName> fondNamesDelete;

        public ObservableCollection<FondName> FondNamesDelete 
        { 
            get { return fondNamesDelete;  }
            set { fondNamesDelete = value; OnPropertyChanged(); }
        }


        public ObservableCollection<FondLog> Log
        {
            get { return _Log; }
            set { _Log = value; OnPropertyChanged(); }
        }

        public Fond _selectedItem = new();

        public Fond SelectedItem
        {
            get => _selectedItem;
            set
            { _selectedItem = value; OnPropertyChanged(); }
        }

        public bool IsFirst
        {
            get => _isFirst;
            set { _isFirst = value; OnPropertyChanged(); }
        }

        public bool IsLast
        {
            get => _isLast;
            set { _isLast = value; OnPropertyChanged(); }
        }

        private FondName _selectedName;
        public FondName SelectedName
        {
            get => _selectedName;
            set
            { _selectedName = value; OnPropertyChanged(); }
        }

        public ICommand RemoveName => new RelayCommand(RemoveNameCommand);

        private void RemoveNameCommand()
        {
            if (SelectedName != null)
            {
                FondNamesDelete.Add(SelectedName);
                FondNames.Remove(SelectedName);
            }
        }


        private void CheckNav(int index)
        {
            IsFirst = index != 0;
            IsLast = index != ItemList.Count - 1;
        }

        private void CheckNav() { IsFirst = false; IsLast = false; }

        protected override void GoNext()
        {
            currentIndex++;
            SelectedItem = (currentIndex < ItemList.Count) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
        }

        protected override void GoPrev()
        {
            currentIndex--;
            SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
        }

        protected override void GoLast()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[ItemList.Count - 1] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
        }

        protected override void GoFirst()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
        }

        protected override void CloseLog() { UCVisibility = Visibility.Collapsed; }

        private ObservableCollection<Fond> itemList = new();
        public ObservableCollection<Fond> ItemList { get { return itemList; } set { itemList = value; OnPropertyChanged(); } }
        public FundWindowVM(Fond selFond, FundPageVM vm, int selIndex, ObservableCollection<Fond> items)
        {
            SelectedItem = selFond;
            pageVM = vm;
            currentIndex = selIndex;
            ItemList = items;
            FillCollections();
        }
        public FundWindowVM(FundPageVM vm, ObservableCollection<Fond> items)
        {
            ItemList = items;
            pageVM = vm;
            FillCollections();
        }
        public FundWindowVM() { }


        protected override void FillCollections()
        {
            try
            {
                using ArchiveBdContext dc = new();
                Acess = new ObservableCollection<Acess>(dc.Acesses);
                FondView = new ObservableCollection<FondView>(dc.FondViews);
                CharRestrict = new ObservableCollection<CharRestrict>(dc.CharRestricts);
                HistoricalPeriod = new ObservableCollection<HistoricalPeriod>(dc.HistoricalPeriods);
                FondType = new ObservableCollection<FondType>(dc.FondTypes);
                DocType = new ObservableCollection<DocType>(dc.DocTypes);
                Categories = new ObservableCollection<Category>(dc.Categories);
                SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
                IncomeSource = new ObservableCollection<IncomeSource>(dc.IncomeSources);
                Ownership = new ObservableCollection<Ownership>(dc.Ownerships);
                StorageTime = new ObservableCollection<StorageTime>(dc.StorageTimes);
                Movements = new ObservableCollection<Movement>(dc.Movements);
                UndocumentPeriods = new ObservableCollection<UndocumentPeriod>(dc.UndocumentPeriods.Where(u => u.Fond == SelectedItem.FondId));
                FondNames = new ObservableCollection<FondName>(dc.FondNames.Where(u => u.Fond == SelectedItem.FondId));
                FondNamesDelete = new ObservableCollection<FondName>();
                if (SelectedItem.FondNumber != null)
                {
                    CheckNav(currentIndex);
                }
                else
                {
                    AddItem();
                }
            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
            }
        }

        protected override void AddItem()
        {
            SelectedItem = new Fond() { Acess = 1, Category = 4, View = 2, Movement = 2, SecretChar = 1, HistoricalPeriod = 2 };
            CheckNav();
        }

        protected override void SaveItem()
        {
            try
            {
                SelectedItem.Category = 4;
                FondLog fondLog;
                using ArchiveBdContext dc = new();
                if (!dc.Fonds.Contains(SelectedItem))
                {
                    if (dc.Fonds.Any(u => u.FondNumber == SelectedItem.FondNumber && u.FondLiteral == SelectedItem.FondLiteral && u.FondIndex == SelectedItem.FondIndex))
                    {
                        ShowMessage("Добавление фонда", "Фонд с таким номером уже существует");
                        return;
                    }
                    dc.Fonds.Add(SelectedItem);
                    dc.SaveChanges();
                    fondLog = new() { Activity = 1, Date = DateTime.Now, Fond = SelectedItem.FondId, User = 1 };
                }
                else
                {
                    // detach the entity from the context
                    dc.Entry(SelectedItem).State = EntityState.Detached;
                    // update the entity outside of the context
                    using ArchiveBdContext dc2 = new();
                    dc2.Fonds.Update(SelectedItem);
                    dc2.SaveChanges();
                    // attach the updated entity to the original context
                    dc.Attach(SelectedItem);
                    fondLog = new() { Activity = 2, Date = DateTime.Now, Fond = SelectedItem.FondId, User = 1 };
                }
                dc.FondLogs.Add(fondLog);
                
                foreach(var item in FondNames)
                {
                    if (dc.FondNames.Any(u => u.NamesId == item.NamesId))
                    {
                        dc.FondNames.Update(item);
                    }
                    else
                    {
                        item.Fond = SelectedItem.FondId;
                        dc.FondNames.Add(item);
                    }
                }
                foreach(var it in FondNamesDelete)
                {
                    if (dc.FondNames.Any(u => u.NamesId == it.NamesId))
                        dc.Remove(it);
                }
                dc.SaveChanges();
                pageVM.UpdateData();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString());
            }
        }

        protected override void OpenItem() { }

        protected override void OpenLog() 
        {
            using ArchiveBdContext dc = new();
            UCVisibility = Visibility.Visible;
            Log = new ObservableCollection<FondLog>(dc.FondLogs.Where(u => u.Fond == SelectedItem.FondId).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        }
    }
}
