using AdminArchive.Classes;
using AdminArchive.ViewModel;

namespace AdminArchive.View.Pages;
public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
        Setting.mainFrame = RootFrame;
        DataContext = new MainPageVM();
    }
}
