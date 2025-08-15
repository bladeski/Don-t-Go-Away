using Dont_Go_Away.Config;
using Xunit;

public class DriftConfigTests
{
    [Fact]
    public void Defaults_AreCorrect()
    {
        var config = new DriftConfig();
        Assert.Equal(120000, config.IdleThresholdMs);
        Assert.Equal(500, config.DriftBoxSize);
        Assert.Equal(50, config.StepDelayMs);
        Assert.Equal(2, config.MaxStep);
        Assert.Equal(0.01, config.ShiftTapChance);
        Assert.Equal("SHIFT", config.SimulatedKey);
    }
}