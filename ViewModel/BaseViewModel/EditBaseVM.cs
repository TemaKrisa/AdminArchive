using AdminArchive.Classes;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
namespace AdminArchive.ViewModel;
/// <summary> ViewModel для редактирования Фондов, Описей, Ед.Хранения и Документов </summary>
internal abstract class EditBaseVM : BaseViewModel
{
    public dynamic pageVM { get; set; } //ViewModel страницы, которая использует данный класс.
    // Команды для добавления, сохранения, закрытия, отображения журнала изменений и навигации по элементам
    private RelayCommand _add, _open, _save, _close, _next, _prev, _last, _first, _closeWindow;
    // Свойства, определяющие, является ли текущий элемент первым или последним в коллекции
    private bool _isFirst, _isLast;
    public int Index;
    public bool IsFirst { get => _isFirst; set { _isFirst = value; OnPropertyChanged(); } } //Свойство, опредющее, является ли текущий элемент первым в коллекции.
    public bool IsLast { get => _isLast; set { _isLast = value; OnPropertyChanged(); } } //Свойство, определяющее, является ли текущий элемент последним в коллекции.
    // Абстрактные методы, которые должны быть реализованы в производных классах
    protected abstract void AddItem(); protected abstract void SaveItem(); //Абстрактные методы для добавления и сохранения элементов.
    protected abstract void OpenLog(); protected abstract void CloseLog(); //Абстрактные методы для открытия и закрытия журнала изменений.
    protected abstract void GoNext(); protected abstract void GoPrev(); //Абстрактные методы для навигации к следующему и предыдущему элементу.
    protected abstract void GoLast(); protected abstract void GoFirst(); //Абстрактные методы для навигации к последнему и первому элементу.
    protected abstract void FillCollections(); //Абстрактный метод для заполнения коллекций.
    public ActionType Action; //Тип действия (добавление или изменение).
    public enum ActionType { Add, Change }; //Перечисление типов действий.
    protected void Closer() => pageVM.UpdateData(); //Метод для закрытия окна и обновления данных на странице.
    // Команды, определяемые через лямбда-выражения и вызывающие соответствующие методы
    public RelayCommand CloseUC { get { return _close ??= new RelayCommand(CloseLog); } } //Команда для закрытия журнала изменений.
    public RelayCommand CloseWindow { get { return _closeWindow ??= new RelayCommand(Closer); } } //Команда для закрытия окна.
    public RelayCommand Next { get { return _next ??= new RelayCommand(GoNext); } } //Команда для перехода к следующему элементу.
    public RelayCommand Prev { get { return _prev ??= new RelayCommand(GoPrev); } } //Команда для перехода к предыдущему элементу.
    public RelayCommand Last { get { return _last ??= new RelayCommand(GoLast); } } //Команда для перехода к последнему элементу.
    public RelayCommand First { get { return _first ??= new RelayCommand(GoFirst); } } //Команда для перехода к первому элементу.
    public RelayCommand Add { get { return _add ?? new RelayCommand(AddItem); } } //Команда для добавления элемента.
    public RelayCommand Save { get { return _save ??= new RelayCommand(SaveItem); } } //Команда дляения элемента.
    public RelayCommand ShowLog { get { return _open ??= new RelayCommand(OpenLog); } } //Команда для открытия журнала изменений.
    public void CheckNav() { IsFirst = false; IsLast = false; } //Метод для проверки навигации.
    public void UpdateAndAddItems<T>(DbSet<T> dbSet, ObservableCollection<T> items, ObservableCollection<T> itemsToDelete, Func<T, T> createNewItem) where T : class, IHasId
    {
        if (items.Count == 0) return; //Если коллекция пуста, то выходим из метода.
        var itemsToUpdate = items.Where(item => dbSet.Any(u => u.Id == item.Id)).ToList(); //Выбираем элементы, которые нужно обновить.
        var itemsToAdd = items.Where(item => !dbSet.Any(u => u.Id == item.Id)).ToList(); //Выбираем элементы, которые нужно добавить.
        foreach (var item in itemsToUpdate.Concat(itemsToAdd)) //Обновляем или добавляем элементы.
        {
            var trackedEntity = dbSet.Local.SingleOrDefault(e => e.Id == item.Id); //Находим отслеживаемый объект в контексте.
            if (trackedEntity != null) { dbSet.Remove(trackedEntity); } //Если объект найден, то удаляем его из контекста.
        }
        dbSet.UpdateRange(itemsToUpdate); //Обновляем элементы в контексте.
        foreach (var item in itemsToAdd) { dbSet.Add(createNewItem(item)); } //Добавляем элементы в контекст.
        dbSet.RemoveRange(itemsToDelete.Where(item => dbSet.Any(u => u.Id == item.Id))); //Удаляем элементы из контекста.
    }
}
