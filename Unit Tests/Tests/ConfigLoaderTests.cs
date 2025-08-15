using System.IO;
using Dont_Go_Away.Config;
using Xunit;

public class ConfigLoaderTests
{
    [Fact]
    public void Load_ReturnsDefault_WhenFileMissing()
    {
        var config = ConfigLoader.Load<DriftConfig>("nonexistent.json");
        Assert.NotNull(config);
        Assert.Equal(120000, config.IdleThresholdMs);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_Works()
    {
        var path = Path.GetTempFileName();
        var original = new DriftConfig { IdleThresholdMs = 12345, SimulatedKey = "A" };
        ConfigLoader.Save(original, path);

        var loaded = ConfigLoader.Load<DriftConfig>(path);
        Assert.Equal(12345, loaded.IdleThresholdMs);
        Assert.Equal("A", loaded.SimulatedKey);

        File.Delete(path);
    }
}