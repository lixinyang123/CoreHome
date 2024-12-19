using MemoryPack;

namespace CoreHome.Infrastructure.Models
{
    public class StaticConfig
    {
        public static readonly string STORAGE_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".corehome");
    }

    public class StaticConfig<ConfigType> : StaticConfig
    {
        private readonly ConfigType initConfig;
        private readonly string configFile;

        /// <summary>
        /// 初始化静态配置
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="InitConfig">初始化配置</param>
        public StaticConfig(string fileName, ConfigType initConfig)
        {
            configFile = Path.Combine(STORAGE_FOLDER, fileName);

            if (!Directory.Exists(STORAGE_FOLDER))
            {
                _ = Directory.CreateDirectory(STORAGE_FOLDER);
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
            set => File.WriteAllBytes(configFile, MemoryPackSerializer.Serialize(value));
        }

        public void ResetConfig()
        {
            Config = initConfig;
        }
    }
}
