using Algorithms.Api;

namespace Algorithms.Tests.Tasks;

public class FibonacciTests
{
    // Known Fibonacci sequence values for testing
    private static readonly Dictionary<int, long> KnownFibValues = new()
    {
        { 0, 0 },
        { 1, 1 },
        { 2, 1 },
        { 3, 2 },
        { 4, 3 },
        { 5, 5 },
        { 6, 8 },
        { 10, 55 },
        { 20, 6765 },
        { 30, 832040 },
        { 40, 102334155 }
    };

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void RecursiveFib_ReturnsCorrectValue(int n)
    {
        // Act
        var result = Fibonacci.RecursiveFib(n);

        // Assert
        Assert.Equal(KnownFibValues[n], result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(40)]
    public void MemoizationFib_ReturnsCorrectValue(int n)
    {
        // Act
        var result = Fibonacci.MemoizationFib(n);

        // Assert
        Assert.Equal(KnownFibValues[n], result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(40)]
    public void IterativeFib_ReturnsCorrectValue(int n)
    {
        // Act
        var result = Fibonacci.IterativeFib(n);

        // Assert
        Assert.Equal(KnownFibValues[n], result);
    }
}
