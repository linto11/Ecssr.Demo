namespace Ecssr.Demo.Common
{
    public class DatabaseSetting
    {
        public bool IsInMemory { get; set; }
        public bool SeedData { get; set; }
        public string ConnectionString { get; set; }
        public string DataFilePath { get; set; }
    }
}
