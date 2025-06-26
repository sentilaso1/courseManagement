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
using System.Windows.Shapes;
using CourseManagerment.Models;

namespace CourseManagerment
{
    /// <summary>
    /// Interaction logic for EditSchedule.xaml
    /// </summary>
    public partial class EditSchedule : Window
    {
        ApContext context = new ApContext();
        public CourseSchedule editSchedule { get; private set; }
        public EditSchedule(CourseSchedule schedule)
        {
            InitializeComponent();
            editSchedule = schedule;
            LoadcbData();
            cbCourseId.SelectedValue = schedule.CourseId;
            cbCourseId.IsEnabled = false;
            Picker.SelectedDate = schedule.TeachingDate;
            Slot.Text = schedule.Slot?.ToString();
            cbRoomId.SelectedValue = schedule.RoomId;
            Description.Text = schedule.Description;
        }
        private void LoadcbData()
        {
            cbCourseId.ItemsSource = context.Courses.ToList();
            cbRoomId.ItemsSource= context.Rooms.ToList();
        }

        private void saveEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                editSchedule.CourseId = (int)cbCourseId.SelectedValue;
                editSchedule.TeachingDate = Picker.SelectedDate??DateTime.Now;
                editSchedule.Slot = int.Parse(Slot.Text);
                editSchedule.RoomId = (int)cbRoomId.SelectedValue;
                editSchedule.Description = Description.Text;
                DialogResult = true;
                Close();

            }catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
    }
}
