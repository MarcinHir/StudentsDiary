using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class AddEditStudents : Form
    {
  
        private int _studentId;
        private Student _student;

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudents(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            GetStudentData();
            tbFirstName.Select();
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie Danych Ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);
                if (_student == null)
                {
                    throw new Exception("Brak użytkownika o podanym ID!!");
                }

                FillTextBoxes();    
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhysic.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbforeignLang.Text = _student.ForeignLang;
            cbPickGroup.Text = _student.Group;
            chbAdditionalTasks.Checked = _student.AdditionalTasks;
            rtbComments.Text = _student.Comments;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            AddNewStudentToList(students);

            _fileHelper.SerializeToFile(students);
            Close();


        }

        private void AddNewStudentToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysic.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbforeignLang.Text,
                Group = cbPickGroup.Text,
                AdditionalTasks = chbAdditionalTasks.Checked
            };

            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHigestId = students.
                    OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHigestId == null ?
                1 : studentWithHigestId.Id + 1;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
