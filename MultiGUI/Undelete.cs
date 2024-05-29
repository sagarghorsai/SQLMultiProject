using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiGUI
{
    public partial class Undelete : Form
    {
        private const int KeyLength = 2;
        private string PlayerKey;

        public Undelete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                PlayerKey = comboBox1.Text.Substring(0, 2);
                MainForm.database.UnDeleteRecord(PlayerKey);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void DeletedPlayers ()
        {
            string[][] deletedStudents = MainForm.database.GetDeletedPlayers();
            try
            {

            
                for (int index1 = 0; index1 < deletedStudents.Length; ++index1)
                {
                    string empty = string.Empty;
                    for (int index2 = 0; index2 < deletedStudents[index1].Length; ++index2)
                    {
                        empty += deletedStudents[index1][index2];
                        if (index2 < deletedStudents[index1].Length - 1)
                            empty += " ";
                    }
                    comboBox1.Items.Add((object)empty);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Undelete_Load(object sender, EventArgs e)
        {
            DeletedPlayers();
        }
    }
}
