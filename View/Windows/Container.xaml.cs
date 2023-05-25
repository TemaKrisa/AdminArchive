using AdminArchive.Classes;
using AdminArchive.ViewModel;

namespace AdminArchive.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для Container.xaml
    /// </summary>
    public partial class Container
    {
        public Container()
        {
            InitializeComponent();
            Setting.containerFrame = RootFrame;
            DataContext = new ContainerVM();
        }
    }
}
