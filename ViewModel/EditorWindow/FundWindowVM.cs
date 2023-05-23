using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        { get => typeEnabled; set { typeEnabled = value; OnPropertyChanged(); } }

        public ObservableCollection<MovementType> MovementTypesEmpty { get; set; }

        private ObservableCollection<Fond> itemList = new();
        public ObservableCollection<Fond> ItemList { get => itemList; set { itemList = value; OnPropertyChanged(); } }


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

        private ObservableCollection<FondLog> _Log;
        public ObservableCollection<FondLog> Log { get => _Log; set { _Log = value; OnPropertyChanged(); } }

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
                if (SelectedItem.Number != null)
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
            FondNames = new ObservableCollection<FondName>(dc.FondNames.Where(u => u.Fond == SelectedItem.Id));
            UndocumentPeriods = new ObservableCollection<UndocumentPeriod>(dc.UndocumentPeriods.Where(u => u.Fond == SelectedItem.Id));
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
                if (SelectedItem.Movement == 1 && SelectedItem.MovementType == null) { ShowMessage("При выборе движения выбыл, также должен быть выбран тип движения!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.Name)) { ShowMessage("Введите наименование фонда!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.ShortName)) { ShowMessage("Введите сокращенное наименование фонда!"); }
                else if (string.IsNullOrWhiteSpace(SelectedItem.ReceiptDate.ToString())) { ShowMessage("Введите дату поступления!"); }
                else
                {

                    if (!dc.Fonds.Contains(SelectedItem))
                    {
                        if (dc.Fonds.Any(u => u.Number == SelectedItem.Number && u.Literal == SelectedItem.Literal && u.Index == SelectedItem.Index))
                        {
                            ShowMessage("Добавление фонда", "Фонд с таким номером уже существует");
                            return;
                        }
                        dc.Fonds.Add(SelectedItem);
                        dc.SaveChanges();
                        Log = new() { Activity = 1, Date = DateTime.Now, Fond = SelectedItem.Id, User = 1 };
                    }
                    else
                    {
                        dc.Update(SelectedItem);
                        dc.SaveChanges();
                        Log = new() { Activity = 2, Date = DateTime.Now, Fond = SelectedItem.Id, User = 1 };
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

                        if (dc.FondNames.Any(u => u.Id == fn.Id)) //Проверка на существование в базе данных
                        {
                            dc.FondNames.Update(fn); //Изменение в базе данных
                        }
                        else
                        {
                            fn.Fond = SelectedItem.Id;
                            dc.FondNames.Add(fn); //Добавление изменений в базу данных 
                        }
                    }
                    foreach (var fnd in FondNamesDelete)
                        if (dc.FondNames.Any(u => u.Id == fnd.Id)) dc.Remove(fnd); //Удаление изменений в базе данных
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
                        if (dc.UndocumentPeriods.Any(u => u.Id == up.Id))
                        {
                            dc.UndocumentPeriods.Update(up);
                        }
                        else
                        {
                            up.Fond = SelectedItem.Id;
                            dc.UndocumentPeriods.Add(up);
                        }
                    }
                    foreach (var upd in UndocumentPeriodsDelete)
                    {
                        if (dc.UndocumentPeriods.Any(u => u.Id == upd.Id))
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
            Log = new ObservableCollection<FondLog>(dc.FondLogs.Where(u => u.Fond == SelectedItem.Id).Include(w => w.UserNavigation).Include(b => b.ActivityNavigation));
        }

        protected override void CloseLog() { UCVisibility = Visibility.Collapsed; }
        #endregion

        #region Переименования фондов

        private ObservableCollection<FondName> fondNames, fondNamesDelete;

        public ObservableCollection<FondName> FondNames
        {
            get => fondNames;
            set { fondNames = value; OnPropertyChanged(); }
        }

        public ObservableCollection<FondName> FondNamesDelete
        {
            get => fondNamesDelete;
            set { fondNamesDelete = value; OnPropertyChanged(); }
        }

        public ICommand RemoveName => new RelayCommand(RemoveNameCommand);
        public ICommand SaveName => new RelayCommand(SaveRenameCommand);
        public ICommand EditRename => new RelayCommand(EditRenameCommand);
        public ICommand CreateRename => new RelayCommand(CreateRenameCommand);
        public ICommand CloseRename => new RelayCommand(CloseRenameCommand);

        private Visibility _renameVisibility = Visibility.Collapsed;
        public Visibility RenameVisibility
        {
            get => _renameVisibility;
            set { _renameVisibility = value; OnPropertyChanged(); }
        }

        private void RemoveNameCommand()
        {
            if (SelectedName != null)
            {
                FondNamesDelete.Add(SelectedName);
                FondNames.Remove(SelectedName);
                CloseRenameCommand();
            }
        }        
        private void CreateRenameCommand()
        {
            SelectedName = new FondName();
            EditRenameCommand();
        }
        
        private void SaveRenameCommand()
        {
            if (SelectedName != null)
            {
                if (SelectedName.StartDate > SelectedName.EndDate) ShowMessage("Начальная дата превышает конечную!");
                FondNames.Add(SelectedName);
                CloseRenameCommand();
            }
        }

        private void EditRenameCommand() => RenameVisibility = Visibility.Visible;
        private void CloseRenameCommand() => RenameVisibility = Visibility.Collapsed;
        #endregion

        #region Незадокументированные периоды

        private ObservableCollection<UndocumentPeriod> undocPeriodDelete, _undocumentPeriods;

        public ObservableCollection<UndocumentPeriod> UndocumentPeriodsDelete
        { get => undocPeriodDelete; set { undocPeriodDelete = value; OnPropertyChanged(); } }
        public ObservableCollection<UndocumentPeriod> UndocumentPeriods
        { get => _undocumentPeriods; set { _undocumentPeriods = value; OnPropertyChanged(); } }

        public ICommand RemovePeriod => new RelayCommand(RemovePeriodCommand);
        public ICommand SavePeriod => new RelayCommand(SavePeriodCommand);
        public ICommand EditPeriod => new RelayCommand(EditPeriodCommand);
        public ICommand CreatePeriod => new RelayCommand(CreatePeriodCommand);
        public ICommand ClosePeriod => new RelayCommand(ClosePeriodCommand);

        private Visibility _periodVisibility = Visibility.Collapsed;
        public Visibility PeriodVisibility
        { get => _periodVisibility; set { _periodVisibility = value; OnPropertyChanged(); } }

        private void RemovePeriodCommand()
        {
            if (SelectedPeriod != null)
            {
                UndocumentPeriodsDelete.Add(SelectedPeriod);
                UndocumentPeriods.Remove(SelectedPeriod);
                ClosePeriodCommand();
            }
        }
        private void CreatePeriodCommand()
        {
            SelectedPeriod = new UndocumentPeriod();
            EditPeriodCommand();
        }

        private void SavePeriodCommand()
        {
            if (SelectedPeriod != null)
            {
                if (SelectedPeriod.StartDate > SelectedPeriod.EndDate) ShowMessage("Начальная дата превышает конечную!");
                FondNames.Add(SelectedName);
                ClosePeriodCommand();
            }
        }

        private void EditPeriodCommand() => PeriodVisibility = Visibility.Visible;
        private void ClosePeriodCommand() => PeriodVisibility = Visibility.Collapsed;
        #endregion
    }
}
