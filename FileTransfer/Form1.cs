using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FileTransfer
{
    public partial class Form1 : Form
    {
        #region 属性 API特效窗体显示和隐藏

        /// <summary>
        /// //从左到右
        /// </summary>
        public const Int32 AW_HOR_LEFT_RIGHT = 0x00000001;
        /// <summary>
        /// 从右到左
        /// </summary>
        private const Int32 AW_HOR_RIGHT_LEFT = 0x00000002;
        /// <summary>
        /// 从上到下
        /// </summary>
        private const Int32 AW_VER_UP_DOWN = 0x00000004;
        /// <summary>
        /// 从下到上
        /// </summary>
        private const Int32 AW_VER_DOWN_UP = 0x00000008;
        /// <summary>
        /// 从中间到四周
        /// </summary>
        private const Int32 AW_CENTER = 0x00000010;
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        private const Int32 AW_HIDE = 0x00010000;
        /// <summary>
        /// 显示窗口
        /// </summary>
        private const Int32 AW_ACTIVATE = 0x00020000;
        /// <summary>
        /// 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        /// </summary>
        private const Int32 AW_SLIDE = 0x00040000;
        /// <summary>
        /// 改变透明度
        /// </summary>
        private const Int32 AW_BLEND = 0x00080000;

        /// <summary>
        /// 特效花费时间 单位：毫秒
        /// </summary>
        private int _speed = 500;

        [DllImport("user32.dll")]
        public static extern void AnimateWindow(IntPtr hwnd, int stime, int style);//显示效果

        /// <summary>
        /// 鼠标坐标
        /// </summary>
        private Point _cursorPoint;

        //API获取鼠标坐标
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        //线程暂停时间 单位：毫秒
        private int _timespan = 1000;
        private System.Threading.Timer _timer;
        private delegate void LoadListDelegate();
        private LoadListDelegate _loaddelegate;

        /// <summary>
        /// 窗体是否显示，true——显示、false——隐藏
        /// </summary>
        private bool _isActive = true;

        /// <summary>
        /// 停靠在边缘时，显示窗体的宽度
        /// </summary>
        private const int _smallX = 5;

        #endregion
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;//取消最大化按钮
            this.MinimizeBox = false;//取消最小化按钮
            this.ShowInTaskbar = false;//任务栏不显示窗体图标
            this.TopMost = false;//设置窗体总是显示在最前面

            _loaddelegate = LoadControl;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //设置窗体显示位置 右下角
            int workY = Screen.PrimaryScreen.WorkingArea.Height - Height;
            int X = Screen.PrimaryScreen.Bounds.Width - Width;
            this.Location = new Point(X, workY);

            //窗体打开的时候就开始计时器
            BeginTimer();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //从右到左滑动
            AnimateWindow(this.Handle, _speed, AW_HOR_RIGHT_LEFT | AW_SLIDE | AW_ACTIVATE);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Dispose();
            //淡出效果
            AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE);
        }

        //重写WndProc方法，禁止拖动和双击标题栏最大化
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x231)
            {
                this.SuspendLayout();
            }
            else if (m.Msg == 0x232)
            {
                this.ResumeLayout();
            }
            else if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)//禁止拖动
            {
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        private void SetHide()
        {
            if (_isActive)
            {
                AnimateWindow(this.Handle, _speed, AW_HOR_LEFT_RIGHT | AW_SLIDE | AW_HIDE);

                int X = Screen.PrimaryScreen.Bounds.Width - _smallX;
                int Y = this.Location.Y;
                this.Location = new Point(X, Y);

                AnimateWindow(this.Handle, 10, AW_BLEND | AW_ACTIVATE);
                _isActive = false;
            }
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        private void SetActivate()
        {
            if (!_isActive)
            {
                AnimateWindow(this.Handle, 10, AW_BLEND | AW_HIDE);

                int X = Screen.PrimaryScreen.Bounds.Width - Width;
                int Y = this.Location.Y;
                this.Location = new Point(X, Y);

                AnimateWindow(this.Handle, _speed, AW_HOR_RIGHT_LEFT | AW_SLIDE | AW_ACTIVATE);
                _isActive = true;
            }
        }

        private void LoadControl()
        {
            #region 控制窗体显示和隐藏
            //获取当前鼠标坐标
            GetCursorPos(out _cursorPoint);
            //根据 窗体当前状态，判断窗体接下来是显示还是隐藏。
            if (_isActive)
            {
                //当前窗体为显示，则接下来是隐藏
                //如果鼠标坐标不在窗体范围内，则设置窗体隐藏，否则不处理
                if (_cursorPoint.X < this.Location.X || _cursorPoint.Y < this.Location.Y)
                {
                    SetHide();
                }
            }
            else
            {
                //当前窗体为隐藏，则接下来是显示
                //如果鼠标坐标在窗体范围内，则设置窗体显示，否则不处理
                if (_cursorPoint.X >= this.Location.X && _cursorPoint.Y >= this.Location.Y)
                {
                    SetActivate();
                }
            }
            #endregion
        }

        private void BeginTimer()
        {
            TimerCallback tcBack = new TimerCallback(InvokTimer);
            _timer = new System.Threading.Timer(tcBack, null, 5000, _timespan);
        }

        private void InvokTimer(object state)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(_loaddelegate);
            }
        }
        frmFile frmFile;
        private void button1_Click(object sender, EventArgs e)
        {
            if (frmFile == null)
                frmFile = new frmFile();
            this.Visible = false;
            frmFile.Show();
        }
    }
}
