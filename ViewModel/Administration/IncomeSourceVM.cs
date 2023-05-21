using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// ViewModel for managing income sources in the database.
    /// </summary>
    public partial class IncomeSourceVM : BaseViewModel
    {
        // Create an instance of the database context
        private ArchiveBdContext dc = new();

        // Create an observable collection of categories
        private ObservableCollection<IncomeSource> _dataList;

        public ObservableCollection<IncomeSource> DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        public IncomeSourceVM()
        {
            DataList = new ObservableCollection<IncomeSource>(dc.IncomeSources);
        }

        public ICommand AddCommand => new RelayCommand<object>(Add);

        private void Add(object? parameter)
        {
            // Cast the parameter to a Category object
            if (parameter is IncomeSource edited)
            {
                if (dc.IncomeSources.Contains(edited)) dc.IncomeSources.Update(edited);
                else dc.IncomeSources.Add(edited);
                dc.SaveChanges();
            }
        }
    }
}
