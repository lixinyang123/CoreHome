using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CoreHome.Infrastructure.Models
{
    //========================================================TODO
    //减少IO次数，提升性能

    public class StaticConfig<ConfigType>
    {
        private readonly ConfigType initConfig;
        private readonly string configPath;
        private readonly string configFile;

        public ConfigType Config
        {
            get
            {
                try
                {
                    return JsonSerializer.Deserialize<ConfigType>(File.ReadAllText(configFile));
                }
                catch (System.Exception)
                {
                    ResetConfig();
                    return initConfig;
                }
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
        public StaticConfig(string fileName,ConfigType initConfig)
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

            this.initConfig = initConfig;
        }

        public void ResetConfig()
        {
            Config = initConfig;
        }
    }
}
