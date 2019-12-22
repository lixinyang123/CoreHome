namespace Infrastructure.Models
{
    public enum BgmType { None, Single, Random, Web }

    public class Bgm
    {
        public BgmType BgmType { get; set; } = BgmType.None;

        public string DefaultMusic { get; set; }

        public string Url { get; set; }
    }
}
