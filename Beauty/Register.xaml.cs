using System.Threading.Tasks;
using System.Windows;
using Beauty.Tool;
using Beauty.DataAccess;

namespace Beauty
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register
    {
        public Register()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            tbCpu.Text = Common.GetCpu();
            //判断是否已经注册
            var registCodeMode = new RegistCodeDAL().GetRegistCode();
            if (Encrypt.VerificationRegistCode(registCodeMode.Code))
            {
                registeredState.Visibility = System.Windows.Visibility.Visible;
                tbRegistCode.Text = registCodeMode.Code;
                btnRegist.IsEnabled = false;
            }

            btnRegist.Click += (s, e) =>
            {
                string registCode = tbRegistCode.Text.Trim();
                //先验证一下是否正确
                if (Encrypt.VerificationRegistCode(registCode))
                {
                    //插入数据库
                    bool flag = new RegistCodeDAL().Register(registCode);
                    if (flag)
                    {
                        MessageBox.Show("注册成功");
                        new Login().Show();
                        Close();
                    }
                    else
                        MessageBox.Show("注册失败");
                }
                else
                    MessageBox.Show("注册码不正确");
            };
        }
    }
}
