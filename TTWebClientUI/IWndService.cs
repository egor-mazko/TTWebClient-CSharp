using System.Windows.Threading;

namespace TTWebClientUI
{
    internal interface IWndService
    {
        Dispatcher Dispatcher { get; }
        void ShowCredsDialog(CredsDialogModel model);
    }
}
