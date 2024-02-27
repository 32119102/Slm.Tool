using Microsoft.Toolkit.Mvvm.ComponentModel;

using wjtool.ViewModel.Controls;
using wjtool.ViewModel.Main;

namespace wjtool.ViewModel
{
    /// <summary>
    /// 模块
    /// </summary>
    public class ViewModelLocator : ObservableObject
    {
        public ViewModelLocator()
        {
            //GlobalData.Init();
        }

        public ViewLeftMainContent ViewLeftMainContent = new ViewLeftMainContent();

        public ViewMainContent ViewMainContent = new ViewMainContent();

        public CshapCodeContent CshapCodeContent = new CshapCodeContent();
    }
}