using AdminArchive.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace AdminArchive.ViewModel
{
    class StorageUnitWindowVM : EditBaseVM
    {
        #region Переменные
        public StorageUnitPageVM pageVM = new();
        private int currentIndex;
        public ObservableCollection<Acess> Acesses { get; set; }
        public ObservableCollection<SecretChar> SecretChars { get; set; }
        public ObservableCollection<Carrier> Carriers { get; set; }
        public ObservableCollection<UnitCategory> Categories { get; set; }
        public ObservableCollection<CharRestrict> CharRestricts { get; set; }
        private ObservableCollection<StorageUnit> itemList;
        public ObservableCollection<StorageUnit> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }
        private ObservableCollection<UnitLog> _Log;
        public ObservableCollection<UnitLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }
        public StorageUnit _selectedUnit = new();
        public StorageUnit SelectedItem { get => _selectedUnit; set { _selectedUnit = value; OnPropertyChanged(); } }
        #endregion
        #region Навигация
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
            FillTables();
        }

        protected override void GoPrev()
        {
            currentIndex--;
            SelectedItem = (currentIndex >= 0) ? ItemList[currentIndex] : SelectedItem;
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }

        protected override void GoFirst()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[0] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }

        protected override void GoLast()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[^1] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
            IsFirst = currentIndex != 0;
            IsLast = currentIndex != ItemList.Count - 1;
            FillTables();
        }
        #endregion
        #region Инициализация
        private Inventory curInv;
        public StorageUnitWindowVM(StorageUnit selUnit, StorageUnitPageVM vm, int selIndex, ObservableCollection<StorageUnit> items, Inventory inventory)
        {
            SelectedItem = selUnit;
            pageVM = vm;
            currentIndex = selIndex;
            curInv = inventory;
            ItemList = items;
            FillCollections();
        }
        public StorageUnitWindowVM(StorageUnitPageVM vm, ObservableCollection<StorageUnit> items, Inventory inventory)
        {
            ItemList = items;
            pageVM = vm;
            curInv = inventory;
            FillCollections();
            AddItem();
        }
        public StorageUnitWindowVM() { }
        #endregion


        private ObservableCollection<StorageUnit> _unitFeatures;
        public ObservableCollection<StorageUnit> UnitFeatures
        {
            get { return _unitFeatures; }
            set { _unitFeatures = value; OnPropertyChanged(); }
        }

        private void FillTables()
        {
            using ArchiveBdContext dc = new();
            UnitFeatures = new ObservableCollection<StorageUnit>(dc.StorageUnits.Include(u => u.Features).Where(u => u.Id == SelectedItem.Id));
        }
        protected override void FillCollections()
        {
            try
            {
                using ArchiveBdContext dc = new ArchiveBdContext();
                SecretChars = new ObservableCollection<SecretChar>(dc.SecretChars);
                Acesses = new ObservableCollection<Acess>(dc.Acesses);
                Carriers = new ObservableCollection<Carrier>(dc.Carriers);
                Categories = new ObservableCollection<UnitCategory>(dc.UnitCategories);
                CharRestricts = new ObservableCollection<CharRestrict>(dc.CharRestricts);
                Features = new ObservableCollection<Feature>(dc.Features);
            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
            }
        }
        protected override void AddItem()
        {
            SelectedItem = new StorageUnit()
            {
                DocType = (int)curInv.DocType,
                Carrier = (int)curInv.Carrier,
                Acess = (int)curInv.Acess,
                SecretChar = curInv.SecretChar,
                CharRestrict = curInv.CharRestrict,
                Inventory = curInv.Id
            };
        }

        protected override void SaveItem()
        {
            try
            {
                using ArchiveBdContext dc = new();
                if (!dc.StorageUnits.Any(u => u.Id == SelectedItem.Id))
                    dc.StorageUnits.Add(SelectedItem);
                dc.SaveChanges();
                pageVM.UpdateData();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString());
            }
        }
        protected override void OpenLog() //Открытие протокола
        {
            using ArchiveBdContext dc = new();
            UCVisibility = Visibility.Visible;
            Log = new ObservableCollection<UnitLog>(dc.UnitLogs.Where(u => u.Unit == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        }

        protected override void CloseLog() { UCVisibility = Visibility.Collapsed; }

        #region особенности
        private ObservableCollection<Feature> _features;
        public ObservableCollection<Feature> Features
        { get { return _features; } set { _features = value; OnPropertyChanged(); } }

        //private StorageUnitFeature _selectedFeature;  
        //public StorageUnitFeature SelectedFeature
        //{
        //    get { return _selectedFeature; }
        //    set { _selectedFeature = value; OnPropertyChanged(); }
        //}

        #endregion

    }
}
