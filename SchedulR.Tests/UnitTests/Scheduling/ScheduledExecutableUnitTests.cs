﻿using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SchedulR.Scheduling;

namespace SchedulR.Tests.UnitTests.Scheduling;

public class ScheduledExecutableUnitTests
{
    private readonly Type _testExecutableType;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScheduledExecutableUnitTests()
    {
        _testExecutableType = Substitute.For<Type>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
    }

    [Fact]
    public void IsDue_WhenNextExecutionTimeIsLessThanNow_ReturnsTrue()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var scheduledExecutable = new ScheduledExecutable(_testExecutableType, _serviceScopeFactory);

        scheduledExecutable.EveryMinutes(1); // Set the interval to 1 minute
        scheduledExecutable.ExecutedAt(now.AddSeconds(-61)); // Set the last execution time to more than 1 minute ago

        // Act
        var result = scheduledExecutable.IsDue(now);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public void IsDue_WhenNextExecutionTimeIsEqualToNow_ReturnsTrue()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var scheduledExecutable = new ScheduledExecutable(_testExecutableType, _serviceScopeFactory);

        scheduledExecutable.EveryMinutes(1); // Set the interval to 1 minute
        scheduledExecutable.ExecutedAt(now.AddSeconds(-60)); // Set the last execution time to exactly 1 minute ago

        // Act
        var result = scheduledExecutable.IsDue(now);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public void IsDue_WhenNextExecutionTimeIsGreaterThanNow_ReturnsFalse()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var scheduledExecutable = new ScheduledExecutable(_testExecutableType, _serviceScopeFactory);

        scheduledExecutable.EveryMinutes(1); // Set the interval to 1 minute
        scheduledExecutable.ExecutedAt(now.AddSeconds(-59)); // Set the last execution time to less than 1 minute ago

        // Act
        var result = scheduledExecutable.IsDue(now);

        // Assert
        result.Should().BeFalse();
    }
}
