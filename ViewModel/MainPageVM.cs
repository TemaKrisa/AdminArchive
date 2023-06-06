namespace AdminArchive.ViewModel;
/// <summary> ViewModel главной страницы приложения </summary>
partial class MainPageVM : BaseViewModel
{
    // Поля для определения типа пользователя
    private bool isAdmin, isWorker, isHead;
    // Свойства для определения типа пользователя
    public bool IsAdmin { get => isAdmin; set { isAdmin = value; OnPropertyChanged(); } }
    public bool IsWorker { get => isWorker; set { isWorker = value; OnPropertyChanged(); } }
    public bool IsHead { get => isHead; set { isHead = value; OnPropertyChanged(); } }
    public MainPageVM()
    {
        // Определение типа пользователя и отображение соответствующих разделов навигации
        switch (AdminArchive.Classes.Setting.Usertype)
        {
            case 2: IsAdmin = true; IsWorker = false; IsHead = false; break;
            case 3: IsAdmin = false; IsWorker = true; IsHead = false; break;
            case 1: IsAdmin = false; IsWorker = true; IsHead = true; break;
        }
    }
}
