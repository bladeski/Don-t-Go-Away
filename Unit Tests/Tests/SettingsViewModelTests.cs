using Dont_Go_Away.Config;
using Dont_Go_Away.ViewModels;
using System.ComponentModel;
using Xunit;

public class SettingsViewModelTests
{
    [Fact]
    public void PropertyChanged_IsRaised_OnSet()
    {
        var config = new DriftConfig();
        var vm = new SettingsViewModel(config);
        string? changed = null;
        vm.PropertyChanged += (s, e) => changed = e.PropertyName;

        vm.IdleThresholdMs = 999;
        Assert.Equal("IdleThresholdMs", changed);
        Assert.Equal(999, config.IdleThresholdMs);

        vm.SimulatedKey = "A";
        Assert.Equal("SimulatedKey", changed);
        Assert.Equal("A", config.SimulatedKey);
    }

    [Fact]
    public void SettingProperties_UpdatesConfig()
    {
        var config = new DriftConfig();
        var vm = new SettingsViewModel(config);

        vm.DriftBoxSize = 250;
        Assert.Equal(250, config.DriftBoxSize);

        vm.StepDelayMs = 75;
        Assert.Equal(75, config.StepDelayMs);

        vm.MaxStep = 5;
        Assert.Equal(5, config.MaxStep);

        vm.ShiftTapChance = 0.5;
        Assert.Equal(0.5, config.ShiftTapChance);
    }

    [Fact]
    public void MultiplePropertyChangedEvents_AreRaised()
    {
        var config = new DriftConfig();
        var vm = new SettingsViewModel(config);
        int eventCount = 0;
        vm.PropertyChanged += (s, e) => eventCount++;

        vm.IdleThresholdMs = 1000;
        vm.DriftBoxSize = 200;
        vm.StepDelayMs = 60;
        vm.MaxStep = 3;
        vm.SimulatedKey = "B";
        vm.ShiftTapChance = 0.2;

        Assert.Equal(6, eventCount);
    }

    [Fact]
    public void Constructor_SetsConfigProperty()
    {
        var config = new DriftConfig { IdleThresholdMs = 555 };
        var vm = new SettingsViewModel(config);
        Assert.Equal(555, vm.IdleThresholdMs);
        Assert.Same(config, vm.Config);
    }
}