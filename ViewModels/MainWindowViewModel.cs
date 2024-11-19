using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Neo.SizeCalculator.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnProertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            SourceWidth = 1080;
            SourceHeight = 1920;
            LoadSetting();
        }
        public void LoadSetting()
        {

            var sizePresetsStr = ConfigHelper.GetConfigValue("sizePresets");
            if (sizePresetsStr == string.Empty)
            {
                SetDefaultSizePreset();
            }
            else
            {
                try
                {
                    SizePresets = sizePresetsStr.Split(',').Select(int.Parse).ToArray();
                }
                catch (Exception)
                {
                    SetDefaultSizePreset();
                }
            }
            if (SizePresets.Length > 0)
            {
                ResultWidth = SizePresets[0];
            }

            var sourceSizePresetsStr = ConfigHelper.GetConfigValue("sourceSizePresets");
            if (sourceSizePresetsStr == string.Empty)
            {
                SetDefaultSourceSizePreset();
            }
            else
            {
                try
                {
                    SourceSizePresets = sourceSizePresetsStr.Split(',').ToArray();
                }
                catch (Exception)
                {
                    SetDefaultSourceSizePreset();
                }
            }
            if (SourceSizePresets.Length > 0)
            {
                var sizes = SourceSizePresets[0].Split('x').Select(int.Parse).ToArray<int>();
                SourceWidth = sizes[0];
                SourceHeight = sizes[1];
            }
        }
        private void SetDefaultSizePreset()
        {
            int[] sizes = { 512, 682, 768, 1024 };
            ConfigHelper.SaveConfigValue("sizePresets", string.Join(",", sizes));
            SizePresets = sizes;
        }
        private void SetDefaultSourceSizePreset()
        {
            string[] sizes = { "512x768", "768x1024" };
            ConfigHelper.SaveConfigValue("sourceSizePresets", string.Join(",", sizes));
            SourceSizePresets = sizes;
        }
        private int[] sizePresets;
        public int[] SizePresets
        {
            get { return sizePresets; }
            set
            {
                sizePresets = value;
                OnProertyChanged(nameof(SizePresets));
            }
        }

        private string[] sourceSizePresets;

        public string[] SourceSizePresets
        {
            get { return sourceSizePresets; }
            set
            {
                sourceSizePresets = value;
                OnProertyChanged(nameof(SourceSizePresets));
            }
        }

        public void SetSizePresets(string sizePresetsStr)
        {
            Regex regex = new Regex(@"\d+");
            var matchs = regex.Matches(sizePresetsStr);
            if (matchs.Count > 0)
            {
                int[] sizes = new int[matchs.Count];
                for (int i = 0; i < matchs.Count; i++)
                {
                    sizes[i] = int.Parse(matchs[i].Value);
                }
                SizePresets = sizes;
                ConfigHelper.SaveConfigValue("sizePresets", string.Join(",", sizes));
            }
        }
        public void SetSourceSizePresets(string sourceSizePresetsStr)
        {
            Regex regex = new Regex(@"\d+?x\d+");
            var matchs = regex.Matches(sourceSizePresetsStr);
            if (matchs.Count > 0)
            {
                string[] sizes = new string[matchs.Count];
                for (int i = 0; i < matchs.Count; i++)
                {
                    sizes[i] = matchs[i].Value;
                }
                SourceSizePresets = sizes;
                ConfigHelper.SaveConfigValue("sourceSizePresets", string.Join(",", sizes));
            }
        }


        private bool isResultWidthBoxChanged;

        public bool IsResultWidthBoxChanged
        {
            get { return isResultWidthBoxChanged; }
            set
            {
                isResultWidthBoxChanged = value;
                OnProertyChanged(nameof(IsResultWidthBoxChanged));
            }
        }

        private double sourceWidth;
        public double SourceWidth
        {
            get { return sourceWidth; }
            set
            {
                sourceWidth = value;
                if (sourceHeight != 0)
                {
                    if (resultWidth != 0)
                    {
                        ResultHeight = Convert.ToInt16(Convert.ToDouble(ResultWidth) / Convert.ToDouble(SourceWidth) * Convert.ToDouble(SourceHeight));
                    }
                }
                OnProertyChanged(nameof(SourceWidth));
            }
        }

        private double sourceHeight;

        public double SourceHeight
        {
            get { return sourceHeight; }
            set
            {
                sourceHeight = value;
                if (SourceHeight != 0 && SourceWidth != 0)
                {
                    ResultHeight = Convert.ToInt16(Convert.ToDouble(ResultWidth) / Convert.ToDouble(SourceWidth) * Convert.ToDouble(SourceHeight));
                }
                OnProertyChanged(nameof(SourceHeight));
            }
        }

        private double resultWidth;
        public double ResultWidth
        {
            get { return resultWidth; }
            set
            {
                resultWidth = value;
                if (IsResultWidthBoxChanged)
                {
                    if (SourceWidth != 0 && SourceHeight != 0 && value != 0)
                    {
                        ResultHeight = Convert.ToInt16(Convert.ToDouble(ResultWidth) / Convert.ToDouble(SourceWidth) * Convert.ToDouble(SourceHeight));
                    }
                    isResultWidthBoxChanged = false;
                }
                OnProertyChanged(nameof(ResultWidth));
            }
        }

        private double resultHeight;

        public double ResultHeight
        {
            get { return resultHeight; }
            set
            {
                resultHeight = value;
                if (!isResultWidthBoxChanged)
                {
                    if (SourceWidth != 0 && SourceHeight != 0 && value != 0)
                    {
                        ResultWidth = Convert.ToInt16(Convert.ToDouble(SourceWidth) / Convert.ToDouble(SourceHeight) * Convert.ToDouble(ResultHeight));
                        IsResultWidthBoxChanged = false;
                    }
                }
                OnProertyChanged(nameof(ResultHeight));
            }
        }

    }
}
