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
    public partial class HistoricalPeriodVM : BaseViewModel
    {
        // Create an instance of the database context
        private ArchiveBdContext dc = new();

        // Create an observable collection of categories
        private ObservableCollection<HistoricalPeriod> _dataList;

        public ObservableCollection<HistoricalPeriod> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        public HistoricalPeriodVM()
        {
            DataList = new ObservableCollection<HistoricalPeriod>(dc.HistoricalPeriods);
        }

        public ICommand AddCommand => new RelayCommand<object>(Add);

        private void Add(object? parameter)
        {
            // Cast the parameter to a Category object
            if (parameter is HistoricalPeriod edited)
            {
                if (dc.HistoricalPeriods.Contains(edited)) dc.HistoricalPeriods.Update(edited);
                else dc.HistoricalPeriods.Add(edited);
                dc.SaveChanges();
            }
        }
    }
}
