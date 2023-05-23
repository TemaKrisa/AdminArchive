using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using Table = System.Windows.Documents.Table;

namespace AdminArchive.ViewModel
{
    internal class ReportVM : BaseViewModel
    {
        private ArchiveBdContext dc;
        private FontFamily font = new("Times New Roman");

        public ICommand SaveRep => new RelayCommand(Report);

        private int _selectedType;
        public int SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        public ReportVM()
        {
            dc = new ArchiveBdContext();
        }

        private void Report()
        {
            switch (SelectedType) 
            {
                case 0: FondListReport(); break;
                case 1: InventoriesListReport(); break;
            }

        }
        private void InventoriesListReport()
        {
            var Fonds = dc.Fonds.ToList();
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(new Paragraph(new Run("Перечень Описей")) { TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 23 });
            flowDoc.Blocks.Add(new Paragraph());
            TableRowGroup tableRowGroup = new();
            TableRow hR = new()
            { FontWeight = FontWeights.Bold, Cells = { new TableCell(new Paragraph(new Run("Название критерия"))), new TableCell(new Paragraph(new Run("Дата"))) } };
            tableRowGroup.Rows.Add(hR);
            flowDoc.Blocks.Add(new Paragraph());
            foreach (var item in Fonds)
            {
                flowDoc.Blocks.Add(new Paragraph(new Run($"{"Фонд №" + item.FullNumber +" " + item.Name}"))
                { TextAlignment = TextAlignment.Left, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 20 });
                 TableRowGroup dataGroup = new();
                Table table = new() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
                for (int i = 0; i < 6; i++) { table.Columns.Add(new TableColumn()); }
                TableRow headerRow = new()
                {
                    FontWeight = FontWeights.Bold,
                    FontFamily = font,
                    FontSize = 22,
                    Cells = { new TableCell(new Paragraph(new Run("№ "))), new TableCell(new Paragraph(new Run("Название описи"))),
                    new TableCell(new Paragraph(new Run("Тип описи"))),new TableCell(new Paragraph(new Run("Дата нач."))),new TableCell(new Paragraph(new Run("Дата кон."))),
                    new TableCell(new Paragraph(new Run("Объем ед. хр.")))}
                };
                table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });

                foreach (var invent in dc.Inventories.Where(u=>u.Fond == item.Id).Include(u=>u.TypeNavigation))
                {
                        TableRow dataRow = new();
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.FullNumber))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.Name))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.TypeNavigation?.Name))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.StartDate.ToString()))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.EndDate.ToString()))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Volume.ToString()))));
                        dataGroup.Rows.Add(dataRow);
                }
                TableRow finalRow = new();
                finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Описей всего: {dc.Inventories.Where(u => u.Fond == item.Id).Count()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
                finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Inventories.Where(u => u.Fond == item.Id).Sum(u => u.Volume)}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
                dataGroup.Rows.Add(finalRow);
                table.RowGroups.Add(dataGroup); 
                flowDoc.Blocks.Add(table);
            }
            FormDoc(flowDoc);
        }

        private void FondListReport()
        {
            //Создание документа
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(new Paragraph(new Run("Перечень фондов"))
            { TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 23 });
            flowDoc.Blocks.Add(new Paragraph());
            // Установка заголовка таблицы
            TableRowGroup tableRowGroup = new();
            TableRow hR = new()
            { FontWeight = FontWeights.Bold, Cells = { new TableCell(new Paragraph(new Run("Название критерия"))), new TableCell(new Paragraph(new Run("Дата")))}};
            tableRowGroup.Rows.Add(hR);
            // Заполнение таблицы
            TableRow dr = new() { Cells = { new TableCell(new Paragraph(new Run("Перечень фондов"))),  new TableCell(new Paragraph(new Run($"на {DateTime.Now.Date:d}")))}};
            tableRowGroup.Rows.Add(dr);
            // Обьединение заголовка таблицы с данными
            Table t1 = new() { Columns = { new TableColumn { Width = new GridLength(500) }, new TableColumn()}, RowGroups = { tableRowGroup }};
            flowDoc.Blocks.Add(t1);
            //Добавление таблицы со списком фондов
            Table table = new() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
            for (int i = 0; i < 6; i++)
            {
                table.Columns.Add(new TableColumn());
            }
            TableRow headerRow = new() { FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 22,
                Cells = { new TableCell(new Paragraph(new Run("№ фонда"))), new TableCell(new Paragraph(new Run("Название фонда"))),
                    new TableCell(new Paragraph(new Run("Кат."))), new TableCell(new Paragraph(new Run("Дата нач."))), 
                    new TableCell(new Paragraph(new Run("Дата кон."))), new TableCell(new Paragraph(new Run("Объем ед. хр.")))}
            };
            table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });

            // Заполненние таблицы
            TableRowGroup dataGroup = new();
            foreach (var item in dc.Fonds.Include(u=>u.CategoryNavigation))
            {
                TableRow dataRow = new();
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.FullNumber))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.ShortName))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.CategoryNavigation?.Name))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.StartDate.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.EndDate.ToString()))));
                dataRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Volume.ToString()))));
                dataGroup.Rows.Add(dataRow);
            }
            table.RowGroups.Add(dataGroup);
            //Добавление последней строки таблицы
            TableRow finalRow = new();
            finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Фондов всего: {dc.Fonds.Count()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
            finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Fonds.Sum(u => u.Volume)}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
            dataGroup.Rows.Add(finalRow);
            flowDoc.Blocks.Add(table);
            FormDoc(flowDoc);
        }

        private static void FormDoc(FlowDocument flowDoc)
        {
            string rtfText;
            TextRange tr = new(flowDoc.ContentStart, flowDoc.ContentEnd);
            using (MemoryStream ms = new())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }
            // Выбор расположение документа
            SaveFileDialog saveDialog = new() { Filter = "Rich Text Format (*.rtf)|*.rtf" };
            if (saveDialog.ShowDialog() == true)
            {
                string fileName = saveDialog.FileName;
                File.WriteAllText(fileName, rtfText);
                // Открытие сохраненного документа
                Process.Start(new ProcessStartInfo() { FileName = fileName, UseShellExecute = true });
            }
        }
    }
}
