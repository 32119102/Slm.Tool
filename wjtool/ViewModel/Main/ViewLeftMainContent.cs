using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

using System.Collections.Generic;

using wjtool.ViewModel.Dto;

namespace wjtool.ViewModel.Main
{
    public class ViewLeftMainContent : ObservableObject
    {
        public ViewLeftMainContent()
        {
            LeftMainList = new List<LeftMainContentDto>();
            LeftMainList.Add(new LeftMainContentDto
            {
                Name = "后台代码",
                Title = "CshapCodeCtl"
            });
            LeftMainList.Add(new LeftMainContentDto
            {
                Name = "新模块",
                Title = "NewModuleCtl"
            });
            LeftMainList.Add(new LeftMainContentDto
            {
                Name = "前端代码",
                Title = "VueCtl"
            });
            LeftMainList.Add(new LeftMainContentDto
            {
                Name = "页面配置",
                Title = "PageCtl"
            });
            LeftMainList.Add(new LeftMainContentDto
            {
                Name = "表单配置",
                Title = "FormCtl"
            });
            var user = new ViewMainContent()
            {
                ContentTitle = LeftMainList[0].Name,
                SubContent = AssemblyHelper.CreateInternalInstance($"UserControl.Controls.{LeftMainList[0].Title}"),
            };
            WeakReferenceMessenger.Default.Send(new SubContentChangedMessage(user));
        }

        private List<LeftMainContentDto> _leftMainList = null;

        /// <summary>
        /// 左侧列表数据
        /// </summary>
        public List<LeftMainContentDto> LeftMainList
        {
            get => _leftMainList;
            set => SetProperty(ref _leftMainList, value);
        }
    }
}