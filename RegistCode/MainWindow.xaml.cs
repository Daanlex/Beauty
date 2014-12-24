using System;
using System.Security.Cryptography;

namespace RegistCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            //System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //string publicKey = rsa.ToXmlString(false);
            //string priviteKey = rsa.ToXmlString(true);

            const string priviteKey = @"<RSAKeyValue><Modulus>oqJQ/jhgv0//WDGdqfUZgiecO4Qorb6CPX9DP1+WsRCf7fMCYWwv7AIBLW1XpV9/y+
                        +rIEm6U+NKnJqt+B90ZHHddWYy6hCFOWvmzTS/MZqvg1HrzNZYSXjqp63ENfWJhJGLkVszInSaaADy13selrcd/
                        zs4AhT7+H7RZsV6z1U=</Modulus><Exponent>AQAB</Exponent><P>5BHOJIyxghmaKCso
                        +b/iy9U6gIUD3s4vZPOGDDgyj2392Lj/1K1PjGWt1tY62FrufVY1GiiQkoM05lhl
                        +Ek7bw==</P><Q>to0L7wH0qH2NZy/voVvy/AA8EprYfnHMBaWoY0rCA4k0LepGWALaXTy
                        wEoPLW4OY6YkXI+ukHzy99gaJhExPew==</Q><DP>alQKwWt+jmMVRcjpb5aQS
                        +t7PNPGvCdwXSZTxnqkx83F3TZSv3qVbaUx6Mkz4g5yxahdVXa4ADZ/gSyRTbIGrQ==</D
                        P><DQ>cOOojZVYBxodZ8JtHgwOp2g9vgOj/g3BbXyUNVL1x9oBOqO1/JdHEoGFIO3/xAgN
                        d94fQUdnpymZD5vbTsUiIQ==</DQ><InverseQ>CwnpSpzByHV6/Vc5x/LDfYJf
                        +HPVmQD6vNlfuXHFRkefhIhZTZZUPJkL1/feZiaKszqtpN2vFEmyBJXoV6uYZQ==</Inve
                        rseQ><D>SDiKfQouNGbi/pgx6pp0NG9AmtFqexVhosuT4l2hfonia3mBsh+n/Ec7nJ
                        +0zzRkfPy2YoU7ICuMI5Uw8kGNnqL1I2aEmb5OhVIKHC4p9AnjWMuVpvkviOIk3hF
                        +Svnp/ou+8jv6X0LfOSQXccARO3uoDDYLoxaRPfNQn5YYcdU=</D></RSAKeyValue>";
            btnRegist.Click += (s, e) => 
            {
                 tbRegistCoed.Text = CreateRegistrationCode(priviteKey,tbCpu.Text);
            };
        }

        /// <summary>
        /// 根据密钥生成注册码
        /// </summary>
        /// <returns></returns>
        public static string CreateRegistrationCode(string priviteKey,string cpu)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(priviteKey);
                // 加密对象 
                var f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] source = System.Text.Encoding.ASCII.GetBytes(cpu);
                var sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);

                byte[] b = f.CreateSignature(result);

                return  Convert.ToBase64String(b);
            } 
        }
    }
}
