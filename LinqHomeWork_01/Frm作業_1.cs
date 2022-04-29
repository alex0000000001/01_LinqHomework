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
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            productsTableAdapter1.Fill(nwDataSet1.Products);
            order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files =  dir.GetFiles();

            this.dataGridView1.DataSource = files;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from log in files
                    where log.Extension == ".log"
                    select log;

            dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from log in files
                    where log.CreationTime.Year ==2019
                    select log;

            dataGridView1.DataSource = q.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from log in files
                    where log.Length > 10000
                    select log;

            dataGridView1.DataSource = q.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = nwDataSet1.Orders;
        }

        private void Frm作業_1_Load(object sender, EventArgs e)
        {
            SqlConnection conn=null;
            using (conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select distinct datepart(yyyy,orderdate)as 'yyy' from Orders order by 'yyy'";
                command.Connection = conn;
                conn.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    string years = $"{dataReader["yyy"]}";
                    comboBox1.Items.Add(years);
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int yee = int.Parse(comboBox1.Text);

            var q = from left in nwDataSet1.Orders
                    where left.OrderDate.Date.Year == yee
                    select left;
            this.dataGridView1.DataSource = q.ToList();

            var qq = from right in nwDataSet1.Order_Details
                     join left in nwDataSet1.Orders
                     on right.OrderID equals left.OrderID
                     where left.OrderDate.Year == yee
                     select right;
            this.dataGridView2.DataSource = qq.ToList();

        }

        private void button12_Click(object sender, EventArgs e)
        {   //上一筆
            int num = int.Parse(textBox1.Text); 
            
          //  int nowTop = (num * times) + 1;
          //  int nowBottom = (num * (times + 1)) - 1;
            try
            {
                if (textchange == true)
                {
                    times = 1;
                    now = num * times;
                    
                    var q3 = from aa in nwDataSet1.Products.Skip(nowChange-now).Take(num)
                             select aa;
                    afterChage = nowChange - now*2;
                    dataGridView1.DataSource = q3.ToList();

                    textchange = !textchange;
                }
                else 
                {
                    times -= 1;
                    now = num * times;
                    nowChange = now;
                    var q2 = from aa in nwDataSet1.Products.Skip(Math.Abs(afterChage - now)).Take(num)
                             select aa;

                    dataGridView1.DataSource = q2.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int times = 0;
        int now;
        int nowChange;
        int afterChage = 0;

        private void button13_Click(object sender, EventArgs e)
        {   //下一筆
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()

            int num = int.Parse(textBox1.Text);

           

            try
            { 
                if (times < 0) 
                {
                    times = 0;

                    times += 1;
                     now = num * times;

                    var q3 = from aa in nwDataSet1.Products.Skip(now).Take(num)
                             select aa;
                    dataGridView1.DataSource = q3.ToList();
                }
                else
                {

                    if (textchange == true)
                    {
                        times = 1;
                        now = num * times;

                        var q4 = from aa in nwDataSet1.Products.Skip(nowChange + now).Take(num)
                                 select aa;
                        afterChage = nowChange - now * 2;
                        dataGridView1.DataSource = q4.ToList();

                        textchange = !textchange;
                    }
                    else
                    {
                        times += 1;
                        now = num * times;
                        nowChange = now;
                        var q3 = from aa in nwDataSet1.Products.Skip(Math.Abs(afterChage + now)).Take(num)
                                 select aa;
                        dataGridView1.DataSource = q3.ToList();

                    }

                    if (nwDataSet1.Products.Rows.Count < num * (times+1))
                    {
                        //times -= 1;
                        MessageBox.Show("已經最後一筆了");
                        //return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool textchange;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textchange = true;
        }
    }
}
