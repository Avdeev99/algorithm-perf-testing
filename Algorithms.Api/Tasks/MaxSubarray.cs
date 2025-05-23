using Algorithms.Api.Helpers;
using System.Diagnostics;

namespace Algorithms.Api;

/// <summary>
/// Given an integer array nums, find the subarray with the largest sum, and return its sum.
/// </summary>
public static class MaxSubarray
{
    /// <summary>
    /// Brute force approach that stores all possible subarrays.
    /// Time Complexity: O(n²)
    /// Space Complexity: O(n²)
    /// </summary>
    public static int FindMaxSubarrayBruteForce(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;

        // Create a 2D array to store all possible subarrays
        int[,] subarrays = new int[nums.Length, nums.Length];
        int maxSum = int.MinValue;

        // Fill the 2D array with subarray sums
        for (int i = 0; i < nums.Length; i++)
        {
            int currentSum = 0;
            for (int j = i; j < nums.Length; j++)
            {
                currentSum += nums[j];
                subarrays[i, j] = currentSum;
                maxSum = Math.Max(maxSum, currentSum);
            }
        }

        return maxSum;
    }

    /// <summary>
    /// Kadane's Algorithm - most efficient solution.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public static int FindMaxSubarrayKadane(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;

        int currentSum = nums[0];
        int maxSum = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            // Either start a new subarray or continue the previous one
            currentSum = Math.Max(nums[i], currentSum + nums[i]);
            maxSum = Math.Max(maxSum, currentSum);
        }

        return maxSum;
    }

    /// <summary>
    /// Helper method to measure performance
    /// </summary>
    public static AlgorithmPerformance MeasurePerformance(Func<int[], int> method, int[] nums)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        var result = method(nums);
        stopwatch.Stop();
        
        return new AlgorithmPerformance
        {
            Result = result,
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
        };
    }
} 