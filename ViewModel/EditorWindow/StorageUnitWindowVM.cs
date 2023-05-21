using AdminArchive.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AdminArchive.ViewModel
{
    class StorageUnitWindowVM : EditBaseVM
    {
        public StorageUnitPageVM pageVM = new();

        private ArchiveBdContext dc;
        public ObservableCollection<Acess> Acess { get; set; }
        public ObservableCollection<FondView> FondView { get; set; }
        public ObservableCollection<CharRestrict> CharRestrict { get; set; }
        public ObservableCollection<HistoricalPeriod> HistoricalPeriod { get; set; }
        public ObservableCollection<FondType> FondType { get; set; }
        public ObservableCollection<DocType> DocType { get; set; }
        public ObservableCollection<Category> Category { get; set; }
        public ObservableCollection<SecretChar> SecretChar { get; set; }
        public ObservableCollection<IncomeSource> IncomeSource { get; set; }
        public ObservableCollection<Ownership> Ownership { get; set; }
        public ObservableCollection<StorageTime> StorageTime { get; set; }

        public StorageUnit _selectedUnit = new();

        public StorageUnit SelectedItem
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                OnPropertyChanged();
            }
        }

        public StorageUnitWindowVM()
        {
            dc = new ArchiveBdContext();
            FillCollections();
        }

        protected override void FillCollections()
        {
            try
            {
                Acess = new ObservableCollection<Acess>(dc.Acesses);
                FondView = new ObservableCollection<FondView>(dc.FondViews);
                CharRestrict = new ObservableCollection<CharRestrict>(dc.CharRestricts);
                HistoricalPeriod = new ObservableCollection<HistoricalPeriod>(dc.HistoricalPeriods);
                FondType = new ObservableCollection<FondType>(dc.FondTypes);
                DocType = new ObservableCollection<DocType>(dc.DocTypes);
                Category = new ObservableCollection<Category>(dc.Categories);
                SecretChar = new ObservableCollection<SecretChar>(dc.SecretChars);
                IncomeSource = new ObservableCollection<IncomeSource>(dc.IncomeSources);
                Ownership = new ObservableCollection<Ownership>(dc.Ownerships);
                StorageTime = new ObservableCollection<StorageTime>(dc.StorageTimes);
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
                if (!dc.StorageUnits.Any(u => u.UnitId == SelectedItem.UnitId))
                    dc.StorageUnits.Add(SelectedItem);
                dc.SaveChanges();
                pageVM.UpdateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        protected override void OpenLog() { }

        protected override void CloseLog()
        {

        }
        protected override void GoNext()
        {
        }
        protected override void GoPrev()
        {

        }
        protected override void GoLast()
        {

        }
        protected override void GoFirst()
        {

        }

    }
}
