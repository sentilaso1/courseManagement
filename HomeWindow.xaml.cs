using System.Windows;

namespace CourseManagerment
{
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            var win = new MainWindow();
            win.ShowDialog();
        }

        private void Student_Click(object sender, RoutedEventArgs e)
        {
            var win = new StudentManagement();
            win.ShowDialog();
        }
    }
}
