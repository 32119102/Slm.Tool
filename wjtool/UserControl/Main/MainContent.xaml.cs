using wjtool.ViewModel.Main;

namespace wjtool.UserControl.Main
{
    /// <summary>
    /// MainContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainContent
    {
        public MainContent()
        {
            InitializeComponent();

            DataContext = new ViewMainContent();
        }
    }
}