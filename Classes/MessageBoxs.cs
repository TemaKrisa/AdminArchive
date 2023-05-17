using Wpf.Ui.Controls;

namespace AdminArchive.Classes
{
    static class MessageBoxs
    {
        public static void Show(string Title, string Content)
        {
            MessageBox d = new();
            d.Show(Title,Content);
        }

        public static void ShowDialog(string Title, string Content)
        {
            Wpf.Ui.Controls.Dialog d = new()
            {
                Content = Content,
                Title = Title
            };
            d.Show();
        }

    }
}
