using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace app
{
    public partial class Form1 : Form
    {
        SqlConnection cs = new SqlConnection("Data Source=DESKTOP-93U1OMQ;Initial Catalog=RPG;Integrated Security=True");
        SqlDataAdapter daParinte = new SqlDataAdapter();
        DataSet dsParinte = new DataSet();
        SqlDataAdapter daFiu = new SqlDataAdapter();
        DataSet dsFiu = new DataSet();
        public Form1()
        {
            InitializeComponent();
            daParinte.SelectCommand = new SqlCommand("SELECT * FROM potion_type", cs);
            dsParinte.Clear();
            daParinte.Fill(dsParinte);
            dataGridView2.DataSource = dsParinte.Tables[0];
        }

        private void ResetForm()
        {
            idTextBox.Text = "";
            powerTextBox.Text = "";
            effectTextBox.Text = "";
        }

        private void ClearTable(DataSet ds, SqlDataAdapter da)
        {
            ds.Clear();
            da.Fill(dsFiu);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void conectare_Click(object sender, EventArgs e)
        {
            //da.SelectCommand = new SqlCommand("SELECT * FROM potion", cs);
            //ds.Clear();
            //da.Fill(ds);
            //dataGridView1.DataSource = ds.Tables[0];
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCell cell = dataGridView2.SelectedRows[0].Cells[0];
                if (String.IsNullOrEmpty(idTextBox.Text))
                {
                    daFiu.InsertCommand = new SqlCommand("INSERT INTO potion (potion_type_int, power_effect, additional_effect) VALUES(@t, @p, @e)", cs);
                    daFiu.InsertCommand.Parameters.Add("@t", SqlDbType.Int).Value = Int32.Parse(cell.Value.ToString());
                    daFiu.InsertCommand.Parameters.Add("@p", SqlDbType.Int).Value = Int32.Parse(powerTextBox.Text);
                    daFiu.InsertCommand.Parameters.Add("@e", SqlDbType.VarChar).Value = effectTextBox.Text;
                    cs.Open();
                    daFiu.InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("Inserted successfully into the database.");
                    cs.Close();
                }
                else
                {

                    daFiu.UpdateCommand = new SqlCommand("UPDATE potion SET potion_type_int = @t, power_effect = @p, additional_effect = @e WHERE id = @i", cs);
                    daFiu.UpdateCommand.Parameters.Add("@t", SqlDbType.Int).Value = Int32.Parse(cell.Value.ToString());
                    daFiu.UpdateCommand.Parameters.Add("@p", SqlDbType.Int).Value = Int32.Parse(powerTextBox.Text);
                    daFiu.UpdateCommand.Parameters.Add("@e", SqlDbType.VarChar).Value = effectTextBox.Text;
                    daFiu.UpdateCommand.Parameters.Add("@i", SqlDbType.Int).Value = Int32.Parse(idTextBox.Text);
                    cs.Open();
                    daFiu.UpdateCommand.ExecuteNonQuery();
                    MessageBox.Show("Updated successfully the database.");
                    cs.Close();
                }
                // already inserted
                ClearTable(dsFiu, daFiu);
                ResetForm();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                cs.Close();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                idTextBox.Text = row.Cells[0].Value.ToString();
                powerTextBox.Text = row.Cells[2].Value.ToString();
                effectTextBox.Text = row.Cells[3].Value.ToString();
            }
            catch(Exception ex)
            {

            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCell cell = dataGridView2.SelectedRows[0].Cells[0];
                daFiu.SelectCommand = new SqlCommand("SELECT * FROM potion where potion_type_int = @p", cs);
                daFiu.SelectCommand.Parameters.Add("@p", Int32.Parse(cell.Value.ToString()));
                cs.Open();
                daFiu.SelectCommand.ExecuteNonQuery();
                cs.Close();
            }
            catch(Exception ex)
            {
                cs.Close();
            }
            ClearTable(dsFiu, daFiu);
        }
    }
}
