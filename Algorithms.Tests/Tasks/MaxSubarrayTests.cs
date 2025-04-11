using Algorithms.Api;

namespace Algorithms.Tests.Tasks;

public class MaxSubarrayTests
{
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
        var dpResult = MaxSubarray.FindMaxSubarrayDP(nums);
        var kadaneResult = MaxSubarray.FindMaxSubarrayKadane(nums);

        // Assert
        Assert.Equal(expected, bruteForceResult);
        Assert.Equal(expected, dpResult);
        Assert.Equal(expected, kadaneResult);
    }
} 