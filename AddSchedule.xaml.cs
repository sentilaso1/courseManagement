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
    /// Interaction logic for AddSchedule.xaml
    /// </summary>
    public partial class AddSchedule : Window
    {
        ApContext context = new ApContext();
        public CourseSchedule newSchedule {  get;private set; }
        public AddSchedule(int? selectedCourseId = null)
        {

            InitializeComponent();
            if (selectedCourseId != null)
            {
                cbCourseId.SelectedValue = selectedCourseId;
                cbCourseId.IsEnabled=false;
            }
            LoadcbData();
        }
        private void LoadcbData()
        {
            cbCourseId.ItemsSource = context.Courses.ToList();
            cbRoomId.ItemsSource= context.Rooms.ToList();
        }

        private void saveAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newSchedule = new CourseSchedule
                {
                    CourseId = (int)cbCourseId.SelectedValue,
                    TeachingDate = Picker.SelectedDate??DateTime.Now,
                    Slot = int.Parse(Slot.Text),
                    RoomId = (int)cbRoomId.SelectedValue,
                    Description = Description.Text,
                };
                DialogResult = true;
                Close();

            }catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
    }
}
