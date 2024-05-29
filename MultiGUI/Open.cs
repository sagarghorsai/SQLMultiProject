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
    public partial class Open : Form
    {
        const int KeyLength = 2;
        string PlayerKey;
   
        public Open()
        {
            InitializeComponent();
        }

        private void Open_Load(object sender, EventArgs e)
        {
            DropDown();
        }
        private void DropDown()
        {
            int i, n;
            string[][] Drivers;
            string item;

            try
            {
                Drivers = MainForm.database.GetCurrentPlayers();
                for (i = 0; i < Drivers.Length; i++)
                {
                    item = String.Empty;

                    for (n = 0; n < Drivers[i].Length; n++)
                    {
                        item += Drivers[i][n];
                        if (n < Drivers[i].Length - 1) item += ' ';
                    }
                    comboBox1.Items.Add(item);
                }
            }
            catch  (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.OpenPlayerAccepted = false;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int KeyLen;
            if (comboBox1.Text.Length < KeyLength)
                KeyLen = comboBox1.Text.Length;
            else
                KeyLen = KeyLength;
            MainForm.OpenPlayerAccepted = true;
            PlayerKey = comboBox1.Text.Substring(0, KeyLen);
            PlayerKey = PlayerKey.Trim().PadLeft(KeyLength);

            MainForm.KeyText.Text = PlayerKey;

            MainForm.OpenRecord(PlayerKey);
            

            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
