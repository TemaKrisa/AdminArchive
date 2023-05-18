using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AdminArchive.Classes
{
    /// <summary>
    /// A service that provides methods related to displaying the <see cref="IDialogControl"/>.
    /// </summary>
    public class DialogService : IDialogService
    {
        private IDialogControl? _dialogControl;

        /// <inheritdoc />
        public void SetDialogControl(IDialogControl dialog) => _dialogControl = dialog;

        /// <inheritdoc />
        public IDialogControl GetDialogControl()
        {
            if (_dialogControl is null)
                throw new InvalidOperationException(
                    $"The ${typeof(DialogService)} cannot be used unless previously defined with {typeof(IDialogControl)}.{nameof(SetDialogControl)}().");

            return _dialogControl;
        }
    }
}
