using HandyControl.Tools;

using System;
using System.Collections.Generic;
using System.Windows.Input;

using wjtool.UserControl.Main;
using wjtool.ViewModel;

namespace wjtool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ViewModelLocator viewModelLocator = new ViewModelLocator();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            ControlMain.Content = new MainWindowContent();
            DataContext = viewModelLocator;

            GlobalShortcut.Init(new List<KeyBinding>
            {
                new KeyBinding(viewModelLocator.CshapCodeContent.SaveConfigCommand, Key.S, ModifierKeys.Control | ModifierKeys.Shift),
                new KeyBinding(viewModelLocator.CshapCodeContent.LoadConfigCommand, Key.D, ModifierKeys.Control | ModifierKeys.Shift),
                new KeyBinding(viewModelLocator.CshapCodeContent.CreateCodeCommand, Key.C, ModifierKeys.Control | ModifierKeys.Shift)
            });
        }
    }
}