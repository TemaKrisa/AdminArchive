using AdminArchive.Classes;
using AdminArchive.Model;
using AdminArchive.View.Pages;
using AdminArchive.View.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using FontFamily = System.Windows.Media.FontFamily;

namespace AdminArchive.ViewModel
{
    internal class FundPageVM : PageBaseVM
    {
        public ObservableCollection<Fond> Fonds { get; set; }
        private ArchiveBdContext dc;
        public ICommand ReportAdd { get; private set; }        
        
        private Fond _selectedItem;
        public Fond SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        public FundPageVM()
        {
            dc = new ArchiveBdContext();
            Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation));
            ReportAdd = new RelayCommand(AddReport);
        }

        public void UpdateData()
        {
            Fonds = new ObservableCollection<Fond>(dc.Fonds.Include(u => u.CategoryNavigation));
        }

        protected override void EditItem()
        {
            FundWindow _fundEditor = new();
            FundWindowVM fundEditorVM = _fundEditor.DataContext as FundWindowVM;
            fundEditorVM.SelectedFond = (SelectedItem as Fond);
            fundEditorVM.pageVM = this;
            _fundEditor.Show();
        }

        protected override void OpenItem()
        {
            if(SelectedItem!= null)
            {
                InventoryPageVM vm = new(SelectedItem);
                InventoryPage v = new() { DataContext = vm };
                FrameManager.mainFrame.Navigate(v);
            }
        }


        protected override void AddItem()
        {
            var viewModel = new FundWindowVM();
            var newWindow = new FundWindow
            {
                DataContext = viewModel
            };
            newWindow.ShowDialog();
        }        

        private void AddReport()
        {
            FlowDocument flowDoc = new();
            //Добавление заголовка
            Paragraph heading = new()
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
                FontFamily = new System.Windows.Media.FontFamily("Times New Roman"),
                FontSize = 23
            };
            heading.Inlines.Add(new Run("Перечень фондов"));
            flowDoc.Blocks.Add(heading);
            Paragraph emptyLine = new();
            flowDoc.Blocks.Add(emptyLine);
            
            //Добавление таблицы с датой формирования отчёта
            Table t1 = new();
            TableRowGroup tableRowGroup = new();

            // Установка заголовка таблицы
            TableRow hR = new() { FontWeight = FontWeights.Bold };
            t1.Columns.Add(new TableColumn() { Width = new GridLength(500)});
            t1.Columns.Add(new TableColumn());
            hR.Cells.Add(new TableCell(new Paragraph(new Run("Название критерия"))));
            hR.Cells.Add(new TableCell(new Paragraph(new Run("Дата"))));
            tableRowGroup.Rows.Add(hR);

            // Заполнение таблицы
            TableRow dr = new();
            dr.Cells.Add(new TableCell(new Paragraph(new Run("Перечень фондов в наличии"))));
            dr.Cells.Add(new TableCell(new Paragraph(new Run($"на {DateOnly.FromDateTime(DateTime.Now)}"))));
            tableRowGroup.Rows.Add(dr);

            // Обьединение заголовка таблицы с данными
            t1.RowGroups.Add(tableRowGroup);
            flowDoc.Blocks.Add(t1);
            flowDoc.Blocks.Add(emptyLine);
            //Добавление таблицы со списком фондов
            Table table = new Table 
                { BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
            for (int i = 0; i < 6; i++) { table.Columns.Add(new TableColumn()); }

            TableRow headerRow = new TableRow
            { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 };

            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("№ фонда"))) 
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Название фонда")))
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Кат.")))
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Дата нач.")))
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Дата кон.")))
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Объем ед. хр.")))
                { FontWeight = FontWeights.Bold, FontFamily = new FontFamily("Times New Roman"), FontSize = 22 });

            table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });
            // Заполненние таблицы
            TableRowGroup dataGroup = new();
            foreach (var item in dc.Fonds)
            {
                TableRow dataRow = new();
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.FondId.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.FondName))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.CategoryNavigation?.CategoryName))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.StartDate.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.EndDate.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Volume.ToString()))));
                dataGroup.Rows.Add(dataRow);
            }

            table.RowGroups.Add(dataGroup);
            
            //Добавление последней строки таблицы
            TableRow finalRow = new();
            finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Фондов всего: {dc.Fonds.Count()}"))) { ColumnSpan= 3, FontWeight = FontWeights.Bold });
            finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Fonds.Sum(u=>u.Volume)}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold});
            dataGroup.Rows.Add(finalRow);
            flowDoc.Blocks.Add(table);
            //Формирование документа
            string rtfText;
            TextRange tr = new(flowDoc.ContentStart, flowDoc.ContentEnd);
            using (MemoryStream ms = new())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }

            // Выбор расположение документа
            SaveFileDialog saveDialog = new()
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf"
            };
            if (saveDialog.ShowDialog() == true)
            {
                string fileName = saveDialog.FileName;
                File.WriteAllText(fileName, rtfText);
                // Открытие сохраненного документа
                Process.Start(new ProcessStartInfo()
                {
                    FileName = fileName,
                    UseShellExecute = true
                });
            }
        }
    }
}
