﻿using AdminArchive.Classes;
using AdminArchive.ViewModel;
namespace AdminArchive.View.Windows;
public partial class Container
{
    public Container()
    {
        InitializeComponent();
        Setting.containerFrame = RootFrame;
        DataContext = new ContainerVM();
    }
}
