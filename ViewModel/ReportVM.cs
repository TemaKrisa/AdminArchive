using AdminArchive.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
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
        private string ReportTitle = "";
        private FontFamily font = new("Times New Roman");
        public ICommand SaveRep => new RelayCommand(Report);
        private int _selectedType;
        public int SelectedType { get { return _selectedType; } set { _selectedType = value; OnPropertyChanged(); } }
        private void Report()
        {
            switch (SelectedType)
            {
                case 0: FondListReport(); break;
                case 1: InventoriesListReport(); break;
                case 2: UnitsListReport(); break;
                case 3: UserActionsList(); break;
            }
        }
        private Table CreateHeaderAndDateTable(string headerText)
        {
            TableRowGroup tableRowGroup = new();
            TableRow hR = new() { FontWeight = FontWeights.Bold, Cells = { new TableCell(new Paragraph(new Run("Название критерия"))), new TableCell(new Paragraph()) } };
            tableRowGroup.Rows.Add(hR);
            TableRow dr = new() { Cells = { new TableCell(new Paragraph(new Run(headerText))), new TableCell(new Paragraph(new Run($"Дата на {DateTime.Now.Date:d}"))) } };
            tableRowGroup.Rows.Add(dr);
            Table table = new() { Columns = { new TableColumn { Width = new GridLength(430) }, new TableColumn() { Width = new GridLength(350) } }, RowGroups = { tableRowGroup } };
            return table;
        }
        private Paragraph CreateHeader(string headerText)
        { return new Paragraph(new Run(headerText)) { TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 20 }; }

        private void InventoriesListReport()
        {
            using ArchiveBdContext dc = new();
            ReportTitle = "Перечень описей";
            var Fonds = dc.Fonds.OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal).ToList();
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(CreateHeader(ReportTitle));
            flowDoc.Blocks.Add(new Paragraph());
            Table headerAndDateTable = CreateHeaderAndDateTable(ReportTitle);
            flowDoc.Blocks.Add(headerAndDateTable);
            flowDoc.Blocks.Add(new Paragraph());
            foreach (var item in Fonds)
            {
                flowDoc.Blocks.Add(new Paragraph(new Run($"{"Фонд №" + item.FullNumber + " " + item.Name}"))
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
                foreach (var invent in dc.Inventories.Where(u => u.Fond == item.Id).Include(u => u.TypeNavigation))
                {
                    TableRow dataRow = new();
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.FullNumber))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.Name))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.TypeNavigation?.Name))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.StartDate.ToString()))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.EndDate.ToString()))));
                    dataRow.Cells.Add(new TableCell(new Paragraph(new Run(invent.Volume.ToString()))));
                    dataGroup.Rows.Add(dataRow);
                }
                TableRow finalRow = new();
                finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Описей всего: {dc.Inventories.Where(u => u.Fond == item.Id).Count()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
                finalRow.Cells.Add(new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Fonds.Where(u => u.Id == item.Id).Select(u => u.Volume).First()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold });
                dataGroup.Rows.Add(finalRow);
                table.RowGroups.Add(dataGroup);
                flowDoc.Blocks.Add(table);
            }
            FormDoc(flowDoc);
        }
        private void UnitsListReport()
        {
            using ArchiveBdContext dc = new();
            var Fonds = dc.Fonds.OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal).ToList();
            ReportTitle = "Перечень единиц хранения";
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(CreateHeader(ReportTitle));
            flowDoc.Blocks.Add(new Paragraph());
            Table headerAndDateTable = CreateHeaderAndDateTable(ReportTitle);
            flowDoc.Blocks.Add(headerAndDateTable);
            flowDoc.Blocks.Add(new Paragraph());
            foreach (var fond in Fonds)
            {
                flowDoc.Blocks.Add(new Paragraph(new Run($"Фонд №{fond.FullNumber} {fond.Name}")) { TextAlignment = TextAlignment.Left, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 20 });
                var Inventories = dc.Inventories.Where(u => u.Fond == fond.Id).OrderBy(u => u.Number).ThenBy(u => u.Literal).ToList();
                foreach (var item in Inventories)
                {
                    flowDoc.Blocks.Add(new Paragraph(new Run($"Опись №{item.FullNumber} {item.Name}")) { TextAlignment = TextAlignment.Left, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 20 });
                    Table table = new() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
                    for (int i = 0; i < 7; i++) { table.Columns.Add(new TableColumn()); }
                    TableRow headerRow = new()
                    {
                        FontWeight = FontWeights.Bold,
                        FontFamily = font,
                        FontSize = 22,
                        Cells = { new TableCell(new Paragraph(new Run("№ "))), new TableCell(new Paragraph(new Run("Том"))),
                    new TableCell(new Paragraph(new Run("Заголовок"))), new TableCell(new Paragraph(new Run("Категория"))),
                    new TableCell(new Paragraph(new Run("Дата нач."))), new TableCell(new Paragraph(new Run("Дата кон."))),
                    new TableCell(new Paragraph(new Run("Объем ед. хр."))) }
                    };
                    table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });
                    TableRowGroup dataGroup = new();
                    foreach (var invent in dc.StorageUnits.Where(u => u.Inventory == item.Id).Include(u => u.CategoryNavigation).OrderBy(u => u.Number).ThenBy(u => u.Literal))
                    {
                        TableRow dataRow = new()
                        {
                            Cells = { new TableCell(new Paragraph(new Run(invent.FullNumber))), new TableCell(new Paragraph(new Run(invent.Vol.ToString()))),
                        new TableCell(new Paragraph(new Run(invent.Title))), new TableCell(new Paragraph(new Run(invent.CategoryNavigation?.Name))),
                        new TableCell(new Paragraph(new Run(invent.StartDate.ToString()))), new TableCell(new Paragraph(new Run(invent.EndDate.ToString()))),
                        new TableCell(new Paragraph(new Run(invent.Volume.ToString()))) }
                        };
                        dataGroup.Rows.Add(dataRow);
                    }
                    TableRow finalRow = new()
                    {
                        Cells = { new TableCell(new Paragraph(new Run($"Единиц хранения всего: {dc.StorageUnits.Where(u => u.Inventory == item.Id).Count()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold },
                    new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Inventories.Where(u => u.Id == item.Id).Select(u => u.Volume).First()}"))) { ColumnSpan = 4, FontWeight = FontWeights.Bold } }
                    };
                    dataGroup.Rows.Add(finalRow);
                    table.RowGroups.Add(dataGroup);
                    flowDoc.Blocks.Add(table);
                }
            }
            FormDoc(flowDoc);
        }
        private void UserActionsList()
        {
            using ArchiveBdContext dc = new();
            ReportTitle = "Отчёт о работе сотрудников";
            var Users = dc.Users.Where(u => u.Role != 2).ToList();
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(CreateHeader(ReportTitle));
            flowDoc.Blocks.Add(new Paragraph());
            Table headerAndDateTable = CreateHeaderAndDateTable(ReportTitle);
            flowDoc.Blocks.Add(headerAndDateTable);
            flowDoc.Blocks.Add(new Paragraph());
            foreach (var user in Users)
            {
                flowDoc.Blocks.Add(new Paragraph(new Run($"{user.Surname} {user.Name} {user.Midname}")) { TextAlignment = TextAlignment.Left, FontWeight = FontWeights.Bold, FontFamily = font, FontSize = 20 });
                Table table = new() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
                for (int i = 0; i < 4; i++) { table.Columns.Add(new TableColumn()); }
                TableRow headerRow = new()
                {
                    FontWeight = FontWeights.Bold,
                    FontFamily = font,
                    FontSize = 22,
                    Cells = { new TableCell(new Paragraph(new Run("Действие"))), new TableCell(new Paragraph(new Run("Фонд"))),
                new TableCell(new Paragraph(new Run("Опись"))), new TableCell(new Paragraph(new Run("Ед.хр."))),
                new TableCell(new Paragraph(new Run("Документ"))) }
                };
                table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });
                TableRowGroup dataGroup = new();
                int[] activities = { 1, 2 };
                string[] actions = { "Добавление", "Изменение" };
                for (int i = 0; i < activities.Length; i++)
                {
                    TableRow actionRow = new() { Cells = { new TableCell(new Paragraph(new Run(actions[i]))) } };
                    actionRow.Cells.Add(new TableCell(new Paragraph(new Run($"{dc.FondLogs.Where(u => u.User == user.Id && u.Activity == activities[i]).Count()}"))));
                    actionRow.Cells.Add(new TableCell(new Paragraph(new Run($"{dc.InventoryLogs.Where(u => u.User == user.Id && u.Activity == activities[i]).Count()}"))));
                    actionRow.Cells.Add(new TableCell(new Paragraph(new Run($"{dc.UnitLogs.Where(u => u.User == user.Id && u.Activity == activities[i]).Count()}"))));
                    actionRow.Cells.Add(new TableCell(new Paragraph(new Run($"{dc.DocumentLogs.Where(u => u.User == user.Id && u.Activity == activities[i]).Count()}"))));
                    dataGroup.Rows.Add(actionRow);
                }
                table.RowGroups.Add(dataGroup);
                flowDoc.Blocks.Add(table);
            }
            FormDoc(flowDoc);
        }
        private void FondListReport()
        {
            using ArchiveBdContext dc = new();
            ReportTitle = "Перечень Фондов";
            FlowDocument flowDoc = new();
            flowDoc.Blocks.Add(CreateHeader(ReportTitle));
            flowDoc.Blocks.Add(new Paragraph());
            flowDoc.Blocks.Add(CreateHeaderAndDateTable(ReportTitle));
            flowDoc.Blocks.Add(new Paragraph());
            Table table = new() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), FontSize = 14 };
            for (int i = 0; i < 6; i++) table.Columns.Add(new TableColumn());
            TableRow headerRow = new()
            {
                FontWeight = FontWeights.Bold,
                FontFamily = font,
                FontSize = 22,
                Cells = { new TableCell(new Paragraph(new Run("№ фонда"))), new TableCell(new Paragraph(new Run("Название фонда"))),
            new TableCell(new Paragraph(new Run("Кат."))), new TableCell(new Paragraph(new Run("Дата нач."))),
            new TableCell(new Paragraph(new Run("Дата кон."))), new TableCell(new Paragraph(new Run("Объем ед. хр.")))}
            };
            table.RowGroups.Add(new TableRowGroup { Rows = { headerRow } });
            TableRowGroup dataGroup = new();
            foreach (var item in dc.Fonds.Include(u => u.CategoryNavigation).OrderBy(u => u.Index).ThenBy(u => u.Number).ThenBy(u => u.Literal))
            {
                TableRow dataRow = new()
                {
                    Cells = { new TableCell(new Paragraph(new Run(item.FullNumber))), new TableCell(new Paragraph(new Run(item.ShortName))),
                new TableCell(new Paragraph(new Run(item.CategoryNavigation?.Name))), new TableCell(new Paragraph(new Run(item.StartDate.ToString()))),
                new TableCell(new Paragraph(new Run(item.EndDate.ToString()))), new TableCell(new Paragraph(new Run(item.Volume.ToString())))}
                };
                dataGroup.Rows.Add(dataRow);
            }
            table.RowGroups.Add(dataGroup);
            TableRow finalRow = new() { Cells = { new TableCell(new Paragraph(new Run($"Фондов всего: {dc.Fonds.Count()}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold }, new TableCell(new Paragraph(new Run($"Обьём всего: {dc.Fonds.AsEnumerable().Sum(u => u.Volume)}"))) { ColumnSpan = 3, FontWeight = FontWeights.Bold } } };
            dataGroup.Rows.Add(finalRow);
            flowDoc.Blocks.Add(table);
            FormDoc(flowDoc);
        }
        private void FormDoc(FlowDocument flowDoc)
        {
            string rtfText;
            TextRange tr = new(flowDoc.ContentStart, flowDoc.ContentEnd);
            using (MemoryStream ms = new())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }
            // Выбор расположение документа
            SaveFileDialog saveDialog = new() { Filter = "Rich Text Format (*.rtf)|*.rtf", FileName = $"{ReportTitle} на {DateTime.Now.ToString("dd MMMM yyyy")}" };
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