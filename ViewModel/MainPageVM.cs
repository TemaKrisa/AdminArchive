using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
partial class MainPageVM : BaseViewModel
{
    private bool isAdmin, isWorker, isHead;
    private bool accounts, bd, reports, administrating, search, settings;
    public bool IsAdmin { get => isAdmin; set { isAdmin = value; OnPropertyChanged(); } }
    public bool IsWorker { get => isWorker; set { isWorker = value; OnPropertyChanged(); } }
    public bool IsHead { get => isHead; set { isHead = value; OnPropertyChanged(); } }

    public bool Accounts { get => accounts; set { accounts = value; OnPropertyChanged(); } }
    public bool BD { get => bd; set { bd = value; OnPropertyChanged(); } }
    public bool Reports { get => reports; set { reports = value; OnPropertyChanged(); } }
    public bool Administrating { get => administrating; set { administrating = value; OnPropertyChanged(); } }
    public bool Search { get => search; set { search = value; OnPropertyChanged(); } }
    public bool Settings { get => settings; set { settings = value; OnPropertyChanged(); } }

    public ObservableCollection<object> NavigationItems = new ObservableCollection<object>();
    public MainPageVM()
    {
        switch (AdminArchive.Classes.Setting.Usertype)
        {
            case 2: IsAdmin = true; IsWorker = false; IsHead = false; break;
            case 3: IsAdmin = false; IsWorker = true; IsHead = false; break;
            case 1: IsAdmin = false; IsWorker = true; IsHead = true; break;
        }
    }

    public void Set(string type)
    {
        Accounts = false; BD = false; Reports = false; Administrating = false;
        Search = false; Settings = false;
        switch (type)
        {
            case "Accounts": Accounts = true; break;
            case "BD": BD = true; break;
            case "Reports": Reports = true; break;
            case "Administrating": Administrating = true; break;
            case "Search": Search = true; break;
            case "Settings": Settings = true; break;
        }
    }
}
