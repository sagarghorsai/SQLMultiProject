using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace MultiGUI
{
    //* Program Name:ABET Part 4:GUI
    /* Date: 04/18/2022
    /* Programmer:Sagar Ghorsai
    /* Class:CSCI 234
    /* Program Description: This program is the last part of the ABET project. I am tasked to 
    implement the same function as the one for console application to GUI.
    /* Input:Text/ID/menuitems
    /* Output:Data from database
    /* Givens:None
    /* Testing Data: Can add,remove,delete,undelete and edit data in the database.

        GUI part was the most difficult one due to having to create different forms but in the end, I am proud
    *of what I got done!

    */





    public partial class MainForm : Form
    {
        public static Database database = new Database();
        private const int FIELDS = 9;
        private static int FontWidth;
        private static int LeftMargin = 6;
        private static int TopMargin = 35;
        private static int ExtraWidth = 120;
        private Graphics g;
        private Label KeyLabel = new Label();
        public static Label KeyText = new Label();
        private static string[] FieldLabelText = new string[7];
        
        private static int NumCombo = 1;
        private static int NumText = FieldLabelText.Length - NumCombo;
        private int LongestFieldLabel;
        private static int VerticalKeySep = 10;
        private static int VerticalFieldSep = 10;
        private static int VerticalFieldPad = 6;
        private Label[] FieldLabels = new Label[FieldLabelText.Length];
        public static TextBox[] FieldText = new TextBox[NumText];
        public static ComboBox[] FieldCombo = new ComboBox[NumCombo];
        private int[] FieldSize = new int[7]
        {15,15,3, 8, 25,25,31};
        private int FieldTop;
        public static bool OpenPlayerAccepted;
        static string[] Record;
        static string[] CleanRecord;
        static string[][] Games;
        static bool RecordLoaded = false;


        public MainForm()
        {
            InitializeComponent();

            try
            {
                database = new Database();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }
            NumText = Database.FieldHeads.Length - NumCombo;


            FieldLabels = new Label[Database.FieldHeads.Length];
            FieldText = new TextBox[NumText];
            FieldCombo = new ComboBox[NumCombo];
            LongestFieldLabel = Database.GetLongestFieldLabel();

            SuspendLayout();
            int i;
            g = CreateGraphics();
            FontHeight = (int)(Font.SizeInPoints / 72 * g.DpiX);
            FontWidth = (FontHeight * 8) / 10;

            FieldTop = TopMargin + FontHeight + VerticalFieldPad + VerticalFieldSep + VerticalKeySep;

            KeyLabel = new Label();
            KeyLabel.AutoSize = false;
            KeyLabel.Location = new Point(LeftMargin, TopMargin);
            KeyLabel.Name = "KeyLabel";
            KeyLabel.Text = "Player #";
            KeyLabel.Size = new Size(FontWidth * KeyLabel.Text.Length + 2, FontHeight + VerticalFieldPad);
            Controls.Add(KeyLabel);

            KeyText = new Label();
            KeyText.AutoSize = false;
            KeyText.Location = new Point(LeftMargin + KeyLabel.Size.Width + 2, TopMargin);
            KeyText.Name = nameof(KeyText);
            KeyText.Text = String.Empty;
            KeyText.Size = new Size(FontWidth * KeyLabel.Text.Length + 2, FontHeight + VerticalFieldPad);
            KeyText.BorderStyle = BorderStyle.Fixed3D;
            KeyText.BackColor = Color.White;
            Controls.Add(KeyText);



            for (i = 1; i < Database.FieldHeads.Length; i++)
            {
                FieldLabels[i] = new Label();
                FieldLabels[i].AutoSize = false;
                FieldLabels[i].Location = new Point(LeftMargin, FieldTop + i * (FontHeight + VerticalFieldPad + VerticalFieldSep));
                FieldLabels[i].Name = "FieldLabel" + i.ToString();
                FieldLabels[i].Text = Database.FieldHeads[i];
                FieldLabels[i].Size = new Size(FontWidth * LongestFieldLabel + 2, FontHeight + VerticalFieldPad);
                FieldLabels[i].TextAlign = ContentAlignment.MiddleRight;
                Controls.Add(FieldLabels[i]);


                if (i < NumText)
                {
                    FieldText[i] = new TextBox();
                    FieldText[i].AutoSize = false;
                    FieldText[i].MinimumSize = new Size(0, 0);
                    FieldText[i].Location = new Point(FieldLabels[i].Location.X + FieldLabels[i].Size.Width + 5, FieldTop + i * (FontHeight + VerticalFieldPad + VerticalFieldSep));
                    FieldText[i].Name = nameof(FieldText) + i.ToString();
                    FieldText[i].Text = string.Empty;
                    FieldText[i].Size = new Size(FontWidth * FieldSize[i] + ExtraWidth, FontHeight + VerticalFieldPad);
                    Controls.Add(FieldText[i]);
                }
                else
                {
                    FieldCombo[i - NumText] = new ComboBox();
                    FieldCombo[i - NumText].AutoSize = false;
                    FieldCombo[i - NumText].MinimumSize = new System.Drawing.Size(0, 0);
                    FieldCombo[i - NumText].Location = new Point(FieldLabels[i].Location.X + FieldLabels[i].Size.Width + 5, FieldTop + i * (FontHeight + VerticalFieldPad + VerticalFieldSep));

                    FieldCombo[i - NumText].Name = nameof(FieldCombo) + i.ToString();
                    FieldCombo[i - NumText].Text = String.Empty;
                    FieldCombo[i - NumText].Size = new Size(FontWidth * FieldSize[i - NumText] + ExtraWidth, FontHeight + VerticalFieldPad);
                    FieldCombo[i - NumText].DropDownStyle = ComboBoxStyle.DropDownList;
                    Controls.Add(FieldCombo[i - NumText]);
                }
            }

            ResumeLayout(false);


            GametypeDrop();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Database that contains players and games data \n" +
                "Made by Sagar Ghorsai");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open open = new Open();
            if (RecordLoaded) ResetRecord();
            CheckRecordChanged();
            open.ShowDialog();
        }

        private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display form1 = new Display();
            form1.ShowDialog();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void purgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to permanently purge all deleted records?", "Student Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            database.Purge();

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewForm newForm = new NewForm();
            if (RecordLoaded)
                ResetRecord();
            CheckRecordChanged();
            int num = (int)newForm.ShowDialog();
            if (NewForm.Ok)
            {
                Record = new string[FieldLabelText.Length + 1];
                Record[0] = KeyText.Text;
                Record[5] = "N";
                Record[6] = DateTime.Now.ToString();
                ClearDataEntry();
                ResetRecord();
                RecordLoaded = true;
                Copy2CleanRecord();
            }
        }
        private void GametypeDrop()
        {

            Games = database.GetGames();
            for (int i = 0; i < Games.Length; i++)
            {
                FieldCombo[0].Items.Add(Games[i][1]);
            }
        }
        private static void Copy2CleanRecord()
        {
            int i;

            CleanRecord = new string[Record.Length];
            for (i = 0; i < Record.Length; i++)
                CleanRecord[i] = Record[i];
        }
        private static void ResetRecord()
        {
            int i;
            int index;



            for (i = 1; i < Database.FieldHeads.Length; i++)
            {
                if (i < NumText)
                    Record[i] = FieldText[i].Text;
                else
                {
                    try
                    {
                        index = FieldCombo[i - NumText].SelectedIndex;
                        Record[i] = Games[index][0];
                    }

                    catch (Exception ex)
                    {
                        Record[i] = String.Empty;
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private static bool RecordChanged()
        {
            int i;

            for (i = 0; i < Record.Length; i++)
                if (CleanRecord[i] != Record[i]) return true;

            return false;
        }
        private static void CheckRecordChanged()
        {
            DialogResult result;

            if (RecordLoaded && RecordChanged())
            {
                result = MessageBox.Show("Record changed. do you want to save the changes?",
                   "Games Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    SaveRecord();
                }
            }
        }
        private static void SaveRecord()

        {
            if (RecordLoaded)
            {
                ResetRecord();
                Record[0] = KeyText.Text;

                if (RecordChanged())
                {
                    try
                    {
                        database.SaveRecord(Record);
                        Copy2CleanRecord();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "SaveRecord");
                    }
                }
                else
                    Copy2CleanRecord();
            }
            else
                MessageBox.Show("No record loaded to save");
        }
        private static void ClearDataEntry()
        {
            int i;


            for (i = 1; i < Database.FieldHeads.Length; i++)
            {
                if (i < NumText)
                    FieldText[i].Text = String.Empty;
                else
                    FieldCombo[i - NumText].SelectedIndex = -1;
            }
        }
        public static void OpenRecord(string key)
        {
            int i;

            try
            {
                Record = database.GetRecord(key);
                for (i = 0; i < Record.Length; i++)
                    if (Record[0] == "no record found")
                    {
                        KeyText.Text = string.Empty;
                        ClearDataEntry();
                        RecordLoaded = false;
                        MessageBox.Show("Record for Player " + key + " not found");
                    }
                    else
                    {

                        for (int index = 1; index < Database.FieldHeads.Length + 1; ++index)
                        {
                            if (index < NumText)
                                FieldText[index].Text = Record[index];
                            else if (index - NumText == 0)
                                FieldCombo[index - NumText].Text = FindValue(Record[index], Games, 0);
                        }
                        RecordLoaded = true;
                        Copy2CleanRecord();
                    }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " OpenRecord");
            }
        }
        public static string FindValue(string TheKey, string[][] values, int index)
        {
            int i;
            string key;
            string value;

            for (i = 0; i < values.Length; i++)
            {
                key = values[i][0];
                value = values[i][index];
                if (TheKey == key) return value;
            }

            return "Not Found";
        }


        private void GameTypebox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key;

            if (RecordLoaded)
            {
                key = Record[0];
                try
                {
                    database.LogicalDeleteRecord(key);
                    RecordLoaded = false;
                    KeyText.Text = String.Empty;
                    ClearDataEntry();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " deleteToolStripMenuItem_Click");
                }
            }
            else
                MessageBox.Show("No record loaded to delete");

        }

        private void undeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = (int)new Undelete().ShowDialog();
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
