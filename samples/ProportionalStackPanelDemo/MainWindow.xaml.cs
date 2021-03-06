﻿using Avalonia;
using Avalonia.Markup.Xaml;
using Dock.Avalonia.Controls;

namespace ProportionalStackPanelDemo
{
    public class MainWindow : HostWindowBase
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
