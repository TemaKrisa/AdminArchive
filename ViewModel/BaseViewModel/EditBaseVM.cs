using AdminArchive.Classes;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AdminArchive.ViewModel;
/// <summary>
/// ViewModel для редактирования Фондов, Описей, Ед.Хранения и Документов
/// </summary>
internal abstract class EditBaseVM : BaseViewModel
{
    public dynamic pageVM { get; set; }
    // Команды для добавления, сохранения, закрытия, отображения журнала изменений и навигации по элементам
    private RelayCommand _add, _open, _save, _close, _next, _prev, _last, _first, _closeWindow;
    // Свойства, определяющие, является ли текущий элемент первым или последним в коллекции
    private bool _isFirst, _isLast;
    public int Index;
    public bool IsFirst { get => _isFirst; set { _isFirst = value; OnPropertyChanged(); } }
    public bool IsLast { get => _isLast; set { _isLast = value; OnPropertyChanged(); } }
    // Абстрактные методы, которые должны быть реализованы в производных классах
    protected abstract void AddItem(); protected abstract void SaveItem();
    protected abstract void OpenLog(); protected abstract void CloseLog();
    protected abstract void GoNext(); protected abstract void GoPrev();
    protected abstract void GoLast(); protected abstract void GoFirst();
    protected abstract void FillCollections();
    public ActionType Action;
    public enum ActionType { Add, Change };
    protected void Closer() => pageVM.UpdateData();
    // Команды, определяемые через лямбда-выражения и вызывающие соответствующие методы
    public RelayCommand CloseUC { get { return _close ??= new RelayCommand(CloseLog); } }
    public RelayCommand CloseWindow { get { return _closeWindow ??= new RelayCommand(Closer); } }
    public RelayCommand Next { get { return _next ??= new RelayCommand(GoNext); } }
    public RelayCommand Prev { get { return _prev ??= new RelayCommand(GoPrev); } }
    public RelayCommand Last { get { return _last ??= new RelayCommand(GoLast); } }
    public RelayCommand First { get { return _first ??= new RelayCommand(GoFirst); } }
    public RelayCommand Add { get { return _add ??= new RelayCommand(AddItem); } }
    public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } }
    public RelayCommand ShowLog { get { return _open ??= new RelayCommand(OpenLog); } }
    public void CheckNav() { IsFirst = false; IsLast = false; }
    public void UpdateAndAddItems<T>(DbSet<T> dbSet, ObservableCollection<T> items, ObservableCollection<T> itemsToDelete, Func<T, T> createNewItem) where T : class, IHasId
    {
        if (items.Count == 0) return;
        var itemsToUpdate = items.Where(item => dbSet.Any(u => u.Id == item.Id)).ToList();
        var itemsToAdd = items.Where(item => !dbSet.Any(u => u.Id == item.Id)).ToList();
        foreach (var item in itemsToUpdate.Concat(itemsToAdd))
        {
            var trackedEntity = dbSet.Local.SingleOrDefault(e => e.Id == item.Id);
            if (trackedEntity != null) { dbSet.Remove(trackedEntity); }
        }
        dbSet.UpdateRange(itemsToUpdate);
        foreach (var item in itemsToAdd) { dbSet.Add(createNewItem(item)); }
        dbSet.RemoveRange(itemsToDelete.Where(item => dbSet.Any(u => u.Id == item.Id)));
    }
}
