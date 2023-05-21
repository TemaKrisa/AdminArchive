using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdminArchive.ViewModel
{
    internal class FundPageVM : PageBaseVM
    {
        private ObservableCollection<Fond> _fonds;
        public ObservableCollection<Fond> Fonds 
        { 
            get => _fonds; 
            set 
            {
                _fonds = value;
                OnPropertyChanged();
            } 
        } // Define an ObservableCollection of Fonds
        private ArchiveBdContext dc;        
        private Fond _selectedItem;
        public Fond SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        public FundPageVM()
        {
            UpdateData();
        }

        public void UpdateData() // Define function to retrieve data from the database and update the view
        {
            dc = new ArchiveBdContext();
            Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.FondIndex).ThenBy(u => u.FondNumber).ThenBy(u => u.FondLiteral));
        }


        protected override void OpenItem() // Define function that is called when a user clicks on an item to open it
        {
            if(SelectedItem!= null)
            {
                InventoryPageVM vm = new(SelectedItem);
                InventoryPage v = new() { DataContext = vm };
                FrameManager.mainFrame.Navigate(v);
            }
        }

        protected override void GoBack() { } // Define function that is called when a user clicks on the "Go Back" button

        protected override void AddItem() // Define function that is called when a user clicks on the "Add" button
        {
            FundWindowVM viewModel = new(this, Fonds);
            FundWindow newWindow = new() { DataContext = viewModel };
            newWindow.ShowDialog();
        }
    
        protected override void EditItem() // Define function that is called when a user clicks on the "Edit" button
        {
            int index = Fonds.IndexOf(SelectedItem);
            FundWindow newWindow = new();
            FundWindowVM viewModel = new((SelectedItem as Fond), this, index, Fonds);
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();
        }
    }
}
