using LinqHomeWork_01.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
           
        }

        private void Frm作業_2_Load(object sender, EventArgs e)
        {
            productPhotoTableAdapter1.Fill(adDataSet1.ProductPhoto);
           
            //  comboBox3.Items.Add();

            SqlConnection conn = null;
            using (conn = new SqlConnection(Settings.Default.AdventureWorksConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select distinct datepart(yyyy,modifiedDate)as 'yyy' from production.productphoto order by 'yyy'";
                command.Connection = conn;
                conn.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    string years = $"{dataReader["yyy"]}";
                    comboBox3.Items.Add(years);
                    comboBox3.SelectedIndex = 0;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pictureBox1.DataBindings.Clear();
            bindingSource1.DataSource = adDataSet1.ProductPhoto;
            dataGridView1.DataSource = bindingSource1;
            pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dateTime1 = dateTimePicker1.Value;
            DateTime dateTime2 = dateTimePicker2.Value;

            int result = DateTime.Compare(dateTime1, dateTime2);
            pictureBox1.DataBindings.Clear();

            if (result == 1)
            {
                MessageBox.Show("請選擇正確區間");
            }
            else
            {

                var Date = from pp in adDataSet1.ProductPhoto
                           where pp.ModifiedDate >= dateTime1 &&
                                 pp.ModifiedDate <= dateTime2
                           select pp;

                this.bindingSource1.DataSource = Date.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.DataBindings.Clear();

            int years = int.Parse(comboBox3.Text);

            var Date = from yy in adDataSet1.ProductPhoto
                       where yy.ModifiedDate.Date.Year == years
                       select yy;
            this.bindingSource1.DataSource = Date.ToList();
            this.dataGridView1.DataSource = bindingSource1;

            pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox1.DataBindings.Clear();
            int years = int.Parse(comboBox3.Text);

            if (comboBox2.SelectedIndex == 0)
            {
                var Date = from yy in adDataSet1.ProductPhoto
                           where yy.ModifiedDate.Date.Year == years &&
                            yy.ModifiedDate.Date.Month < 4
                           select yy;
                this.bindingSource1.DataSource = Date.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            }
            else if(comboBox2.SelectedIndex == 1)
            {
                var Date = from yy in adDataSet1.ProductPhoto
                           where yy.ModifiedDate.Date.Year == years &&
                            yy.ModifiedDate.Date.Month > 3 &&
                            yy.ModifiedDate.Date.Month < 7
                           select yy;
                this.bindingSource1.DataSource = Date.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                var Date = from yy in adDataSet1.ProductPhoto
                           where yy.ModifiedDate.Date.Year == years &&
                            yy.ModifiedDate.Date.Month > 6 &&
                            yy.ModifiedDate.Date.Month < 10
                           select yy;
                this.bindingSource1.DataSource = Date.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            }
            else
            {
                var Date = from yy in adDataSet1.ProductPhoto
                           where yy.ModifiedDate.Date.Year == years &&
                            yy.ModifiedDate.Date.Month > 9 
                           select yy;
                this.bindingSource1.DataSource = Date.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                pictureBox1.DataBindings.Add("Image", bindingSource1, "LargePhoto", true);
            }
        }
    }
}
