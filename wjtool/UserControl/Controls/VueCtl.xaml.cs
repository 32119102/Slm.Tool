using Data.Abstractions.Entities;

using System.Windows.Controls;

using wjtool.ViewModel.Controls;

namespace wjtool.UserControl.Controls
{
    /// <summary>
    /// Vue.xaml 的交互逻辑
    /// </summary>
    public partial class VueCtl
    {
        public VueCtl()
        {
            InitializeComponent();
            DataContext = new VueContent();
        }

        private void CheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DbTable item in e.AddedItems)
            {
                item.Selected = true;
            }
            // For on each add Items
            // do what you want
            foreach (DbTable item in e.RemovedItems)
            {
                item.Selected = false;
            }
            // For on each removed Items
            // do what you want
        }
    }
}