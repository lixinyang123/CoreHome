using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CoreHome.Infrastructure.Models
{
    public class StaticConfig<ConfigType>
    {
        private readonly string configPath;
        private readonly string configFile;

        public ConfigType Config
        {
            get
            {
                return JsonSerializer.Deserialize<ConfigType>(File.ReadAllText(configFile));
            }
            set
            {
                File.WriteAllText(configFile, JsonSerializer.Serialize(value));
            }
        }

        /// <summary>
        /// 初始化静态配置
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="InitConfig">初始化配置</param>
        public StaticConfig(string fileName,ConfigType InitConfig)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                configPath = @"C:/Server/CoreHome/";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                configPath = @"/home/Server/CoreHome/";
            }

            configFile = configPath + fileName;

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (!File.Exists(configFile))
            {
                Config = InitConfig;
            }
        }
    }
}
