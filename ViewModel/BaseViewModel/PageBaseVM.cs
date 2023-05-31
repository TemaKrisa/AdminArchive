using CommunityToolkit.Mvvm.Input;
using System.Windows;
namespace AdminArchive.ViewModel;
/// <summary> Базовая ViewModel для страниц фондов, документов, описей, ед.хранения  </summary>
internal abstract class PageBaseVM : BaseViewModel
{
    // Команды для добавления, редактирования, открытия, возврата, поиска, сброса поиска, открытия и закрытия окна поиска
    private RelayCommand _add, _open, _edit, _back, _search, _reset, _openSearch, _closeSearch, _update;
    public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
    public RelayCommand Edit { get { return _edit ??= new RelayCommand(EditItem); } }
    public RelayCommand Open { get { return _open ??= new RelayCommand(OpenItem); } }
    public RelayCommand Back { get { return _back ??= new RelayCommand(GoBack); } }
    public RelayCommand Search { get { return _search ??= new RelayCommand(SearchCommand); } }
    public RelayCommand Reset { get { return _reset ??= new RelayCommand(ResetSearch); } }
    public RelayCommand OpenSearch { get { return _openSearch ??= new RelayCommand(OpenSearchCommand); } }
    public RelayCommand CloseSearch { get { return _closeSearch ??= new RelayCommand(CloseSearchCommand); } }
    public RelayCommand Update { get { return _update ??= new RelayCommand(UpdateData); } }
    // Абстрактные методы, которые должны быть реализованы в производных классах
    ///<summary>Функция, которая вызывается при нажатии на кнопку "Добавить"</summary>
    protected abstract void AddItem();
    public abstract void UpdateData();
    ///<summary> Сброс поиска </summary>
    protected abstract void ResetSearch();
    ///<summary> переместиться назад  </summary>
    protected abstract void GoBack();
    ///<summary>Функция, которая вызывается при нажатии на кнопку "Редактировать"</summary>
    protected abstract void EditItem();
    ///<summary>Функция, которая вызывается при щелчке на элементе, чтобы открыть его </summary>
    protected abstract void OpenItem();
    ///<summary>Функция, которая вызывается при нажатии на кнопку "Сбросить" и сбрасывает фильтры поиска</summary>
    protected void CloseSearchCommand() => UCVisibility = System.Windows.Visibility.Collapsed;
    ///<summary>Функция, которая вызывается при нажатии на кнопку "Поиск"</summary>
    protected void OpenSearchCommand() => UCVisibility = System.Windows.Visibility.Visible;
    ///<summary> функция, которая вызывается при нажатии на кнопку "Поиск" </summary>
    protected void SearchCommand() { UCVisibility = Visibility.Collapsed; UpdateData(); }
}
