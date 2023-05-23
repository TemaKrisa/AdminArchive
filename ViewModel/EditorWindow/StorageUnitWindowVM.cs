using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public StorageUnit SelectedItem { get => _selectedUnit; set { _selectedUnit = value; OnPropertyChanged(); }}
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

        public StorageUnitWindowVM(StorageUnit selUnit, StorageUnitPageVM vm, int selIndex, ObservableCollection<StorageUnit> items, Fond fond)
        {
            SelectedItem = selUnit;
            pageVM = vm;
            currentIndex = selIndex;
            ItemList = items;
            FillCollections();
        }
        public StorageUnitWindowVM(StorageUnitPageVM vm, ObservableCollection<StorageUnit> items,Fond fond)
        {
            ItemList = items;
            pageVM = vm;
            FillCollections();
        }
        public StorageUnitWindowVM() { }
        #endregion

        private void FillTables()
        {

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
            }
            catch (Exception e)
            {
                ShowMessage(e.Message);
            }
        }
        protected override void AddItem()
        {
            SelectedItem = new StorageUnit();
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

    }
}
