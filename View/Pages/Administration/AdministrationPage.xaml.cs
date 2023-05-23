using AdminArchive.Classes;
using System.Windows.Controls;

namespace AdminArchive.View.Pages
{
    public partial class AdministrationPage : Page
    {
        public AdministrationPage()
        {
            InitializeComponent();
            Setting.adminFrame = AdminFrame;
        }
    }
}
