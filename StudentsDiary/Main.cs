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
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();

            var number = new List<Student>();
            

        }
        
        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            var chosenGroup = cbGroup.Text;


            if (chosenGroup == "Wszystkie")
                dgvDiary.DataSource = students;

            else 
                dgvDiary.DataSource = students.FindAll(x => x.Group == chosenGroup);

        }
        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Imię";
            dgvDiary.Columns[1].HeaderText = "Nazwisko";
            dgvDiary.Columns[2].HeaderText = "Numer";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Fizyka";
            dgvDiary.Columns[6].HeaderText = "Technologia";
            dgvDiary.Columns[7].HeaderText = "Język Polski";
            dgvDiary.Columns[8].HeaderText = "Język Obcy";
            dgvDiary.Columns[9].HeaderText = "Grupa";
            dgvDiary.Columns[10].HeaderText = "Zajęcia Dodatkowe";
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
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
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[2].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
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
            
            addEditStudents.FormClosing += AddEditStudents_FormClosing;
            addEditStudents.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditStudents addEditStudents = new AddEditStudents();
            addEditStudents.FormClosing += AddEditStudents_FormClosing;
            addEditStudents.ShowDialog();
        }

        private void AddEditStudents_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        //private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var students = _fileHelper.DeserializeFromFile();
 
        //    if (cbGroup.Text == "Wszystkie")
        //    {
                
        //        dgvDiary.DataSource = students;
        //    }
        //    else
        //    {
        //        dgvDiary.DataSource = students.Where(x => x.Group == cbGroup.Text);
        //    }
            
            
        //}
    }
}
