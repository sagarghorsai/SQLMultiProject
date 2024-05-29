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
    public partial class Display : Form
    {
        private static string[] ColNames = new string[5]
   { "ID", "First Name","Last Name", "Email", "GameType",
   };
        private static int[] ColWidths = new int[5]
        {25,85,85,200,75};

        private static string[][] Players;
        private static string[] TableData = new string[ColNames.Length];

        public Display()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Players = MainForm.database.GetAllPlayers();
            dataGridView1.ColumnCount = ColNames.Length;
            for (int i = 0; i < ColNames.Length; ++i)
            {
                dataGridView1.Columns[i].Name = ColNames[i];
                dataGridView1.Columns[i].Width = ColWidths[i];
            }
            for (int a = 0; a < Players.Length; ++a)
            {
                int index2;
                for (index2 = 0; index2 < Players[a].Length; ++index2)
                    TableData[index2] = Players[a][index2];
                dataGridView1.Rows.Add(TableData);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
