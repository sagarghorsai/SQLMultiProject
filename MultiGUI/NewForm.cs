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
    public partial class NewForm : Form
    {
        private const int keylength = 2;
        private string PlayerKey;
        public static bool Ok;
        public NewForm()
        {
            InitializeComponent();
        }
        private bool IsInteger(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')
                    return false;
            }
            return true;
        }

        private void NewForm_Load(object sender, EventArgs e)
        {
            PlayerLists();
        }
        private void PlayerLists()
        {
            int i, n;
            string[][] Players;
            string item;

            try
            {
                Players = MainForm.database.GetCurrentPlayers();
                for (i = 0; i < Players.Length; i++)
                {
                    item = String.Empty;

                    for (n = 0; n < Players[i].Length; n++)
                    {
                        item += Players[i][n];
                        if (n < Players[i].Length - 1) item += ' ';
                    }
                    listBox1.Items.Add(item);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Players listbox");
            }
        }


        private void OK_Click(object sender, EventArgs e)
        {
            if (!IsInteger(textBox1.Text))
            {
                MessageBox.Show("Player # must be an integer");
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else if (textBox1.Text.Length > keylength)
            {
                MessageBox.Show("Player # cannot be longer than " + keylength + " digits");
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else
            {
                PlayerKey = textBox1.Text.PadLeft(keylength);
                if (MainForm.database.RecordExists(PlayerKey))
                {
                    MessageBox.Show("Player #  " + PlayerKey + " already exists");
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
                else
                {
                    MainForm.KeyText.Text = PlayerKey;
                    NewForm.Ok = true;
                    Close();
                }


            }
        }
    }
}
