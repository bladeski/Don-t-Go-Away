using Core_Logic.Config;
using System.IO;
using Xunit;
using Core_Logic.Domain.Interfaces;
using Core_Logic.Application.Services;

public class ConfigLoaderExceptionTests
{
    [Fact]
    public void Load_ReturnsDefault_OnCorruptFile()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, "{not valid json}");
        IConfigLoader configLoader = (IConfigLoader)new ConfigLoader();
        var config = configLoader.Load<DriftConfig>(path);
        Assert.NotNull(config);
        File.Delete(path);
    }

    [Fact]
    public void Save_CreatesFile()
    {
        var path = Path.GetTempFileName();
        File.Delete(path);
        IConfigLoader configLoader = (IConfigLoader)new ConfigLoader();
        var config = new DriftConfig { IdleThresholdMs = 42 };
        configLoader.Save(config, path);
        Assert.True(File.Exists(path));
        File.Delete(path);
    }
}