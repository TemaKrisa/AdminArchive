using AdminArchive.Classes;
using AdminArchive.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AdminArchive.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для Container.xaml
    /// </summary>
    public partial class Container 
    {
        public Container()
        {
            InitializeComponent();
            //string rootPath = @"C:\Users\A S\source\AdminArchive\Model\";
            //var header = "***********************************" + Environment.NewLine;

            //var files = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories);

            //var result = files.Select(path => new { Name = Path.GetFileName(path), Contents = File.ReadAllText(path) })
            //                  .Select(info =>
            //                      header
            //                    + "Filename: " + info.Name + Environment.NewLine
            //                    + header
            //                    + info.Contents);


            //var singleStr = string.Join(Environment.NewLine, result);
            //Console.WriteLine(singleStr);
            //File.WriteAllText(@"C:\Launchers\output.txt", singleStr, Encoding.UTF8);

            FrameManager.containerFrame = RootFrame;
            DataContext = new ContainerVM();
        }
    }
}
