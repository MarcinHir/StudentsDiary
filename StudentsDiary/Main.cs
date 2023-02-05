using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private string _filePath =
            Path.Combine(Environment.CurrentDirectory, "Students.txt");
        public Main()
        {

            InitializeComponent();
            var students = DeserializeFromFile();

            dgvDiary.DataSource = students;

        }
        public void SerializeToFile(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public List<Student> DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Student>();
           
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
        }




        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile();

            dgvDiary.DataSource = students;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybierz ucznia którego chcesz edytować !");
                return;
            }
            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[0].Value.ToString() + " " + selectedStudent.Cells[1].Value.ToString()).Trim()}", "Usuwanie Ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                var students = DeserializeFromFile();
                students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[2].Value));
                SerializeToFile(students);
                dgvDiary.DataSource = students;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę wybierz ucznia którego chcesz edytować !");
                return;
            }
            AddEditStudents addEditStudents = new AddEditStudents(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[2].Value));
            
            addEditStudents.ShowDialog();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditStudents addEditStudents = new AddEditStudents();
            addEditStudents.ShowDialog();

        }
    }
}
