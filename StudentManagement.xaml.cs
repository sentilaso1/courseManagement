using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CourseManagerment.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagerment
{
    public partial class StudentManagement : Window
    {
        ApContext context = new ApContext();
        public StudentManagement()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string? search = null)
        {
            var query = context.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    (s.FirstName ?? "").Contains(search) ||
                    (s.MidName ?? "").Contains(search) ||
                    (s.LastName ?? "").Contains(search));
            }
            dgStudents.ItemsSource = query.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void dgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStudents.SelectedItem is Student student)
            {
                txtStudentId.Text = student.StudentId.ToString();
                txtRoll.Text = student.Roll;
                txtFirstName.Text = student.FirstName;
                txtMidName.Text = student.MidName;
                txtLastName.Text = student.LastName;
                txtStudentId.IsEnabled = false;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var student = new Student
                {
                    StudentId = int.Parse(txtStudentId.Text),
                    Roll = txtRoll.Text,
                    FirstName = txtFirstName.Text,
                    MidName = txtMidName.Text,
                    LastName = txtLastName.Text
                };
                context.Students.Add(student);
                context.SaveChanges();
                LoadData(txtSearch.Text);
                ResetForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgStudents.SelectedItem is Student selected)
            {
                var student = context.Students.Find(selected.StudentId);
                if (student != null)
                {
                    student.Roll = txtRoll.Text;
                    student.FirstName = txtFirstName.Text;
                    student.MidName = txtMidName.Text;
                    student.LastName = txtLastName.Text;
                    context.SaveChanges();
                    LoadData(txtSearch.Text);
                    ResetForm();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to edit");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgStudents.SelectedItem is Student selected)
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var student = context.Students
                        .Include(s => s.RollCallBooks)
                        .Include(s => s.Courses)
                        .FirstOrDefault(s => s.StudentId == selected.StudentId);
                    if (student != null)
                    {
                        if (student.RollCallBooks.Any())
                        {
                            context.RollCallBooks.RemoveRange(student.RollCallBooks);
                        }
                        if (student.Courses.Any())
                        {
                            student.Courses.Clear();
                        }
                        context.Students.Remove(student);
                        context.SaveChanges();
                        LoadData(txtSearch.Text);
                        ResetForm();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete");
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
            txtSearch.Text = string.Empty;
            LoadData();
        }

        private void ResetForm()
        {
            txtStudentId.Text = string.Empty;
            txtRoll.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMidName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            dgStudents.SelectedItem = null;
            txtStudentId.IsEnabled = true;
        }
    }
}
