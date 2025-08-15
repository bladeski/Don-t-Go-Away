using Dont_Go_Away.Config;
using System.IO;
using Xunit;

public class ConfigLoaderExceptionTests
{
    [Fact]
    public void Load_ReturnsDefault_OnCorruptFile()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, "{not valid json}");
        var config = ConfigLoader.Load<DriftConfig>(path);
        Assert.NotNull(config);
        File.Delete(path);
    }

    [Fact]
    public void Save_CreatesFile()
    {
        var path = Path.GetTempFileName();
        File.Delete(path);
        var config = new DriftConfig { IdleThresholdMs = 42 };
        ConfigLoader.Save(config, path);
        Assert.True(File.Exists(path));
        File.Delete(path);
    }
}