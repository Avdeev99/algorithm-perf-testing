using System.Diagnostics;
using Algorithms.Api;
using Xunit;
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
    public void TimeComplexity_RecursiveIsExponential()
    {
        // Test with larger values to ensure measurable time
        var results = new List<(int n, long ms)>();
        
        // Use larger values to ensure we see measurable time differences
        for (int n = 25; n <= 40; n += 3)
        {
            var perf = Fibonacci.MeasurePerformance(Fibonacci.RecursiveFib, n);
            
            // Ensure we never have zero time (add 1ms minimum)
            var timeMs = perf.ExecutionTimeMs == 0 ? 1 : perf.ExecutionTimeMs;
            results.Add((n, timeMs));
            
            _output.WriteLine($"Recursive Fib({n}): {timeMs} ms");
        }

        // Check for exponential growth pattern at the end of the sequence
        // Focus on the last few measurements where the exponential growth should be most evident
        bool foundExponentialGrowth = false;
        
        for (int i = 1; i < results.Count; i++)
        {
            var currentRatio = (double)results[i].ms / results[i - 1].ms;
            var linearRatio = (double)results[i].n / results[i - 1].n;
            
            _output.WriteLine($"Time ratio: {currentRatio:F2}, Linear ratio: {linearRatio:F2}");
            
            // If we find any instance where time grows significantly faster than linear,
            // we have evidence of super-linear (potentially exponential) growth
            if (currentRatio > linearRatio * 1.5 && results[i-1].n >= 30)
            {
                foundExponentialGrowth = true;
                _output.WriteLine($"Found exponential growth evidence: time ratio {currentRatio:F2} >> linear ratio {linearRatio:F2}");
            }
        }
        
        // Assert that we found evidence of exponential growth
        Assert.True(foundExponentialGrowth, 
            "Recursive Fibonacci should show evidence of exponential time complexity");
    }

    [Fact]
    public void TimeComplexity_MemoizationIsLinear()
    {
        // We'll test with reasonable n values for memoization
        var results = new List<(int n, long ms)>();
        
        for (int n = 10; n <= 80; n += 10)
        {
            var perf = Fibonacci.MeasurePerformance(Fibonacci.MemoizationFib, n);
            results.Add((n, perf.ExecutionTimeMs == 0 ? 1 : perf.ExecutionTimeMs)); // Avoid div by zero
            _output.WriteLine($"Memoization Fib({n}): {perf.ExecutionTimeMs} ms");
        }

        // Check that growth is roughly linear (or better)
        // We allow some variance as small inputs may have measurement inconsistencies
        for (int i = 1; i < results.Count; i++)
        {
            var timeRatio = (double)results[i].ms / results[i - 1].ms;
            var inputRatio = (double)results[i].n / results[i - 1].n;
            
            _output.WriteLine($"Time ratio: {timeRatio:F2}, Input ratio: {inputRatio:F2}");
            
            // For small n, timing might be inconsistent, so we're more lenient
            if (results[i-1].n >= 40)
            {
                Assert.True(timeRatio < inputRatio * 2, 
                    "Memoization Fibonacci should show roughly linear time complexity");
            }
        }
    }

    [Fact]
    public void TimeComplexity_IterativeIsLinear()
    {
        // We'll test with reasonable n values for iterative approach
        var results = new List<(int n, long ms)>();
        
        for (int n = 10; n <= 80; n += 10)
        {
            var perf = Fibonacci.MeasurePerformance(Fibonacci.IterativeFib, n);
            results.Add((n, perf.ExecutionTimeMs == 0 ? 1 : perf.ExecutionTimeMs)); // Avoid div by zero
            _output.WriteLine($"Iterative Fib({n}): {perf.ExecutionTimeMs} ms");
        }

        // Check that growth is roughly linear (or better)
        for (int i = 1; i < results.Count; i++)
        {
            var timeRatio = (double)results[i].ms / results[i - 1].ms;
            var inputRatio = (double)results[i].n / results[i - 1].n;
            
            _output.WriteLine($"Time ratio: {timeRatio:F2}, Input ratio: {inputRatio:F2}");
            
            // For small n, timing might be inconsistent, so we're more lenient
            if (results[i-1].n >= 40)
            {
                Assert.True(timeRatio < inputRatio * 2, 
                    "Iterative Fibonacci should show roughly linear time complexity");
            }
        }
    }

    [Fact]
    public void SpaceComplexity_Comparison()
    {
        const int n = 35; // Large enough to show differences but not too large
        
        var recursivePerf = Fibonacci.MeasurePerformance(Fibonacci.RecursiveFib, n);
        var memoPerf = Fibonacci.MeasurePerformance(Fibonacci.MemoizationFib, n);
        var iterativePerf = Fibonacci.MeasurePerformance(Fibonacci.IterativeFib, n);
        
        _output.WriteLine($"Recursive Fib({n}) memory: {recursivePerf.MemoryUsedBytes} bytes");
        _output.WriteLine($"Memoization Fib({n}) memory: {memoPerf.MemoryUsedBytes} bytes");
        _output.WriteLine($"Iterative Fib({n}) memory: {iterativePerf.MemoryUsedBytes} bytes");
        
        // The memoization approach should use more memory than iterative
        // due to the array allocation for memoization
        Assert.True(memoPerf.MemoryUsedBytes > iterativePerf.MemoryUsedBytes,
            "Memoization should use more memory than iterative approach");
            
        // Output performance comparison
        _output.WriteLine("\nPerformance Comparison:");
        _output.WriteLine($"Recursive: {recursivePerf.ExecutionTimeMs} ms, {recursivePerf.MemoryUsedBytes} bytes");
        _output.WriteLine($"Memoization: {memoPerf.ExecutionTimeMs} ms, {memoPerf.MemoryUsedBytes} bytes");
        _output.WriteLine($"Iterative: {iterativePerf.ExecutionTimeMs} ms, {iterativePerf.MemoryUsedBytes} bytes");
    }
}
