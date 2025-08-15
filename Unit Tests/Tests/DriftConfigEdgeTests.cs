using Dont_Go_Away.Config;
using Xunit;

public class DriftConfigEdgeTests
{
    [Fact]
    public void CanSetAndGetAllProperties()
    {
        var config = new DriftConfig
        {
            IdleThresholdMs = 1,
            DriftBoxSize = 2,
            StepDelayMs = 3,
            MaxStep = 4,
            ShiftTapChance = 0.5,
            SimulatedKey = "CTRL"
        };

        Assert.Equal(1, config.IdleThresholdMs);
        Assert.Equal(2, config.DriftBoxSize);
        Assert.Equal(3, config.StepDelayMs);
        Assert.Equal(4, config.MaxStep);
        Assert.Equal(0.5, config.ShiftTapChance);
        Assert.Equal("CTRL", config.SimulatedKey);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1000000)]
    public void IdleThresholdMs_AcceptsValues(int value)
    {
        var config = new DriftConfig { IdleThresholdMs = value };
        Assert.Equal(value, config.IdleThresholdMs);
    }
}