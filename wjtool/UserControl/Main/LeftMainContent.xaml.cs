using Microsoft.Toolkit.Mvvm.Messaging;

using System.Windows.Controls;

using wjtool.ViewModel;
using wjtool.ViewModel.Main;

namespace wjtool.UserControl.Main
{
    /// <summary>
    /// LeftMainContent.xaml 的交互逻辑
    /// </summary>
    public partial class LeftMainContent
    {
        public LeftMainContent()
        {
            InitializeComponent();
            DataContext = new ViewLeftMainContent();
        }

        private void ListBoxDemo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ListBoxDemo.SelectedIndex;
            var dataContent = this.DataContext as ViewLeftMainContent;
            var entity = dataContent.LeftMainList[index];
            var user = new ViewMainContent()
            {
                ContentTitle = entity.Name,
                SubContent = AssemblyHelper.CreateInternalInstance($"UserControl.Controls.{entity.Title}"),
            };
            WeakReferenceMessenger.Default.Send(new SubContentChangedMessage(user));
        }
    }
}