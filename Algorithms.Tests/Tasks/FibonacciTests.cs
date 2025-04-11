using Algorithms.Api;
using Xunit.Abstractions;

namespace Algorithms.Tests.Tasks;

public class FibonacciTests
{
    private readonly ITestOutputHelper _output;

    public FibonacciTests(ITestOutputHelper output)
    {
        _output = output;
    }

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

    [Fact]
    public void RecursiveFib_FailsPerformanceTest()
    {
        // Arrange
        const int n = 40;
        const int timeThresholdMs = 100;
        
        // Act
        var performance = Fibonacci.MeasurePerformance(Fibonacci.RecursiveFib, n);
        
        // Output results
        _output.WriteLine($"Recursive Fib({n}) time: {performance.ExecutionTimeMs}ms");
        
        // Assert - Recursive should be slow for large inputs
        Assert.True(performance.ExecutionTimeMs > timeThresholdMs, 
            $"Recursive solution should be slow, but completed in: {performance.ExecutionTimeMs}ms");
    }

    [Fact]
    public void MemoizationFib_PassesPerformanceTest()
    {
        // Arrange
        const int n = 40;
        const int timeThresholdMs = 100;
        
        // Act
        var performance = Fibonacci.MeasurePerformance(Fibonacci.MemoizationFib, n);
        
        // Output results
        _output.WriteLine($"Memoization Fib({n}) time: {performance.ExecutionTimeMs}ms");
        
        // Assert - Should be fast for large inputs
        Assert.True(performance.ExecutionTimeMs < timeThresholdMs, 
            $"Memoization solution exceeded time threshold: {performance.ExecutionTimeMs}ms > {timeThresholdMs}ms");
    }

    [Fact]
    public void IterativeFib_PassesPerformanceTest()
    {
        // Arrange
        const int n = 40;
        const int timeThresholdMs = 100;
        
        // Act
        var performance = Fibonacci.MeasurePerformance(Fibonacci.IterativeFib, n);
        
        // Output results
        _output.WriteLine($"Iterative Fib({n}) time: {performance.ExecutionTimeMs}ms");
        
        // Assert - Should be fast for large inputs
        Assert.True(performance.ExecutionTimeMs < timeThresholdMs, 
            $"Iterative solution exceeded time threshold: {performance.ExecutionTimeMs}ms > {timeThresholdMs}ms");
    }

    [Fact]
    public void ComparativePerformance_ShowsAlgorithmicDifferences()
    {
        // Arrange - Use a value that won't hang the test but shows differences
        const int n = 35;
        
        // Act - Measure execution time for all implementations
        var recursivePerf = Fibonacci.MeasurePerformance(Fibonacci.RecursiveFib, n);
        var memoizationPerf = Fibonacci.MeasurePerformance(Fibonacci.MemoizationFib, n);
        var iterativePerf = Fibonacci.MeasurePerformance(Fibonacci.IterativeFib, n);
        
        // Output results for visibility
        _output.WriteLine($"Recursive Fib({n}) time: {recursivePerf.ExecutionTimeMs}ms");
        _output.WriteLine($"Memoization Fib({n}) time: {memoizationPerf.ExecutionTimeMs}ms");
        _output.WriteLine($"Iterative Fib({n}) time: {iterativePerf.ExecutionTimeMs}ms");
        
        // Assert relationships
        Assert.True(recursivePerf.ExecutionTimeMs > memoizationPerf.ExecutionTimeMs, 
            "Recursive should be slower than Memoization");
        
        Assert.True(memoizationPerf.ExecutionTimeMs >= iterativePerf.ExecutionTimeMs, 
            "Memoization should be slower than or equal to Iterative");
    }
}
