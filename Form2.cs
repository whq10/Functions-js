using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Hotpot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "保存")
            {
                string cardId = this.textBox1.Text;
                string name = this.textBox2.Text;
                using (var cmd = new OleDbCommand("INSERT INTO [Customer] (FullName, CardId) VALUES ('" + name + "', " + cardId + ");", Form1.connection))
                {
                    cmd.ExecuteNonQuery();
                }

            }
            else if (((Button)sender).Text == "更新")
            {
                using (var cmd = new OleDbCommand("UPDATE [Customer] SET FullName='" + this.textBox2.Text + "', CardId= '"+ this.textBox1.Text + "' WHERE id=" + Form1.dataGridView1.CurrentRow.Cells["id"].Value.ToString(), Form1.connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            Form1.dtCustomers.Clear();
            Form1.adapter.Fill(Form1.dtCustomers);
            this.Close();
        }
    }
}
