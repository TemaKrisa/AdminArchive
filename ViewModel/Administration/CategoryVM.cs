using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel for managing categories in the database.
    /// </summary>
    public partial class CategoryVM : BaseViewModel
    {
        // Create an instance of the database context
        private ArchiveBdContext dc = new();

        // Create an observable collection of categories
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public CategoryVM()
        {
            Categories = new ObservableCollection<Category>(dc.Categories);
        }

        public ICommand AddCommand => new RelayCommand<object>(Add);

        private void Add(object? parameter)
        {
            // Cast the parameter to a Category object
            if (parameter is Category editedCategory)
            {
                if (dc.Categories.Contains(editedCategory)) dc.Categories.Update(editedCategory);
                else dc.Categories.Add(editedCategory);
                dc.SaveChanges();
            }
        }
    }
}
