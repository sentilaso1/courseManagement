using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CourseManagerment.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagerment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApContext context = new ApContext();
        public MainWindow()
        {
            InitializeComponent();
            LoadCourse();
            LoadData();
        }
        private void LoadCourse()
        {
            var courses = context.Courses.Include(c => c.Subject).ToList();
            cbCourse.ItemsSource = courses;
            cbCourse.DisplayMemberPath = "CourseCode";
            cbCourse.SelectedValuePath = "CourseId";
        }
        private void LoadData(int? courseId=null)
        {
            var result = new List<CourseSchedule>();
            if (courseId == null)
            {
                result = context.CourseSchedules.ToList();
            }
            else
            {
                result = context.CourseSchedules.Where(sc => sc.CourseId == courseId).ToList();
            }
           
            dgSchedule.ItemsSource = result;
        }

        private void cbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCourse.SelectedValue is int courseId) { 
                LoadData(courseId);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbCourse.SelectedIndex = -1;
            LoadData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int courseId = (int)cbCourse.SelectedValue;
            var addSchedule = new AddSchedule(courseId);

            if (addSchedule.ShowDialog() == true) {
                context.CourseSchedules.Add(addSchedule.newSchedule);
                context.SaveChanges();
                LoadData(courseId);
            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem is CourseSchedule selected)
            {
                var schedule = context.CourseSchedules.Find(selected.TeachingScheduleId);
                if (schedule != null)
                {
                    var editWindow = new EditSchedule(schedule);
                    if (editWindow.ShowDialog() == true)
                    {
                        context.SaveChanges();
                        if (cbCourse.SelectedValue is int cId)
                        {
                            LoadData(cId);
                        }
                        else
                        {
                            LoadData();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a schedule to edit");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem is CourseSchedule selected)
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var schedule = context.CourseSchedules.Find(selected.TeachingScheduleId);
                    if (schedule != null)
                    {
                        context.CourseSchedules.Remove(schedule);
                        context.SaveChanges();
                        if (cbCourse.SelectedValue is int cId)
                        {
                            LoadData(cId);
                        }
                        else
                        {
                            LoadData();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a schedule to delete");
            }
        }
    }
}