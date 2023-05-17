using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace AdminArchive.ViewModel
{
    class FundWindowVM : EditBaseVM
    {
        public FundPageVM pageVM = new();

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
        public ObservableCollection<FondName> FondNames { get; set; }
        public ObservableCollection<Movement> Movements { get; set; }
        public ObservableCollection<UndocumentPeriod> UndocumentPeriods { get; set; }
        public ObservableCollection<FondLog> Log { get; set; }

        public Fond _selectedItem = new();
        public Fond SelectedItem
        {
            get => _selectedItem;
            set
            { _selectedItem = value; OnPropertyChanged(); }
        }

        private bool _isFirst, _isLast;
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

        private void CheckNav(int index)
        {
            IsFirst = index != 0;
            IsLast = index != fonds.Count - 1;
            GetNumber();
        }        
        private void CheckNav()
        {
            IsFirst = false;
            IsLast = false;
        }

        protected override void GoNext()
        {
            int currentIndex = fonds.IndexOf(SelectedItem);
            SelectedItem = (currentIndex < fonds.Count - 1) ? fonds[currentIndex + 1] : SelectedItem;
            CheckNav(currentIndex);
        }

        protected override void GoPrev()
        {
            int currentIndex = fonds.IndexOf(SelectedItem);
            SelectedItem = (currentIndex > 0) ? fonds[currentIndex - 1] : SelectedItem;
            CheckNav(currentIndex);
        }

        protected override void GoLast()
        {
            SelectedItem = (fonds.Count > 0) ? fonds[fonds.Count - 1] : null;
            CheckNav(fonds.IndexOf(SelectedItem));
        }

        protected override void GoFirst()
        {
            SelectedItem = (fonds.Count > 0) ? fonds[0] : null;
            CheckNav(fonds.IndexOf(SelectedItem));
        }

        protected override void CloseLog() 
        {
            LogVisibility = Visibility.Collapsed;
        }


        private Visibility _logVisibility = Visibility.Collapsed;
        public Visibility LogVisibility
        {
            get { return _logVisibility; }
            set { _logVisibility = value; OnPropertyChanged(); }
        }

        private string? _prefix,_number,_literal;

        public string? Prefix 
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                OnPropertyChanged();
            }
        }

        public string? Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }
        public string? Literal
        {
            get { return _literal; }
            set
            {
                _literal = value;
                OnPropertyChanged();
            }
        }

        private List<Fond> fonds = new();

        public FundWindowVM(Fond selFond)
        {
            SelectedItem = selFond;
            dc = new ArchiveBdContext();
            FillCollections();
        }       
        
        public FundWindowVM()
        {
            dc = new ArchiveBdContext();
            FillCollections();
        }


        protected override void FillCollections()
        {
            try
            {
                fonds = dc.Fonds.OrderBy(u => u.FondNumber).AsNoTracking().ToList();
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
                Movements = new ObservableCollection<Movement>(dc.Movements);
                UndocumentPeriods = new ObservableCollection<UndocumentPeriod>(dc.UndocumentPeriods.Where(u => u.Fond == SelectedItem.FondId));
                FondNames = new ObservableCollection<FondName>(dc.FondNames.Where(u => u.Fond == SelectedItem.FondId));


                if (SelectedItem.FondNumber != null)
                {
                    GetNumber();
                    var currentIndex = fonds.IndexOf(SelectedItem);
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

        private void GetNumber()
        {
            if (char.IsLetter(SelectedItem.FondNumber[0])) Prefix = SelectedItem.FondNumber.Substring(0, 1);
            else Prefix = "";
            Number = Regex.Replace(SelectedItem.FondNumber, "[^0-9]", "");
            Literal = Regex.Match(SelectedItem.FondNumber, @"[А-Яа-я]{1,2}$").Value;
        }
        protected override void AddItem()
        {
            Number = Convert.ToString(Convert.ToInt32(Number) + 1);
            SelectedItem = new Fond() { Acess = 1, Category = 4, View = 2, Movement = 2 };
            Prefix = null;
            Literal = null;
            CheckNav();
        }

        protected override void SaveItem()
        {
            try
            {
                SelectedItem.FondNumber = Prefix + Number + Literal;
                if (!dc.Fonds.Contains(SelectedItem)) dc.Fonds.Add(SelectedItem);
                else dc.Fonds.Update(SelectedItem);
                dc.SaveChanges();
                pageVM.UpdateData();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OpenItem() { }
        protected override void OpenLog() 
        {
            LogVisibility = Visibility.Visible;
            Log = new ObservableCollection<FondLog>(dc.FondLogs.Where(u => u.Fond == SelectedItem.FondId).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        }
    }
}
