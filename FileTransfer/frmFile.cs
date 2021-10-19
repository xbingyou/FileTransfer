using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransfer
{
    public partial class frmFile : Form
    {
        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;
        /// <summary>
        /// 文件路径
        /// </summary>
        private string filePath;
        /// <summary>
        /// 文件大小
        /// </summary>
        private long fileSize;
        public frmFile()
        {
            InitializeComponent();
            this.textBox1.LostFocus += new EventHandler(TextBox1_LostFocus);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            Thread.CurrentThread.IsBackground = true;
            string strIp = IpUtil.GetLocalIp();
            int nPort = IpUtil.GetRandomPort();
            textBox2.Text = strIp;
            textBox3.Text = nPort.ToString();
            label1.Text = "您的ip:" + strIp + " 您的端口:" + nPort;
            var s = new FileRecive(this, strIp, nPort);
            new Thread(s.run).Start();
        }
        int i = 0;
        void TextBox1_LostFocus(object sender, EventArgs e)
        {
            i ++;
            label1.Text = i.ToString();
        }
        /// <summary>
        /// 信息提示框
        /// </summary>
        /// <param name="msg"></param>
        public void Tip(string msg)
        {
            MessageBox.Show(msg, "温馨提示");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dig = new OpenFileDialog();

            if (dig.ShowDialog() == DialogResult.OK)
            {
                //获取文件名
                this.fileName = dig.SafeFileName;

                //获取文件路径
                this.filePath = dig.FileName;

                FileInfo f = new FileInfo(this.filePath);

                //获取文件大小
                this.fileSize = f.Length;
                textBox1.Text = filePath;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ip = textBox2.Text;
            string port = textBox3.Text;

            if (fileName.Length == 0)
            {

                Tip("请选择文件");
                return;
            }
            if (ip.Length == 0 || port.ToString().Length == 0)
            {

                Tip("端口和ip地址是必须的!");
                return;
            }

            var c = new FileSend(this, new string[] { ip, port, fileName, filePath, fileSize.ToString() });
            new Thread(c.Send).Start();
        }
        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value"></param>
        public void UpDateProgress(int value)
        {
            this.progressBar1.Value = value;
            this.label2.Text = value + "%";
            System.Windows.Forms.Application.DoEvents();
        }
        private delegate void SetTextCallback(string text);
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string state)
        {
            if (this.label5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetState);
                this.Invoke(d, new object[] { state });
            }
            else
            {
                label5.Text = state;
            }
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        public void Exit()
        {

            Application.Exit();
        }

        private void frmFile_Click(object sender, EventArgs e)
        {
            this.label1.Focus();
        }
        Form1 form1 = new Form1();

        private void button3_Click(object sender, EventArgs e)
        {
            form1.Show();
            this.Hide();
        }
    }
}
