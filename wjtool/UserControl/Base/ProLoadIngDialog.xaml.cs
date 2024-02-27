using wjtool.ViewModel;

namespace wjtool.UserControl.Base
{
    /// <summary>
    /// ProLoadIngDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProLoadIngDialog
    {
        private ProLoadIngDialogViewModel _proLoadIngDialogViewModel;

        public ProLoadIngDialog(string msg)
        {
            InitializeComponent();
            _proLoadIngDialogViewModel = new ProLoadIngDialogViewModel();
            _proLoadIngDialogViewModel.Text = msg;
            DataContext = _proLoadIngDialogViewModel;
        }
    }
}