using MemoryPack;
using System.Runtime.InteropServices;

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
                _ = Directory.CreateDirectory(configPath);
            }

            this.initConfig = initConfig;
        }

        public ConfigType Config
        {
            get
            {
                try
                {
                    return MemoryPackSerializer.Deserialize<ConfigType>(File.ReadAllBytesAsync(configFile).Result);
                }
                catch (Exception)
                {
                    ResetConfig();
                    return initConfig;
                }
            }
            set => File.WriteAllBytesAsync(configFile, MemoryPackSerializer.Serialize(value)).Wait();
        }

        public void ResetConfig()
        {
            Config = initConfig;
        }
    }
}
