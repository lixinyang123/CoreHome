using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CoreHome.Infrastructure.Models
{
    public class StaticConfig<ConfigType>
    {
        private readonly ConfigType initConfig;
        private readonly string configPath;
        private readonly string configFile;

        /// <summary>
        /// 初始化静态配置
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="InitConfig">初始化配置</param>
        public StaticConfig(string fileName, ConfigType initConfig)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                configPath = @"C:/Server/CoreHome/";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                configPath = @"/home/Server/CoreHome/";
            }

            configFile = Path.Combine(configPath, fileName);

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            this.initConfig = initConfig;
        }

        public ConfigType Config
        {
            get
            {
                try
                {
                    return JsonSerializer.Deserialize<ConfigType>(File.ReadAllTextAsync(configFile).Result);
                }
                catch (System.Exception)
                {
                    ResetConfig();
                    return initConfig;
                }
            }
            set
            {
                File.WriteAllTextAsync(configFile, JsonSerializer.Serialize(value));
            }
        }

        public void ResetConfig()
        {
            Config = initConfig;
        }
    }
}
