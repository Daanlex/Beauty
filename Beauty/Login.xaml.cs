using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Beauty.DataAccess;
using Beauty.Model;
using Beauty.Tool;

namespace Beauty
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            //DateTime dt = Convert.ToDateTime("2013-7-25");
            //TimeSpan ts = DateTime.Now.Subtract(dt);
            //string aa = ts.Days / 7 + "周" + ts.Days % 7 + "天";
            //dt.AddDays(280);
            //Encrypt.EncryptDES("120");


            var initlogin = new Initializationdb();

            //程序启动的时候先判断数据库是否初始化了
            initlogin.IsInit();

            //程序启动的时候判断当天是否已经备份了数据库
            initlogin.Backdb();

            //程序启动的时候判断是否还有试用时间或者注册了
            var registCode = new RegistCodeDAL().GetRegistCode();
            if (string.IsNullOrEmpty(registCode.Code))
            {
                //先判断过期天数是否为0
                if (registCode.SurplusDays != "0")
                {
                    //判断还有没有试用时间
                    DateTime expiredTime =
                        Convert.ToDateTime(registCode.FirstTime).AddDays(Convert.ToDouble(registCode.SurplusDays));

                    if (expiredTime < DateTime.Now)
                    {
                        //过期了，修改试用天数为0，然后提示注册
                        new RegistCodeDAL().UploadTryDays();

                        MessageBox.Show("您的软件在已到期,请注册!");
                        new Register().Show();
                        Close();
                    }
                    else
                    {
                        //程序启动的时候先判断数据库是否初始化了
                        new Initializationdb().IsInit();
                        InitializeComponent();
                        InitControl();
                    }
                }
                else
                {
                    //如果天数为0了，那么就提示要注册了
                    MessageBox.Show("您的软件在已到期,请注册!");
                    new Register().Show();
                    Close();
                }
            }
            else
            {
                //验证注册码是否正确
                bool flag = Encrypt.VerificationRegistCode(registCode.Code);
                if (!flag)
                {
                    MessageBox.Show("注册码不正确,请重新注册");
                    new Register().Show();
                    Close();
                }
                else
                {
                    InitializeComponent();
                    InitControl();
                }
            }

        }
        private void InitControl()
        {
            btnLogin.Click += (s, e) =>
            {
                if (!new LoginDAL().VerificationUser(new User { UserName = UserName.Text.Trim(), PassWord = PassWord.Password }))
                    MessageBox.Show(ResourceHelper.GetStaticResource("LoginError"));
                else
                {
                    new MainWindow().Show();
                    Close();
                }
            };
            PassWord.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter)
                        btnLogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                };

            //启动程序
            //new PriscaConnect().AutoOpenPriscaConnect();
        }
    }
}
