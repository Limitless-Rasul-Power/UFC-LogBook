using Guna.UI2.WinForms;
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

namespace WindowsForms_LogBook
{
    public partial class UC1 : UserControl
    {
        public Guna2CustomRadioButton HereRdBtn { get => IsHereRdBtn; }
        public Guna2CustomRadioButton LateRdBtn { get => IsLateRdBtn; }
        public Guna2CustomRadioButton NotHereRdBtn { get => IsNotHereRdBtn; }
        public Guna2ComboBox ExamWorkGradeCmbBx { get => ExamWorkCmbBx; }
        public Guna2ComboBox UFCWorkGradeCmbBx { get => UFCWorkCmbBx; }
        public Guna2TextBox CommentTxtBx { get => CommentsTxtBx; }

        public int CrystalCount
        {
            get
            {
                if (Panel1ThirdDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                    return 3;
                else if (Panel1SecondDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                    return 2;
                else if (Panel1FirstDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                    return 1;

                return 0;
            }
        }


        private string _pictureBox;
        private string _fullName;
        private int _number;
        private string _commentsTxtBxTextHelper = default;
        private readonly List<string> gradesExamWork = new List<string>();
        private readonly List<string> gradesUFCWork = new List<string>();
        private int _numberHolder = default;

        private Warrior _warrior = default;
        public Warrior Warrior
        {
            get => _warrior;

            set 
            {
                if (value != null && File.Exists("Database/Warriors.json") && File.Exists("Database/CrystalCount.json"))
                {
                    _warrior = value;
                    SetPropertiesFromFile();
                }

            }
        }

        public UC1()
        {
            InitializeComponent();

            JsonFileHelper.JSONDeSerialization(ref gradesExamWork, "Grades.json");
            JsonFileHelper.JSONDeSerialization(ref gradesUFCWork, "Grades.json");

            UFCWorkCmbBx.DataSource = gradesUFCWork;
            UFCWorkCmbBx.DropDownStyle = ComboBoxStyle.DropDown;

            ExamWorkCmbBx.DataSource = gradesExamWork;
            ExamWorkCmbBx.DropDownStyle = ComboBoxStyle.DropDown;

            

            DeleteToolTip.SetToolTip(DeletePctBx, "Delete marked crystal or crytals in this warrior.");
            CommentToolTip.SetToolTip(CommentPctBx, "Write comment about this warrior");
            IsHereToolTip.SetToolTip(IsHereRdBtn, "Here");
            IsLateToolTip.SetToolTip(IsLateRdBtn, "Late");
            IsNotHereToolTip.SetToolTip(IsNotHereRdBtn, "Not Here");
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; NumberLbl.Text = value.ToString(); }
        }


        public string PictureBoxPath
        {
            get { return _pictureBox; }
            set { _pictureBox = value; IconPctBx.Image = new Bitmap(value); ExtendedImagePctBx.Image = new Bitmap(value); }
        }

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; FullNameLbl.Text = value; }
        }
        public Guna2HtmlLabel CrystalLbl { get; set; }



        public void IconPctBx_MouseHover(object sender, EventArgs e)
        {
            ExtendedImagePctBx.Visible = true;
            FullNameLbl.Visible = false;
        }
        public void IconPctBx_MouseLeave(object sender, EventArgs e)
        {
            ExtendedImagePctBx.Visible = false;
            FullNameLbl.Visible = true;
        }
        public void DeletePctBx_MouseHover(object sender, EventArgs e)
        {
            DeletePctBx.Image = new Bitmap("../../Images/delete.png");
        }
        public void DeletePctBx_MouseLeave(object sender, EventArgs e)
        {
            DeletePctBx.Image = new Bitmap("../../Images/empty delete.png");
        }
        public void SetVisibilityToCommentIconAndInvisibilityToCommentsTxtBxAndAndBtns()
        {
            CommentPctBx.Visible = true;
            CommentsTxtBx.Visible = false;
            CommentsSaveBtn.Visible = false;
            CommentsIgnoreBtn.Visible = false;
        }
        public void SetInvisibilityToCommentIconAndVisibilityToCommentsTxtBxAndAndBtns()
        {
            CommentPctBx.Visible = false;
            CommentsTxtBx.Visible = true;
            CommentsSaveBtn.Visible = true;
            CommentsIgnoreBtn.Visible = true;
        }
        public void CommentPctBx_Click(object sender, EventArgs e)
        {
            SetInvisibilityToCommentIconAndVisibilityToCommentsTxtBxAndAndBtns();
            _commentsTxtBxTextHelper = CommentsTxtBx.Text;
        }
        public void CommentsSaveBtn_Click(object sender, EventArgs e)
        {
            SetVisibilityToCommentIconAndInvisibilityToCommentsTxtBxAndAndBtns();
        }
        public void CommentsIgnoreBtn_Click(object sender, EventArgs e)
        {
            SetVisibilityToCommentIconAndInvisibilityToCommentsTxtBxAndAndBtns();
            CommentsTxtBx.Text = _commentsTxtBxTextHelper;
        }
        public void SetDiamondImage(Guna2PictureBox pictureBox, string image)
        {
            pictureBox.Image = new Bitmap(image);

            if (image == "../../Images/diamond.png")
                pictureBox.ImageLocation = "../../Images/diamond.png";
            else
                pictureBox.ImageLocation = default;
        }
        public void SetCrystalNumber()
        {
            if (Panel1FirstDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) + 1).ToString();
            if (Panel1SecondDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) + 1).ToString();
            if (Panel1ThirdDiamondPctBx.ImageLocation == "../../Images/diamond.png")
                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) + 1).ToString();
        }
        public void Panel1FirstDiamondPctBx_Click(object sender, EventArgs e)
        {
            if (int.TryParse(CrystalLbl.Text, out int result) && result + _numberHolder - 1 >= 0)
            {
                SetCrystalNumber();

                SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/diamond.png");
                SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/empty diamond.png");
                SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/empty diamond.png");

                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) - 1).ToString();
                _numberHolder = 1;
            }
        }
        public void Panel1SecondDiamondPctBx_Click(object sender, EventArgs e)
        {
            if (int.TryParse(CrystalLbl.Text, out int result) && result + _numberHolder - 2 >= 0)
            {
                SetCrystalNumber();

                SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/diamond.png");
                SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/diamond.png");
                SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/empty diamond.png");


                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) - 2).ToString();
                _numberHolder = 2;
            }
        }
        public void Panel1FThirdDiamondPctBx_Click(object sender, EventArgs e)
        {
            if (int.TryParse(CrystalLbl.Text, out int result) && result + _numberHolder - 3 >= 0)
            {
                SetCrystalNumber();

                SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/diamond.png");
                SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/diamond.png");
                SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/diamond.png");


                CrystalLbl.Text = (int.Parse(CrystalLbl.Text) - 3).ToString();
                _numberHolder = 3;
            }
        }
        public void DeletePctBx_Click(object sender, EventArgs e)
        {
            if (Panel1FirstDiamondPctBx.ImageLocation == "../../Images/diamond.png")
            {
                SetCrystalNumber();

                SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/empty diamond.png");
                SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/empty diamond.png");
                SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/empty diamond.png");

                _numberHolder = default;
            }

        }
        public void MarkAllRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            IsHereRdBtn.Checked = true;
        }
        public void IsHereRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            ChangeToolsState(true);
        }
        public void IsLateRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            ChangeToolsState(true);
        }
        public void IsNotHereRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            ExamWorkCmbBx.SelectedIndex = 0;
            UFCWorkCmbBx.SelectedIndex = 0;

            SetCrystalNumber();

            SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/empty diamond.png");
            SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/empty diamond.png");
            SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/empty diamond.png");


            ChangeToolsState(false);
        }
        public void ChangeToolsState(bool flag)
        {
            ExamWorkCmbBx.Enabled = flag;
            UFCWorkCmbBx.Enabled = flag;
            Panel1FirstDiamondPctBx.Enabled = flag;
            Panel1SecondDiamondPctBx.Enabled = flag;
            Panel1ThirdDiamondPctBx.Enabled = flag;
            DeletePctBx.Enabled = flag;
        }

        private void SetPropertiesFromFile()
        {
            if (_warrior.State == AttentionStates.IsHere)
                IsHereRdBtn.Checked = true;
            else if (_warrior.State == AttentionStates.IsLate)
                IsLateRdBtn.Checked = true;
            else if (_warrior.State == AttentionStates.IsNotHere)
                IsNotHereRdBtn.Checked = true;

            ExamWorkCmbBx.Text = _warrior.ExaminationWorkGrade;
            UFCWorkCmbBx.Text = _warrior.UFCWorkGrade;

            if (_warrior.CrystalCount >= 1)
            {
                SetDiamondImage(Panel1FirstDiamondPctBx, "../../Images/diamond.png");
            }
            if (_warrior.CrystalCount >= 2)
            {
                SetDiamondImage(Panel1SecondDiamondPctBx, "../../Images/diamond.png");
            }
            if (_warrior.CrystalCount == 3)
            {
                SetDiamondImage(Panel1ThirdDiamondPctBx, "../../Images/diamond.png");
            }

            _numberHolder = _warrior.CrystalCount;

            if (!string.IsNullOrWhiteSpace(_warrior.CommentFromTrainer))
            {
                CommentsTxtBx.Text = _warrior.CommentFromTrainer;
            }

        }




    }
}
