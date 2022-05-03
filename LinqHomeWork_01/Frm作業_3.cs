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
            TreeNode node=null;

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
                TreeNode node = treeView1.Nodes.Add(group.Mykey.ToString(),s);
                foreach(var item in group.Mygroup)
                {
                    string a = (item.Length).ToString();
                    node.Nodes.Add(item.ToString()+ " Size = "+a);
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
    }
}
