using Installizer;
using System.Windows;
namespace Inst {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string? TaskName;
        public string? TaskPath;
        public string? FilePath;
        public string? ArugmentLists;
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Tasker tasker = new Tasker("ab", "cd");
            int[] i = new int[] { 1, 2 };
            tasker.TaskActionsDefine(@"C:\Program Files\PowerShell\7\pwsh.exe", null, null);
            tasker.TaskDailyTriggerrDefine("12:30");
            //tasker.TaskSettingsDefine();
            tasker.TaskPrincipal(user: "Feanor\\סארט");
            tasker.RegisterTask();
            System.Console.WriteLine("now");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {

        }

        private void SvgCanvas_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e) {

        }

        private void TaskName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            TaskName = e.OriginalSource.ToString();
        }

        private void TaskPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            TaskPath = e.OriginalSource.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            using (System.Windows.Forms.OpenFileDialog fileDialog = new()) {
                fileDialog.Filter = "exe files (*.exe)|*.exe";
                fileDialog.Title = "Start a file";
                fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    FilePath = fileDialog.FileName;
                }
            }
            Pathus.Text = FilePath;

        }

        private void Argusy_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            ArugmentLists = e.OriginalSource.ToString();
        }
    }
}
