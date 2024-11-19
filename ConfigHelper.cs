using System;
using System.Configuration;
using System.IO;

namespace Neo.SizeCalculator
{
    internal class ConfigHelper
    {
        // 获取配置文件路径的私有方法，避免重复代码
        private static string GetConfigFilePath()
        {
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        }

        // 获取配置值的方法
        public static string GetConfigValue(string key)
        {
            string configFilePath = GetConfigFilePath();

            // 检查配置文件是否存在，如果不存在则创建一个新的配置文件并使用默认值
            if (!File.Exists(configFilePath))
            {
                CreateDefaultConfigFile(configFilePath);
            }

            // 读取配置文件中的值
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationElement settings = config.AppSettings.Settings[key];

            return settings != null ? settings.Value : string.Empty;
        }

        // 保存配置值的方法
        public static void SaveConfigValue(string key, string value)
        {
            string configFilePath = GetConfigFilePath();

            // 检查配置文件是否存在，如果不存在则创建一个新的配置文件并使用默认值
            if (!File.Exists(configFilePath))
            {
                CreateDefaultConfigFile(configFilePath);
            }

            // 读取配置文件
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            // 更新或添加配置值
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }

            // 保存配置更改
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        // 创建默认配置文件的方法
        private static void CreateDefaultConfigFile(string configFilePath)
        {
            // 创建一个新的配置文件对象
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = null; // 确保没有外部文件引用

            // 添加默认配置值
            // 这里可以根据需要添加更多的默认配置项
            config.AppSettings.Settings.Add("sizePresets", "512,682,768,1024");
            config.AppSettings.Settings.Add("sourceSizePresets", "512x768,768x1024");

            // 保存配置文件
            config.SaveAs(configFilePath, ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}