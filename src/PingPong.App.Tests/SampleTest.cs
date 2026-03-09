namespace PingPong.App.Tests;

public class SampleTest
{
    [Fact]
    public void SampleTest_AlwaysPasses()
    {
        // Arrange
        var expected = 42;

        // Act
        var actual = 42;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SampleTest_BasicMath()
    {
        Assert.Equal(4, 2 + 2);
    }
}
