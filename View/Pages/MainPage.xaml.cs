using AdminArchive.Classes;
using AdminArchive.ViewModel;
using System.Windows.Controls;

namespace AdminArchive.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            FrameManager.mainFrame = RootFrame;
            DataContext = new MainPageVM();
        }
    }
}
