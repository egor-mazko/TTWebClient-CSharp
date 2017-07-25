using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTWebClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWndService
    {
        public MainWindow()
        {
            InitializeComponent();

            var model = new MainWindowModel(this);
            DataContext = model;
        }

        public void ShowCredsDialog(CredsDialogModel model)
        {
            CredsDialog dlg = new CredsDialog();
            dlg.DataContext = model;
            dlg.Owner = this;

            model.Closed = () => dlg.Close();

            dlg.ShowDialog();
        }

        private void TabItemPrivate_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue == false)
                TabControl.SelectedItem = TabItemPublic;
        }
    }
}
