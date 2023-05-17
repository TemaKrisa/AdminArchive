using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel for managing categories in the database.
    /// </summary>
    public partial class CategoryVM : AdminBaseVM
    {
        private ArchiveBdContext dc = new();

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
            //Отображение списка
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
