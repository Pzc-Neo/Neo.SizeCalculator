using System;
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
using Wpf.Ui.Controls;

namespace Neo.SizeCalculator
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : FluentWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
        }
        public SettingWindow(string sizePresetStr, string sourceSizePresetStr)
        {
            InitializeComponent();
            textBox_PresetSize.Text = sizePresetStr;
            textBox_SourcePresetSize.Text = sourceSizePresetStr;
            textBox_PresetSize.Focus();
        }
        public delegate void SetSizePresetsDelegate(string sizePresetsStr, string sourceSizePresetStr);

        public SetSizePresetsDelegate setSizePreset;
        public void SetSizePresets(string sizePresetsStr, string sourceSizePresetStr)
        {
            setSizePreset(sizePresetsStr, sourceSizePresetStr);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            SetSizePresets(textBox_PresetSize.Text, textBox_SourcePresetSize.Text);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            textBox_PresetSize.Text = "512,682,768,1024";
        }
    }
}
