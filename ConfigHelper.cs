using System;
using System.Configuration;
using System.IO;

namespace Neo.SizeCalculator
{
    internal class ConfigHelper
    {
        public static string GetConfigValue(string key)
        {
            // 获取配置文件路径
            string configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Console.WriteLine("configFilePath: " + configFilePath);

            // 如果配置文件不存在，则返回空字符串
            if (!File.Exists(configFilePath))
                return string.Empty;

            // 读取配置文件中的值
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationElement settings = config.AppSettings.Settings[key];

            return settings != null ? settings.Value : string.Empty;
        }

        public static void SaveConfigValue(string key, string value)
        {
            // 获取配置文件路径
            string configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            // 如果配置文件不存在，则返回
            if (!File.Exists(configFilePath))
                return;

            // 读取配置文件
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            // 更新或添加配置值
            KeyValueConfigurationElement settings = config.AppSettings.Settings[key];
            if (settings != null)
            {
                settings.Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }

            // 保存配置更改
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
