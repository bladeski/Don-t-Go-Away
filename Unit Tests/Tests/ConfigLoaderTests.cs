using System.IO;
using Core_Logic.Config;
using Core_Logic.Domain.Interfaces;
using Xunit;

public class ConfigLoaderTests
{
    [Fact]
    public void Load_ReturnsDefault_WhenFileMissing()
    {
        IConfigLoader configLoader = new ConfigLoader();
        var config = configLoader.Load<DriftConfig>("nonexistent.json");
        Assert.NotNull(config);
        Assert.Equal(120000, config.IdleThresholdMs);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_Works()
    {
        IConfigLoader configLoader = new ConfigLoader();
        var path = Path.GetTempFileName();
        var original = new DriftConfig { IdleThresholdMs = 12345, SimulatedKey = "A" };
        configLoader.Save(original, path);

        var loaded = configLoader.Load<DriftConfig>(path);
        Assert.Equal(12345, loaded.IdleThresholdMs);
        Assert.Equal("A", loaded.SimulatedKey);

        File.Delete(path);
    }
}