using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace wjtool.ViewModel.Main
{
    public class ContentTitleChangedMessage : ValueChangedMessage<string>
    {
        public ContentTitleChangedMessage(string value) : base(value)
        {
        }
    }

    public class SubContentChangedMessage : ValueChangedMessage<ViewMainContent>
    {
        public SubContentChangedMessage(ViewMainContent value) : base(value)
        {
        }
    }

    public class ViewMainContent : ObservableObject
    {
        public ViewMainContent()
        {
            UpdateMainContent();
        }

        private string _contentTitle;

        /// <summary>
        /// 标题
        /// </summary>
        public string ContentTitle
        {
            get => _contentTitle;
            set => SetProperty(ref _contentTitle, value);
        }

        private object _subContent = null;

        /// <summary>
        ///     子内容
        /// </summary>
        public object SubContent
        {
            get => _subContent;
            set => SetProperty(ref _subContent, value);
        }

        private void UpdateMainContent()
        {
            //WeakReferenceMessenger.Default.Register<ViewMainContent, ContentTitleChangedMessage>
            // (this, (r, m) =>
            // {
            //     r.ContentTitle = m.Value;
            // });

            WeakReferenceMessenger.Default.Register<ViewMainContent, SubContentChangedMessage>
            (this, (r, m) =>
            {
                r.ContentTitle = m.Value.ContentTitle;
                r.SubContent = m.Value.SubContent;
            });
        }
    }
}