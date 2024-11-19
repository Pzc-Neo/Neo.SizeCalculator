using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace Neo.SizeCalculator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private ViewModels.MainWindowViewModel ViewModelMainWindow;
        public MainWindow()
        {
            InitializeComponent();
            ViewModelMainWindow = new ViewModels.MainWindowViewModel();
            DataContext = ViewModelMainWindow;
        }

        private void LoadSizeFromCliboard(object sender, RoutedEventArgs e)
        {
            // Screenshot
            BitmapSource bs = Clipboard.GetImage();
            if (bs != null)
            {
                ViewModelMainWindow.SourceWidth = Convert.ToInt16(bs.Width);
                ViewModelMainWindow.SourceHeight = Convert.ToInt16(bs.Height);
                return;
            }

            // Image File
            if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();
                if (files.Count > 0)
                {
                    ReadAndSetImageInfoToUI(files[0]);
                }
            }
        }

        private void ReadAndSetImageInfoToUI(string file)
        {
            if (file == null) return;
            try
            {

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(file);
                image.EndInit();
                ViewModelMainWindow.SourceWidth = Convert.ToInt16(image.Width);
                ViewModelMainWindow.SourceHeight = Convert.ToInt16(image.Height);
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("剪切板里面的不是图片，错误信息=> " + error.Message, "不是图片");
            }
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    ReadAndSetImageInfoToUI(files[0]);
                }
            }
        }


        private void textBox_resultWidth_TextChanged(object sender, TextChangedEventArgs e)
        {

            ViewModelMainWindow.IsResultWidthBoxChanged = true;
        }

        private void textBox_resultHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModelMainWindow.IsResultWidthBoxChanged = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((ComboBox)sender).SelectedItem ?? ((ComboBox)sender).SelectionBoxItem;
            var width = item.ToString();
            ViewModelMainWindow.IsResultWidthBoxChanged = true;
            ViewModelMainWindow.ResultWidth = Convert.ToDouble(width);

        }
        private void SetSizePreset(string sizePresetsStr, string sourceSizePresetsStr)
        {
            ViewModelMainWindow.SetSizePresets(sizePresetsStr);
            ViewModelMainWindow.SetSourceSizePresets(sourceSizePresetsStr);
        }

        private void SettingButtonClick(object sender, RoutedEventArgs e)
        {
            string sizePresetsStr = string.Join(",", ViewModelMainWindow.SizePresets);
            string sourceSizePresetsStr = string.Join(",", ViewModelMainWindow.SourceSizePresets);
            SettingWindow settingWindow = new SettingWindow(sizePresetsStr, sourceSizePresetsStr)
            {
                Owner = this
            };
            settingWindow.setSizePreset = SetSizePreset;
            settingWindow.ShowDialog();
        }

        private void ComboBox_SourceSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((ComboBox)sender).SelectedItem ?? ((ComboBox)sender).SelectionBoxItem;
            var sizesStr = item.ToString();
            var sizes = sizesStr.Split('x').Select(int.Parse).ToArray();
            ViewModelMainWindow.SourceWidth = sizes[0];
            ViewModelMainWindow.SourceHeight = sizes[1];
        }
    }
}
