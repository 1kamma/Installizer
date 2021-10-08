using Installizer;
using System.Windows;
namespace Inst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tasker tasker = new Tasker("ab", "cd");
            int[] i = new int[] { 1, 2 };
            tasker.TaskActionsDefine(@"C:\Program Files\PowerShell\7\pwsh.exe", null, null);
            tasker.TaskDailyTriggerrDefine("12:30");
            //tasker.TaskSettingsDefine();
            tasker.TaskPrincipal(user: "Feanor\\סארט");
            tasker.RegisterTask();
            System.Console.WriteLine("now");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SvgCanvas_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }
    }
}
