using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace vuapos.Presentation.Views.Shared
{
    public sealed partial class _Layout : UserControl
    {
        public _Layout()
        {
            this.InitializeComponent();
        }

        public string Title
        {
            get => HeaderTitle.Text;
            set => HeaderTitle.Text = value;
        }

        public UIElement PageContent
        {
            get => (UIElement)ContentArea.Content;
            set => ContentArea.Content = value;
        }
    }
}
