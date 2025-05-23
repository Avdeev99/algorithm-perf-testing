using Algorithms.Api;
using Xunit.Abstractions;

namespace Algorithms.Tests.Tasks;

public class MaxSubarrayTests
{
    private readonly ITestOutputHelper _output;

    public MaxSubarrayTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData(new[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 6)] // Standard case
    [InlineData(new[] { 1 }, 1)] // Single element
    [InlineData(new[] { -1 }, -1)] // Single negative element
    [InlineData(new[] { -2, -1, -3 }, -1)] // All negative
    [InlineData(new[] { 1, 2, 3, 4 }, 10)] // All positive
    public void AllImplementations_ReturnCorrectResult(int[] nums, int expected)
    {
        // Act
        var bruteForceResult = MaxSubarray.FindMaxSubarrayBruteForce(nums);
        var kadaneResult = MaxSubarray.FindMaxSubarrayKadane(nums);

        // Assert
        Assert.Equal(expected, bruteForceResult);
        Assert.Equal(expected, kadaneResult);
    }

    // Helper to generate test arrays
    private int[] GenerateRandomArray(int size, int minValue = -100, int maxValue = 100)
    {
        var random = new Random();
        var result = new int[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = random.Next(minValue, maxValue);
        }
        return result;
    }

    [Fact]
    public void BruteForce_FailsPerformanceTest()
    {
        // Arrange
        const int arraySize = 2000;
        const int timeThresholdMs = 10;
        var nums = GenerateRandomArray(arraySize);
        
        // Act
        var performance = MaxSubarray.MeasurePerformance(MaxSubarray.FindMaxSubarrayBruteForce, nums);
        
        // Output results
        _output.WriteLine($"Brute Force (size {arraySize}) time: {performance.ExecutionTimeMs}ms");
        
        // Assert - Should be slow for large inputs
        Assert.True(performance.ExecutionTimeMs > timeThresholdMs, 
            $"Brute force solution was unexpectedly fast: {performance.ExecutionTimeMs}ms <= {timeThresholdMs}ms");
    }

    [Fact]
    public void Kadane_PassesPerformanceTest()
    {
        // Arrange
        const int arraySize = 10000;
        const int timeThresholdMs = 10;
        var nums = GenerateRandomArray(arraySize);
        
        // Act
        var performance = MaxSubarray.MeasurePerformance(MaxSubarray.FindMaxSubarrayKadane, nums);
        
        // Output results
        _output.WriteLine($"Kadane (size {arraySize}) time: {performance.ExecutionTimeMs}ms");
        
        // Assert - Should be fast for large inputs
        Assert.True(performance.ExecutionTimeMs < timeThresholdMs, 
            $"Kadane solution exceeded time threshold: {performance.ExecutionTimeMs}ms > {timeThresholdMs}ms");
    }
} 