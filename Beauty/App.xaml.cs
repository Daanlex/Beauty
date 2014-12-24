using System.Windows;
using System.Text;
using Beauty.Tool;

namespace Beauty
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public App()
        {
            Init();
        }
        private void Init()
        {
            //捕获全局异常
            DispatcherUnhandledException += (s, e) =>
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("应用程序出现了异常:"+ e.Exception.Message);
                stringBuilder.AppendLine("错误跟踪:"+ e.Exception.StackTrace);
                stringBuilder.AppendLine("============================================================================================");
                Common.CreateFile(ConfigString.dberrorlog);
                Common.WriterErrorLog(stringBuilder.ToString(), ConfigString.dberrorlog);
                MessageBox.Show("程序出现异常");
                e.Handled = true;
            };
        }
    }
}
