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
    public partial class Form1 : Form
    {

        public static OleDbConnection connection = null;
        public static DataTable dtCustomers = null;
        public static OleDbDataAdapter adapter = null;
        public Form1()
        {
            InitializeComponent();

            var conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=Hotpot.mdb;");
            conn.Open();
            connection = conn;

            dtCustomers = new DataTable();

            adapter = new OleDbDataAdapter("SELECT * FROM Customer",
                conn);
            adapter.Fill(dtCustomers);

            dataGridView1.DataSource = dtCustomers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            

            //using (var cmd = new OleDbCommand("INSERT INTO [Customer] (FullName, CardId) VALUES ('Test Name', 123456);", connection))
            //{
            //    cmd.ExecuteNonQuery();
            //}

            //dtCustomers.Clear();
            //adapter.Fill(dtCustomers);

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2();
            f2.button2.Text = "更新";
            f2.ShowDialog();

            //using (var cmd = new OleDbCommand("UPDATE [Customer] SET FullName='Updated Row', CardId=9876 WHERE id=" + dataGridView1.CurrentRow.Cells["id"].Value.ToString(), connection))
            //{
            //    cmd.ExecuteNonQuery();
            //}

            //dtCustomers.Clear();
            //adapter.Fill(dtCustomers);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            using (var cmd = new OleDbCommand("DELETE FROM Customer WHERE id=" + dataGridView1.CurrentRow.Cells["id"].Value.ToString(), connection))
            {
                cmd.ExecuteNonQuery();
            }

            dtCustomers.Clear();
            adapter.Fill(dtCustomers);


        }

        private void button4_Click(object sender, EventArgs e)
        {
            var row = dataGridView1.Rows[0];
            var cell = row.Cells[1];
            MessageBox.Show(cell.Value.ToString());
            dataGridView1.BeginEdit(false); // begins edit mode (i.e. if we manually double-clicked cell)
            cell.Value = "444";
            dataGridView1.EndEdit(); // ends edit mode, saves to DT
            adapter.Update(dtCustomers); // writes DT changes to DB (adapter should be set up properly - read above)

        }
    }
}
