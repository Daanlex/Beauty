using System;
using System.Collections.Generic;
using System.Windows;
using Beauty.DataAccess;
using Beauty.Model;

namespace Beauty
{
    /// <summary>
    /// UserSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UserSetting 
    {
        public UserSetting()
        {
            InitializeComponent();
            InitControl();
            InitEvent();
        }
        private void InitControl()
        {
            dgUserSetting.ItemsSource = GetData();
        }

        private void InitEvent()
        {
            dgUserSetting.SelectionChanged += (s, e) =>
            {
                var u=(UserSettingMd) dgUserSetting.SelectedValue;
                if(u!=null)
                    DataPacking(u);
            };

            btnUpdate.Click += (s, e) => Update();
        }

        /// <summary>
        /// 返回所有的默认值数据
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UserSettingMd> GetData()
        {
            return new UserSettingDAL().GetUserSetting();
        }

        private void Update()
        {
            if (tbDefaultValueNo.Text.Trim() != "")
            {
                bool flag = new UserSettingDAL().Update(DataPacking());
                if (flag)
                {
                    MessageBox.Show("修改成功");
                    InitControl();
                }
                else
                    MessageBox.Show("修改失败");
            }
            else
                MessageBox.Show("请先选择数据再修改");

            
        }


        /// <summary>
        /// 打包数据
        /// </summary>
        /// <returns></returns>
        private UserSettingMd DataPacking()
        {
            var u = new UserSettingMd
                {
                    DefaultValueNo = Convert.ToInt64(tbDefaultValueNo.Text.Trim()),
                    DefaultValueName = tbDefaultValueName.Text.Trim(),
                    UpperValueOrDefaultValue = tbUpperValueOrDefaultValue.Text.Trim(),
                    LowerValue = tbLowerValue.Text.Trim(),
                    Reserved = tbReserved.Text.Trim()
                };
            return u;
        }

        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="u">UserSettingMd 对象</param>
        private void DataPacking(UserSettingMd u)
        {
            tbDefaultValueNo.Text = u.DefaultValueNo.ToString();
            tbDefaultValueName.Text = u.DefaultValueName;
            tbUpperValueOrDefaultValue.Text = u.UpperValueOrDefaultValue;
            tbLowerValue.Text = u.LowerValue;
            tbReserved.Text = u.Reserved;
        }

    }
}
