using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
partial class MainPageVM : BaseViewModel
{
    private bool isAdmin, isWorker, isHead;
    public bool IsAdmin { get => isAdmin; set { isAdmin = value; OnPropertyChanged(); } }
    public bool IsWorker { get => isWorker; set { isWorker = value; OnPropertyChanged(); } }
    public bool IsHead { get => isHead; set { isHead = value; OnPropertyChanged(); } }

    public ObservableCollection<object> NavigationItems = new ObservableCollection<object>();
    public MainPageVM()
    {
        switch (AdminArchive.Classes.Setting.Usertype) //Отображение разделов навигации
        {
            case 2: IsAdmin = true; IsWorker = false; IsHead = false; break;
            case 3: IsAdmin = false; IsWorker = true; IsHead = false; break;
            case 1: IsAdmin = false; IsWorker = true; IsHead = true; break;
        }
    }
}
