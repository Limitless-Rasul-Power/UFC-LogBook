using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.IO;

namespace WindowsForms_LogBook
{
    public partial class Form1 : Form
    {
        private readonly List<Warrior> Warriors = new List<Warrior>();
        private string _expTxtBxTextHelper = default;        
        private bool _isEditIconPressed = false;        
        private readonly int _currentY = default;
        private readonly List<UC1> UserControls = new List<UC1>();

        public readonly List<string> _imagePaths = new List<string>() { "../../Images/Conor.jpg", "../../Images/Jon Jones.jpg",
        "../../Images/Ronda Rousey.jpg", "../../Images/Michael Bisping.jpg", "../../Images/Joanna.jpg"};

        public Form1()
        {
            InitializeComponent();           

            CrystalToolTip.SetToolTip(CrystalSymbolPctBx, "Awards which warriors have for special works.");
            EditToolTip.SetToolTip(EditPctBx, "Add which experience or experiences will warriors see today.");
            MarkAllToolTip.SetToolTip(MarkAllRdBtn, "Everybody here");

            if (File.Exists("Database/Warriors.json") && File.Exists("Database/CrystalCount.json"))
            {
                string helper = default;
                JsonFileHelper.JSONDeSerialization(ref Warriors, "Warriors.json");
                JsonFileHelper.JSONDeSerialization(ref helper, "CrystalCount.json");
                CrystalLbl.Text = helper;
            }
            else
            {
                Warriors.Add(new Warrior("Conor McGregor", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Warriors.Add(new Warrior("Jon Jones", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Warriors.Add(new Warrior("Ronda Rousey", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Warriors.Add(new Warrior("Michael Bisping", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Warriors.Add(new Warrior("Joanna Jedrzejczyk", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
            }

            if (File.Exists("Database/Experience.json"))
            {
                string helper = default;
                JsonFileHelper.JSONDeSerialization(ref helper, "Experience.json");               
                ExperienceTxtBx.Text = helper;                
            }

            _currentY = guna2ShadowPanel1.Location.Y;

            for (int i = 0; i < Warriors.Count; i++)
            {
                UserControls.Add(new UC1());

                UserControls[i].CrystalLbl = CrystalLbl;

                UserControls[i].PictureBoxPath = _imagePaths[i];
                UserControls[i].FullName = Warriors[i].FullName;
                UserControls[i].Number = i + 1;

                _currentY += 110;
                UserControls[i].Location = new Point(35, _currentY);

                UserControls[i].Warrior = Warriors[i];
                Controls.Add(UserControls[i]);
            }

        }
      
        private void SetVisibilityWhenWritingExperience()
        {
            ExperienceTxtBx.Visible = true;
            SaveBtn.Visible = true;
            IgnoreBtn.Visible = true;           
        }

        private void SetInvisibilityWhenFinishExperience()
        {
            ExperienceTxtBx.Visible = false;
            SaveBtn.Visible = false;
            IgnoreBtn.Visible = false;
        }
        private void EditPctBx_Click(object sender, MouseEventArgs e)
        {
            if (_isEditIconPressed == false)
            {
                _isEditIconPressed = true;
                _expTxtBxTextHelper = ExperienceTxtBx.Text;
                SetVisibilityWhenWritingExperience();
            }
            else
            {
                _isEditIconPressed = false;
                SetInvisibilityWhenFinishExperience();
                ExperienceTxtBx.Text = _expTxtBxTextHelper;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SetInvisibilityWhenFinishExperience();
            _isEditIconPressed = false;
        }

        private void IgnoreBtn_Click(object sender, EventArgs e)
        {
            SetInvisibilityWhenFinishExperience();
            ExperienceTxtBx.Text = _expTxtBxTextHelper;
            _isEditIconPressed = false;
        }


        private void MarkAllRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < UserControls.Count; i++)
            {
                UserControls[i].MarkAllRdBtn_CheckedChanged(sender, e);
            }            
        }

        private void ExitBtn_MouseHover(object sender, EventArgs e)
        {
            ExitBtn.BorderThickness = 0;
            ExitBtn.FillColor = Color.Red;            
        }

        private void ExitBtn_MouseLeave(object sender, EventArgs e)
        {
            ExitBtn.BorderThickness = 1;
            ExitBtn.FillColor = ColorTranslator.FromHtml("#fffff");
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
            WriteFileWhenApplicationEnded();
        }   
        

        private void WriteFileWhenApplicationEnded()
        {
            for (int i = 0; i < UserControls.Count; i++)
            {
                if (UserControls[i].HereRdBtn.Checked)
                    Warriors[i].State = AttentionStates.IsHere;
                else if (UserControls[i].LateRdBtn.Checked)
                    Warriors[i].State = AttentionStates.IsLate;
                else if (UserControls[i].NotHereRdBtn.Checked)
                    Warriors[i].State = AttentionStates.IsNotHere;
                else
                    Warriors[i].State = AttentionStates.None;

                if (UserControls[i].ExamWorkGradeCmbBx.Text == "-")
                {
                    Warriors[i].ExaminationWorkGrade = "Didn't get any grade";
                }
                else
                    Warriors[i].ExaminationWorkGrade = UserControls[i].ExamWorkGradeCmbBx.Text;


                if (UserControls[i].UFCWorkGradeCmbBx.Text == "-")
                {
                    Warriors[i].UFCWorkGrade = "Didn't get any grade";
                }
                else
                    Warriors[i].UFCWorkGrade= UserControls[i].UFCWorkGradeCmbBx.Text;

                Warriors[i].CrystalCount = UserControls[i].CrystalCount;

                Warriors[i].CommentFromTrainer = UserControls[i].CommentTxtBx.Text;

            }

            if (!string.IsNullOrWhiteSpace(ExperienceTxtBx.Text))
                JsonFileHelper.JSONSerialization(ExperienceTxtBx.Text, "Experience.json");

            JsonFileHelper.JSONSerialization(Warriors, "Warriors.json");
            JsonFileHelper.JSONSerialization(CrystalLbl.Text, "CrystalCount.json");
        }

    }
}
