﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VSBuilder.Views
{
    /// <summary>
    /// コピーファイル設定追加画面クラス
    /// </summary>
    public partial class CopyFileSettingWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CopyFileSettingWindow()
        {
            InitializeComponent();

            DataContext = (App.Current as App)?.ViewModelLocator.CopyFileSettingVM;
        }
    }
}
