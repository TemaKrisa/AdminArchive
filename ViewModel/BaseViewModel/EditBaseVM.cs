using AdminArchive.Classes;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdminArchive.ViewModel
{
    internal abstract class EditBaseVM : BaseViewModel
    {
        private RelayCommand _add, _open, _save;
        protected abstract void AddItem();
        protected abstract void SaveItem();
        protected abstract void OpenItem();
        protected abstract void FillCollections();
        public RelayCommand Add { get { return _add ?? (_add = new RelayCommand(AddItem)); } }
        public RelayCommand Save { get { return _save ?? (_save = new RelayCommand(SaveItem)); } }
        public RelayCommand Open { get { return _open ?? (_open = new RelayCommand(OpenItem)); } }
    }
}
