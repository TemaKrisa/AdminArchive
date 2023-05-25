namespace AdminArchive.ViewModel;
partial class MainPageVM : BaseViewModel
{
    private bool isAdmin, isWorker, isHead;
    public bool IsAdmin
    { get { return isAdmin; } set { isAdmin = value; OnPropertyChanged(); } }
    public bool IsWorker
    { get { return isWorker; } set { isWorker = value; OnPropertyChanged(); } }
    public bool IsHead
    { get { return isHead; } set { isHead = value; OnPropertyChanged(); } }

    public MainPageVM()
    {
        switch (AdminArchive.Classes.Setting.Usertype)
        {
            case 2: IsAdmin = true; IsWorker = false; IsHead = false; break;
            case 3: IsAdmin = false; IsWorker = true; IsHead = false; break;
            case 1: IsAdmin = false; IsWorker = true; IsHead = true; break;
        }
    }
}
