using System.Windows;
using Beauty.SerialPorts;
using Beauty.Tool;

namespace Beauty
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            //增加过期时间

            myTitle.Children.Add(new HospitalTitle());
            myContent.Children.Add(new Content());

            Inputs.Click += (s, e) =>
            {
                myContent.Children.Clear();
                myContent.Children.Add(new Content());
            };

            UserSetting.Click += (s, e) =>
            {
                myContent.Children.Clear();
                myContent.Children.Add(new UserSetting());
            };
            InterFaceSetting.Click += (s, e) =>
            {
                myContent.Children.Clear();
                myContent.Children.Add(new InterFaceSetting());
            };

            RegisterSoftware.Click += (s, e) =>
            {
                new Register().Show();
                this.Close();
            };
            AboutMe.Click += (s, e) => MessageBox.Show("如果有问题，请联系QQ:26977801。");
            Exit.Click += (s, e) => Close();

            //开启从Prisca返回的文件的监控
            new FileMonitoring();
        }

        private void InitTangs()
        {
            //btnRun.Click += (s, e) =>
            //{
            //    SerialPortUtil serial = new SerialPortUtil();
            //    serial.DataReceived += new DataReceivedEventHandler(serial_DataReceived);
            //    serial.Error += new System.IO.Ports.SerialErrorReceivedEventHandler(serial_Error);
            //};
        }
        /// <summary>
        /// 如果出现错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serial_Error(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        /// <summary>
        /// 接收返回的数据
        /// </summary>
        /// <param name="e"></param>
        void serial_DataReceived(DataReceivedEventArgs e)
        {
            string readData = e.DataReceived;
        }
    }
}
