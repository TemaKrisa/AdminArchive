using CommunityToolkit.Mvvm.Input;

namespace AdminArchive.ViewModel
{
    /// <summary>
    /// Базовая ViewModel для страниц фондов, документов, описей, ед.хранения 
    /// </summary>
    internal abstract class PageBaseVM : BaseViewModel 
    {
        // Команды для добавления, редактирования, открытия, возврата, поиска, сброса поиска, открытия и закрытия окна поиска
        private RelayCommand _add, _open, _edit,_back,_search,_reset,_openSearch, _closeSearch;
        public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
        public RelayCommand Edit { get { return _edit ??= new RelayCommand(EditItem); } }
        public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }
        public RelayCommand Back { get { return _back ??= new RelayCommand(GoBack); } }
        public RelayCommand Search { get { return _search ??= new RelayCommand(SearchCommand); } }
        public RelayCommand Reset { get { return _reset ??= new RelayCommand(ResetSearch); } }
        public RelayCommand OpenSearch { get { return _openSearch ??= new RelayCommand(OpenSearchCommand); } }
        public RelayCommand CloseSearch { get { return _closeSearch ??= new RelayCommand(CloseSearchCommand); } }
        // Абстрактные методы, которые должны быть реализованы в производных классах
        protected abstract void AddItem();
        protected abstract void SearchCommand();
        protected abstract void ResetSearch();
        protected abstract void CloseSearchCommand();
        protected abstract void OpenSearchCommand();
        protected abstract void GoBack();
        protected abstract void EditItem();
        protected abstract void OpenItem();
    }
}
