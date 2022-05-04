using LinqHomeWork_01;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            TreeNode node = null;

            foreach (int a in nums)
            {
                string sizes = MyKey(a);

                if (treeView1.Nodes[sizes] == null)
                {
                    node = this.treeView1.Nodes.Add(sizes);
                    node.Name = sizes;
                    node.Nodes.Add(a.ToString());
                }
                else
                {
                    node.Nodes.Add(a.ToString());
                }
            }
        }

        private string MyKey(int a)
        {
            if (a < 4)
                return "Small";
            else if (a < 7)
                return "Medium";
            else
                return "Large";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();

            var q = from w in files
                    group w by MyGroup(w.Length) into g
                    select new
                    {
                        Mykey = g.Key,
                        Mycount = g.Count(),
                        Mygroup = g

                    };

            dataGridView1.DataSource = q.ToList();

            foreach (var group in q)
            {
                string s = $"{group.Mykey}-({group.Mycount})";
                TreeNode node = treeView1.Nodes.Add(group.Mykey.ToString(), s);
                foreach (var item in group.Mygroup)
                {
                    string a = (item.Length).ToString();
                    node.Nodes.Add(item.ToString() + " Size = " + a);
                }
            }

        }

        private object MyGroup(long length)
        {
            if (length < 1000)
                return "Small";
            else if (length < 5000)
                return "Medium";
            else
                return "Large";


        }

        private void button6_Click(object sender, EventArgs e)
        {
            TreeNode node = null;
            treeView1.Nodes.Clear();

            DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();

            var q = from w in files
                    orderby w.CreationTime descending
                    group w by w.CreationTime.Year into g

                    select new
                    {
                        Mykey = g.Key,
                        Mycount = g.Count(),
                        Mygroup = g

                    };

            dataGridView1.DataSource = q.ToList();

            foreach (var group in q)
            {
                string s = $"{group.Mykey}-({group.Mycount})";
                node = treeView1.Nodes.Add(group.Mykey.ToString(), s);
                foreach (var item in group.Mygroup)
                {
                    string a = (item.Length).ToString();
                    node.Nodes.Add(item.ToString() + " Size = " + a);
                }
            }
        }
        NorthwindEntities dbContext = new NorthwindEntities();
        private void button8_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();

            var q = from p in dbContext.Products.AsEnumerable()
                    where p.UnitPrice != null
                    orderby p.UnitPrice ascending
                    group p by MyMethod(p.UnitPrice) into g
                    select new
                    {
                        g.Key,
                        Myconut = g.Count(),
                        Mygroup = g
                    };

            dataGridView1.DataSource = q.ToList();

            foreach (var group2 in q)
            {
                TreeNode node = null;
                node = treeView1.Nodes.Add(group2.Key.ToString());
                foreach (var item2 in group2.Mygroup)
                {
                    node.Nodes.Add(item2.ProductName + " -- UnitPrice = " + item2.UnitPrice);
                }
            }
        }

        private object MyMethod(decimal? a)
        {
            if (a < 20)
                return "低價區";
            else if (a < 40)
                return "中價區";
            else
                return "高價區";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var q = (from o in dbContext.Orders
                     group o by o.OrderDate.Value.Year into g

                     select new
                     {

                         Year = g.Key,
                         Count = g.Count()
                     }).OrderBy(q2 => q2.Year);

            dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var q = (from o in dbContext.Orders
                     group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g

                     select new
                     {
                         Year = g.Key,
                         Count = g.Count()
                     }).OrderBy(q2 => q2.Year);

            dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = (from o in dbContext.Order_Details.AsEnumerable()
                    group o by o.OrderID into g
                    select new
                    {
                        OrderId = g.Key,
                        Count = g.Count(),
                        Total = $"{g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)):c2}"
                    }).OrderBy(p=>p.OrderId);
            dataGridView1.DataSource = q.ToList();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Top5
            var q = (from o in dbContext.Order_Details.AsEnumerable()
                    group o by new { o.Order.EmployeeID, o.Order.Employee.FirstName,o.Order.Employee.LastName } into g
                    //orderby g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)) descending
                    select new
                    {
                        EmployeeName = g.Key.FirstName + " "+g.Key.LastName,
                        EmployeeID = g.Key.EmployeeID,
                        count = g.Count(),
                        Total = $"{g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)):c2}"   
                    }).OrderByDescending(p => decimal.Parse(p.Total,System.Globalization.NumberStyles.Currency)).Take(5);
            dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var q = (from o in dbContext.Order_Details.AsEnumerable()
                     group o by o.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Year = g.Key,
                         Count = g.Count(),
                         Total = g.Sum(p=> p.UnitPrice*p.Quantity* (decimal)(1 - p.Discount))
                     }).OrderBy(p => p.Year);
            dataGridView1.DataSource = q.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var q = (from c in dbContext.Categories
                     from p in c.Products
                     orderby p.UnitPrice descending
                     select new {p.ProductName, p.UnitPrice , c.CategoryID, c.CategoryName }).Take(5);

            dataGridView1.DataSource = q.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            bool result2 = dbContext.Products.Any(p1 => p1.UnitPrice > 300);

            if (result2) MessageBox.Show(result2.ToString() + "有大於300的~");
            else MessageBox.Show(result2.ToString()+" 沒有大於300的~");
               
            //var q = from p in dbContext.Products
            //        select p.UnitPrice;
            //q.ToList();
            //bool result;
            //result = q.Any(a => a > 300);

        }
    }
}
