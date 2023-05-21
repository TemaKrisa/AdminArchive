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
        #region Переменные
        public FundPageVM pageVM = new();

        private int currentIndex;

        public ObservableCollection<Movement> Movements { get; set; }
        
        private ObservableCollection<MovementType> movementTypes;

        public ObservableCollection<MovementType> MovementTypes
        {
            get => movementTypes;
            set { movementTypes = value; OnPropertyChanged(); }
        }

        private bool typeEnabled;

        public bool TypeEnabled
        {
            get => typeEnabled;
            set
            {
                typeEnabled = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MovementType> MovementTypesEmpty { get; set; }
        
        private ObservableCollection<UndocumentPeriod> _undocumentPeriods;
        public ObservableCollection<UndocumentPeriod> UndocumentPeriods
        {
            get => _undocumentPeriods;
            set
            {
                _undocumentPeriods = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Fond> itemList = new();
        public ObservableCollection<Fond> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }

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
        public ObservableCollection<ReceiptReason> ReceiptReasons { get; set; }

        private ObservableCollection<FondName> fondNames;

        public ObservableCollection<FondName> FondNames 
        {
            get => fondNames;
            set { fondNames = value; OnPropertyChanged(); }
        }        
        private ObservableCollection<FondName> fondNamesDelete;

        public ObservableCollection<FondName> FondNamesDelete 
        { 
            get => fondNamesDelete;
            set { fondNamesDelete = value; OnPropertyChanged(); }
        }        
        private ObservableCollection<UndocumentPeriod> undocPeriodDelete;

        public ObservableCollection<UndocumentPeriod> UndocumentPeriodsDelete 
        { 
            get => undocPeriodDelete; 
            set { undocPeriodDelete = value; OnPropertyChanged(); }
        }


        public ObservableCollection<FondLog> Log
        {
            get => _Log; 
            set { _Log = value; OnPropertyChanged(); }
        }

        public Fond _selectedItem = new();

        public Fond SelectedItem
        {
            get => _selectedItem; 
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set { isEnabled = value; OnPropertyChanged(); }
        }

        private FondName _selectedName;
        public FondName SelectedName
        {
            get => _selectedName;
            set { _selectedName = value; OnPropertyChanged(); }
        }        
        private UndocumentPeriod _selectedPeriod;
        public UndocumentPeriod SelectedPeriod
        {
            get => _selectedPeriod;
            set { _selectedPeriod = value; OnPropertyChanged(); }
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
        public ICommand RemovePeriod => new RelayCommand(RemovePeriodCommand);

        private void RemovePeriodCommand()
        {
            if (SelectedPeriod != null)
            {
                UndocumentPeriodsDelete.Add(SelectedPeriod);
                UndocumentPeriods.Remove(SelectedPeriod);
            }
        }

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

        protected override void GoLast()
        {
            SelectedItem = (ItemList.Count > 0) ? ItemList[ItemList.Count - 1] : null;
            currentIndex = ItemList.IndexOf(SelectedItem);
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

        #endregion

        #region Инициализация
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

        #endregion

        #region Заполнение коллекций
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
                MovementTypes = new ObservableCollection<MovementType>(dc.MovementTypes);
                MovementTypesEmpty = new ObservableCollection<MovementType>();
                ReceiptReasons = new ObservableCollection<ReceiptReason>(dc.ReceiptReasons);
                FillTables();
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

        private void FillTables() 
        {
            using ArchiveBdContext dc = new();
            FondNames = new ObservableCollection<FondName>(dc.FondNames.Where(u => u.Fond == SelectedItem.FondId));
            UndocumentPeriods = new ObservableCollection<UndocumentPeriod>(dc.UndocumentPeriods.Where(u => u.Fond == SelectedItem.FondId));
            FondNamesDelete = new ObservableCollection<FondName>();
            UndocumentPeriodsDelete = new ObservableCollection<UndocumentPeriod>();
        }
        #endregion


        protected override void AddItem() //Создание нового фонда с заполнением полей
        {
            SelectedItem = new Fond() { Acess = 1, Category = 4, View = 2, Movement = 2, SecretChar = 1, HistoricalPeriod = 2 };
            CheckNav();
        }

        protected override void SaveItem() //Сохранение фонда
        {
            try
            {
                FondLog Log;
                using ArchiveBdContext dc = new();
                if (SelectedItem.Movement == 2 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.FondName)) { ShowMessage("Введите наименование фонда!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.FondShortName)) { ShowMessage("Введите сокращенное наименование фонда!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.ReceiptDate.ToString())) { ShowMessage("Введите дату поступления!"); }
                else
                {

                    if (!dc.Fonds.Contains(SelectedItem))
                    {
                        if (dc.Fonds.Any(u => u.FondNumber == SelectedItem.FondNumber && u.FondLiteral == SelectedItem.FondLiteral && u.FondIndex == SelectedItem.FondIndex))
                        {
                            ShowMessage("Добавление фонда", "Фонд с таким номером уже существует");
                            return;
                        }
                        dc.Fonds.Add(SelectedItem);
                        dc.SaveChanges();
                        Log = new() { Activity = 1, Date = DateTime.Now, Fond = SelectedItem.FondId, User = 1 };
                    }
                    else
                    {
                        dc.Update(SelectedItem);
                        dc.SaveChanges();
                        Log = new() { Activity = 2, Date = DateTime.Now, Fond = SelectedItem.FondId, User = 1 };
                    }
                    dc.FondLogs.Add(Log);

                    //Сохранение переименований фондов
                    foreach (var fn in FondNames)
                    {
                        if (fn.StartDate > fn.EndDate)
                        {
                            ShowMessage("У переименования фонда начальная дата больше конечной!");
                            return;
                        }
                        if (string.IsNullOrEmpty(fn.Name))
                        {
                            ShowMessage("Отсутсвует наименование у переименования фонда!");
                            return;
                        }
                        
                        if (dc.FondNames.Any(u=>u.NamesId == fn.NamesId)) //Проверка на существование в базе данных
                        {
                            dc.FondNames.Update(fn); //Изменение в базе данных
                        }
                        else
                        {
                            fn.Fond = SelectedItem.FondId; 
                            dc.FondNames.Add(fn); //Добавление изменений в базу данных 
                        }
                    }
                    foreach (var fnd in FondNamesDelete)
                        if (dc.FondNames.Any(u => u.NamesId == fnd.NamesId)) dc.Remove(fnd); //Удаление изменений в базе данных
                    dc.SaveChanges();
                    //Сохранение незадокументированных периодов
                    foreach (var up in UndocumentPeriods)
                    {
                        if (up.StartDate > up.EndDate)
                        {
                            ShowMessage("У незадокументированного периода  начальная дата больше конечной!");
                        }

                        if (string.IsNullOrEmpty(up.Reason))
                        {
                            ShowMessage("Отсутсвует причина у незадокументированного периода!");
                            return;
                        }                        
                        if (string.IsNullOrEmpty(up.DocumentLocation))
                        {
                            ShowMessage("Отсутсвует местонахождение у незадокументированного периода!");
                            return;
                        }
                        if (dc.UndocumentPeriods.Any(u => u.PeriodId == up.PeriodId))
                        {
                            dc.UndocumentPeriods.Update(up);
                        }
                        else
                        {
                            up.Fond = SelectedItem.FondId;
                            dc.UndocumentPeriods.Add(up);
                        }
                    }
                    foreach (var upd in UndocumentPeriodsDelete)
                    {
                        if (dc.UndocumentPeriods.Any(u=> u.PeriodId == upd.PeriodId))
                            dc.Remove(upd);
                    }
                    dc.SaveChanges();
                    pageVM.UpdateData();
                }
            }
            catch (Exception ex) { ShowMessage(ex.ToString()); }
        }

        #region Протокол
        protected override void OpenLog() //Открытие протокола
        {
            using ArchiveBdContext dc = new();
            UCVisibility = Visibility.Visible;
            Log = new ObservableCollection<FondLog>(dc.FondLogs.Where(u => u.Fond == SelectedItem.FondId).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        }

        protected override void CloseLog() { UCVisibility = Visibility.Collapsed; }
        #endregion
    }
}
