using wjtool.ViewModel.Controls;

namespace wjtool.UserControl.Controls
{
    /// <summary>
    /// NewModule.xaml 的交互逻辑
    /// </summary>
    public partial class NewModuleCtl
    {
        public NewModuleCtl()
        {
            InitializeComponent();
            DataContext = new NewModuleContent();
        }
    }
}