using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CbTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.clbAllA.MultiColumn = true;
            this.clbAllA.CheckOnClick = true;
            this.clbAllA.ColumnWidth = 80;
            this.clbAllA.Items.Add("A1");
            this.clbAllA.Items.Add("A2");
            this.clbAllA.Items.Add("A3");
            this.clbAllA.Items.Add("A4");
            this.clbAllA.Items.Add("A5");
            this.clbAllA.Items.Add("A6");
            this.clbAllA.Items.Add("A7");
            this.clbAllA.Items.Add("A8");
            ArrayList mylist = new ArrayList();
            mylist.Add(new DictionaryEntry("1213", "ertyyt"));
            mylist.Add(new DictionaryEntry("12133", "wretert"));
            mylist.Add(new DictionaryEntry("121334", "wewyuyertq"));
            mylist.Add(new DictionaryEntry("1213343", "ytu"));
            mylist.Add(new DictionaryEntry("12135421", "tuytku"));
            comboBox1.DataSource = mylist;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.clbAllA.Items.Count; i++)
            {
                this.clbAllA.SetItemChecked(i, cbAllA.Checked);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<string> lst = new List<string>();
            for (int i = 0; i < this.clbAllA.CheckedItems.Count; i++)
            {
                lst.Add(this.clbAllA.CheckedItems[i].ToString());
            }
            if (this.clbAllA.CheckedItems.Count > 0)
            {
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyyMM") + "\\72" + this.clbAllA.CheckedItems[0].ToString() + ".png";
                if (File.Exists(strPath))
                    this.pictureBox1.Image = Image.FromFile(strPath);
                else
                    this.pictureBox1.Image = null;
            }
            else
            {
                this.pictureBox1.Image = null;
            }
        }

        private void clbAllA_MouseEnter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedText.ToString() + "---" + comboBox1.SelectedValue.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyyMM") + "\\72" + this.clbAllA.CheckedItems[0].ToString() + ".png";
            ImgDispose();
            if (File.Exists(strPath))
                File.Delete(strPath);
        }
        /// <summary>
        /// 释放图片文件
        /// </summary>
        private void ImgDispose()
        {
            if (this.pictureBox1.Image != null)
            {
                Image img = pictureBox1.Image;
                this.pictureBox1.Image = null;
                img.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            fileDialogSave.Filter = "(*.pdf)|*.pdf";
            if (fileDialogSave.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.textBox1.Text = "1";
            timer1.Stop();
        }
    }
}
